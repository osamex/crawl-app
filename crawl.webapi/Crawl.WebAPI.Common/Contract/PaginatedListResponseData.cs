using System;
using System.Linq;
using AutoMapper;
using Crawl.WebAPI.Common.Contract.QueryFilter;
using Crawl.WebAPI.Common.Domain.Entities;

namespace Crawl.WebAPI.Common.Contract
{
	public class PaginatedListResponseData<TResponseData> where TResponseData : class
	{
		public int TotalPages { get; set; }
		public long TotalElementsCount { get; set; }
		public bool HasPrevious { get; set; }
		public bool HasNext { get; set; }
		public TResponseData[] DataList { get; set; }

		public static PaginatedListResponseData<TResponseData> Create(IQueryable<BaseEntity> source, QueryFilterRequest filterRequest, IMapper mapper)
		{
			if (source == null || filterRequest == null) throw new Exception("Error when create paginated list");
			var totalElementCount = source.LongCount();
			var totalPages = (int)Math.Ceiling(totalElementCount / (double)filterRequest.SelectedPageSize);
			if (filterRequest.OrderBy != null)
			{
				source = filterRequest.OrderBy.Desc ? source.OrderByDescending(o => filterRequest.OrderBy.FiledName) : source.OrderBy(o => filterRequest.OrderBy.FiledName);
			}

			return new PaginatedListResponseData<TResponseData>
			{
				TotalPages = totalPages,
				TotalElementsCount = totalElementCount,
				HasPrevious = filterRequest.CurrentPageNumber > 1,
				HasNext = filterRequest.CurrentPageNumber < totalPages,
				DataList = mapper.Map<BaseEntity[], TResponseData[]>(source.Skip((filterRequest.CurrentPageNumber - 1) * filterRequest.SelectedPageSize).Take(filterRequest.SelectedPageSize).ToArray())
			};
		}
	}
}