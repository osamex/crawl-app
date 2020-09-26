using System;

namespace Crawl.WebAPI.Common.Domain.Entities
{
	public class BaseEntity : ICloneable
	{
		public BaseEntity()
		{
			var nowUtc = DateTime.UtcNow;
			DbKey = 0;
			AppKey = Guid.Empty;
		}

		public long DbKey { get; set; }

		public Guid AppKey { get; set; }

		public object Clone()
		{
			return MemberwiseClone();
		}
	}
}