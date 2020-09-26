using System;

namespace Crawl.WebAPI.Common.Contract.Auth
{
	public class AuthResponseData
	{
		public Guid UserAppKey { get; set; }
		public string UserEmail { get; set; }
		public string BarerToken { get; set; }
	}
}