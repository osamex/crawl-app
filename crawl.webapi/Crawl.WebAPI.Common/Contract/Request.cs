using System;

namespace Crawl.WebAPI.Common.Contract
{
	public class Request<TRequestData>
	{
		public Request()
		{
			Id = Guid.NewGuid();
			Created = DateTime.UtcNow;
			RequestData = default;
		}

		public Guid Id { get; set; }
		public DateTime Created { get; set; }
		public TRequestData RequestData { get; set; }
	}
}