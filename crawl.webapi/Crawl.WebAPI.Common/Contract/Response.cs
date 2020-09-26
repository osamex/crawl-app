using System;
using System.Collections.Generic;
using System.Linq;

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
			Errors = new List<string>();
		}

		public Guid Id { get; set; }
		public DateTime Created { get; set; }
		public Guid RequestId { get; set; }
		public DateTime RequestCreated { get; set; }
		public bool IsSuccess => !Errors.Any();
		public TResponseData ResponseData { get; set; }
		public List<string> Errors { get; set; }
	}
}