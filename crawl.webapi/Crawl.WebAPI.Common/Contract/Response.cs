using System;

namespace Crawl.WebAPI.Common.Contract
{
	public class Response<TResponseData>
	{
		public Response()
		{
			Id = Guid.NewGuid();
			Created = DateTime.UtcNow;
			RequestId = Guid.Empty;
			RequestCreated = DateTime.MinValue;
			ResponseData = default;
		}

		public Guid Id { get; set; }
		public DateTime Created { get; set; }
		public Guid RequestId { get; set; }
		public DateTime RequestCreated { get; set; }
		public bool IsSuccess { get; set; }
		public TResponseData ResponseData { get; set; }
	}
}