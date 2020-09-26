using Crawl.WebAPI.Common.Contract;
using Crawl.WebAPI.Common.Contract.Auth;
using MediatR;

namespace Crawl.WebAPI.Common.Query.Auth
{
	public class SignInQuery : IRequest<Response<AuthResponseData>>
	{
		public Request<AuthRequestData> Request { get; set; }
	}
}