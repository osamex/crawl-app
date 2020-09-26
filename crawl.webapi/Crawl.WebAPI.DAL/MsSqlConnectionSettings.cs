namespace Crawl.WebAPI.DAL
{
	public class MsSqlConnectionSettings
	{
		public string ConnectionString { get; set; }
		public bool? ForcedExecuteSeed { get; set; }
	}
}