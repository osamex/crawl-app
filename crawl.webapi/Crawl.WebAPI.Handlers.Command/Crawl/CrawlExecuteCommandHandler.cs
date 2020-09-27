using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading;
using System.Threading.Tasks;
using Crawl.WebAPI.Common.Command.Crawl;
using Crawl.WebAPI.Common.Contract;
using Crawl.WebAPI.Common.Contract.Crawl;
using Crawl.WebAPI.Common.Contract.QueryFilter;
using Crawl.WebAPI.Common.DAL.Repositories;
using Crawl.WebAPI.Common.Domain.Entities;
using Crawl.WebAPI.Common.Enums;
using Crawl.WebAPI.Common.Helpers;
using FluentValidation;
using HtmlAgilityPack;
using MediatR;
using Serilog;

namespace Crawl.WebAPI.Handlers.Command.Crawl
{
	public class CrawlExecuteCommandHandler : INotificationHandler<CrawlExecuteCommand>
	{

		private readonly ILogger _logger;
		private readonly IValidatorFactory _validatorFactory;
		private readonly IImagesRepositoryAsync _imagesRepository;
		private readonly ISitesRepositoryAsync _sitesRepository;

		public CrawlExecuteCommandHandler(ILogger logger, IValidatorFactory validatorFactory, IImagesRepositoryAsync imagesRepository, ISitesRepositoryAsync sitesRepository)
		{
			_logger = logger;
			_validatorFactory = validatorFactory;
			_imagesRepository = imagesRepository;
			_sitesRepository = sitesRepository;
		}

		public async Task Handle(CrawlExecuteCommand command, CancellationToken cancellationToken)
		{
			_logger.Debug("Start RE-VALIDATE AND EXECUTE command: {@command}", command);

			try
			{
				var validator = _validatorFactory.GetValidator<CrawlRequestData>();
				if (validator == null)
				{
					throw new Exception("Can't find validator");
				}

				var validationResult = await validator.ValidateAsync(new ValidationContext<CrawlRequestData>(command.Request.RequestData), cancellationToken);
				if (validationResult.IsValid)
				{
					if (await Execute(command.Request, cancellationToken))
					{
						await SendNotification(new Notification
						{
							Message = "Images have been successfully downloaded",
							Type = MessageTypes.Information
						});
					}
					else
					{
						throw new Exception("Unhandled exception on execute command");
					}
				}
				else if (validationResult.Errors != null && validationResult.Errors.Any())
				{
					foreach (var error in validationResult.Errors)
					{
						await SendNotification(new Notification
						{
							Message = error.ErrorMessage,
							Type = MessageTypes.Warning
						});
					}
				}
				else
				{
					throw new Exception("Unhandled exception on validate command");
				}
			}
			catch (Exception exception)
			{
				_logger.Error(exception, "Internal error when RE-VALIDATE AND EXECUTE command: {@command}", command);
				await SendNotification(new Notification
				{
					Message = exception.Message,
					Type = MessageTypes.Error
				});
				throw;
			}
			finally
			{
				_logger.Debug("Finish RE-VALIDATE AND EXECUTE command: {@command}", command);
			}
		}

		public async Task<bool> Execute(Request<CrawlRequestData> request, CancellationToken cancellationToken)
		{
			var baseUrl = request.RequestData.Url;
			if (!baseUrl.StartsWith("http://", StringComparison.OrdinalIgnoreCase) &&
			    !baseUrl.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
			{
				if (!baseUrl.StartsWith("www."))
				{
					baseUrl = "http://www." + baseUrl;
				}
				else
				{
					baseUrl = "http://" + baseUrl;
				}
			}

			var client = new WebClient();
			var source = client.DownloadString(new Uri(baseUrl));
			var document = new HtmlDocument();
			document.LoadHtml(source);
			var imageUrls = document.DocumentNode.Descendants("img").Select(i => i.Attributes["src"]).Select(s => s.Value).ToList();
			var site = await _sitesRepository.FirstOrDefault(f => f.Url == baseUrl);

			if (site == null)
			{
				site = new SiteEntity { Url = baseUrl };
				var dbKey = await _sitesRepository.Add(site);
				site = await _sitesRepository.FirstOrDefault(f => f.DbKey == dbKey);
			}

			if (site == null)
			{
				throw new Exception("Error when create entity");
			}

			var totalImages = imageUrls.Count;
			var addedImages = 0;
			var updatedImages = 0;
			var omittedImages = 0;
			foreach (var imageUrl in imageUrls)
			{
				try
				{
					string fullImageUrl;
					if (imageUrl.StartsWith("//"))
					{
						fullImageUrl = "http:" + imageUrl;
					}
					else
					{
						fullImageUrl = imageUrl;
					}

					var webClient = new WebClient();
					Stream stream;
					try
					{
						stream = await webClient.OpenReadTaskAsync(fullImageUrl);
					}
					catch (Exception e)
					{
						stream = await webClient.OpenReadTaskAsync(baseUrl + fullImageUrl);
					}

					var memoryStream = new MemoryStream();
					await stream.CopyToAsync(memoryStream, cancellationToken);
					var imageBytes = memoryStream.ToArray();
					var imageEntity = await _imagesRepository.GetFiltered<ImageEntity>(f => f.ImageUrl == fullImageUrl,
						new QueryFilterRequest
						{
							SelectedPageSize = 10,
							CurrentPageNumber = 1,
							OrderBy = new OrderData
							{
								FiledName = "Version",
								Desc = true
							}
						});

					if (imageEntity.DataList.Any())
					{
						var lastImage = imageEntity.DataList.FirstOrDefault();
						if (lastImage != null)
						{
							if (lastImage.Image.ToString() != imageBytes.ToString())
							{
								var newVersion = lastImage.Version++;
								long newImageVersionDbKey = await _imagesRepository.Add(new ImageEntity
								{
									ImageUrl = fullImageUrl,
									SiteDbKey = site.DbKey,
									Image = imageBytes,
									Version = newVersion
								});
								updatedImages++;
								_logger.Debug($"Added new version ({newVersion}) of image dbKey: {newImageVersionDbKey}");
							}
							else
							{
								omittedImages++;
							}
						}
						else
						{
							_logger.Debug("Error when added new image version");
						}
					}
					else
					{
						long newImageDbKey = await _imagesRepository.Add(new ImageEntity
						{
							ImageUrl = fullImageUrl,
							SiteDbKey = site.DbKey,
							Image = imageBytes,
							Version = 0
						});
						addedImages++;
						_logger.Debug($"Added new image dbKey: {newImageDbKey}");
					}
				}
				catch (Exception e)
				{
					_logger.Error(e, e.Message);
				}
			}

			await SendNotification(new Notification
			{
				Type = MessageTypes.Information,
				Message = $"Total images: <b>{totalImages}</b><br>" +
				          $"Added new: <b>{addedImages}</b><br>" +
				          $"Updated (create next version): <b>{updatedImages}</b><br>" +
									$"Omitted images: <b>{omittedImages}</b><br>"
			});
			return true;
		}

		private static async Task SendNotification(Notification notification)
		{
			var apiUrl = ConfigurationHelper.GetConfigSection<string>("ApiUrl");
			var client = new HttpClient();
			var content = new ObjectContent<Notification>(notification, new JsonMediaTypeFormatter());
			var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"{apiUrl}/api/notification/send")
			{
				Content = content
			};

			await client.SendAsync(requestMessage).ConfigureAwait(false);
		}
	}
}