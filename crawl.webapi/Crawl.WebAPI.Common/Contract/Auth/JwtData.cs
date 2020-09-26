using System;

namespace Crawl.WebAPI.Common.Contract.Auth
{
	public class JwtData
	{
		public Guid UserAppKey { get; set; }
		public DateTime CreateDate { get; set; }
		public DateTime ExpirationDate { get; set; }
	}
}