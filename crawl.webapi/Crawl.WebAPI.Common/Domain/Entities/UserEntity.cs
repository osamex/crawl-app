namespace Crawl.WebAPI.Common.Domain.Entities
{
	public class UserEntity : BaseEntity
	{
		public UserEntity()
		{
			EMail = string.Empty;
			PasswordHash = null;
			PasswordSalt = null;
		}

		public string EMail { get; set; }
		public byte[] PasswordHash { get; set; }
		public byte[] PasswordSalt { get; set; }
	}
}