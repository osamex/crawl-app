using System;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Text;
using Crawl.WebAPI.Common.Contract.Crawl;
using FluentValidation;

namespace Crawl.WebAPI.Handlers.Command.Crawl
{
	public class CrawlExecuteValidator : AbstractValidator<CrawlRequestData>
	{
		public CrawlExecuteValidator()
		{
			RuleFor(data => data)
				.Cascade(CascadeMode.Stop)
				.NotNull()
				.NotEmpty()
				.WithMessage("Request is empty or n ull");

			RuleFor(data => data.Url)
				.Cascade(CascadeMode.Stop)
				.NotNull()
				.NotEmpty()
				.WithMessage("Request is empty or n ull")
				.Must(x => Uri.IsWellFormedUriString(x, UriKind.RelativeOrAbsolute))
				.WithMessage("Is not valid URL")
				.Must(url =>
				{
					try
					{
						if (!url.StartsWith("http://", StringComparison.OrdinalIgnoreCase) &&
						    !url.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
						{
							url = "http://" + url;
						}

						var request = WebRequest.Create(url) as HttpWebRequest;
						request.Method = "HEAD";
						var response = request.GetResponse() as HttpWebResponse;
						var statusCode = response.StatusCode;
						response.Close();
						return statusCode == HttpStatusCode.OK;
					}
					catch(Exception e)
					{
						return false;
					}
				}).WithMessage("Web site is not response");
		}
	}
}