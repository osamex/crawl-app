namespace Crawl.WebAPI.DAL.MsSQL
{
	public class MsSqlConnectionSettings
	{
		public string ConnectionString { get; set; }
		public bool? ForcedExecuteSeed { get; set; }
	}
}