using System;
using System.Threading;
using System.Threading.Tasks;
using Crawl.WebAPI.Common.Contract;
using Crawl.WebAPI.Common.Contract.Auth;
using Crawl.WebAPI.Common.DAL.Repositories;
using Crawl.WebAPI.Common.Helpers;
using Crawl.WebAPI.Common.Query.Auth;
using MediatR;
using Serilog;

namespace Crawl.WebAPI.Handlers.Query.Auth
{
	public class SignInQueryHandler : IRequestHandler<SignInQuery, Response<AuthResponseData>>
	{

		private readonly IUsersRepositoryAsync _usersRepository;
		public SignInQueryHandler(IUsersRepositoryAsync usersRepository)
		{
			_usersRepository = usersRepository;
		}

		public async Task<Response<AuthResponseData>> Handle(SignInQuery query, CancellationToken cancellationToken)
		{
			var response = new Response<AuthResponseData>
			{
				RequestCreated = query.Request.Created,
				RequestId = query.Request.Id,
				ResponseData = null,
			};

			try
			{
				var userOnDb = await _usersRepository.GetUser(query.Request.RequestData.Email);
				if (userOnDb != null)
				{
					if (PasswordHelper.VerifyPasswordHash(query.Request.RequestData.Password, userOnDb.PasswordHash, userOnDb.PasswordSalt))
					{
						var systemAuthSecretKey = ConfigurationHelper.GetConfigSection<string>("JwtToken:SystemAuthSecretKey");
						var barerTokenValidPeriodHours = ConfigurationHelper.GetConfigSection<int>("JwtToken:ValidPeriodHours");
						response.ResponseData = AuthHelper.CreateAuthResponseData(
							userOnDb.AppKey,
							userOnDb.EMail, systemAuthSecretKey,
							new JwtData
							{
								UserAppKey = userOnDb.AppKey,
								CreateDate = DateTime.Now,
								ExpirationDate = DateTime.Now.AddHours(barerTokenValidPeriodHours)
							});
					}
				}
			}
			catch (Exception e)
			{
				Log.Error(e, e.Message);
				response.Errors.Add(e.Message);
			}

			return response;
		}
	}
}