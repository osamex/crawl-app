namespace Crawl.WebAPI.Common.Contract.QueryFilter
{
	public class QueryFilterRequest
	{

		public QueryFilterRequest()
		{
			maxPageSize = 200;
			CurrentPageNumber = 1;
			_selectedPageSize = 10;
			OrderBy = null;
		}

		private readonly int maxPageSize;
		public int CurrentPageNumber { get; set; }

		private int _selectedPageSize;
		public int SelectedPageSize
		{
			get => _selectedPageSize;
			set => _selectedPageSize = (value > maxPageSize) ? maxPageSize : value;
		}

		public OrderData OrderBy { get; set; }
	}
}