using System;
using System.Linq;
using Crawl.WebAPI.Common.DAL;
using Crawl.WebAPI.Common.Domain.Entities;
using Crawl.WebAPI.Common.Helpers;
using Microsoft.EntityFrameworkCore;

namespace Crawl.WebAPI.DAL.MsSQL
{
	public class DataBaseContext : DbContext, IDataContext
	{
		public DbSet<UserEntity> Users { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			var config = ConfigurationHelper.GetConfigSection<MsSqlConnectionSettings>("Connection");
			optionsBuilder.UseSqlServer(config.ConnectionString);
			ForcedExecuteSeed = config.ForcedExecuteSeed;
		}

		public bool Migrate()
		{
			Database.Migrate();
			return true;
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			#region Users

			modelBuilder.Entity<UserEntity>().ToTable("Users").HasKey(x => x.DbKey);
			modelBuilder.Entity<UserEntity>().Property(x => x.DbKey).ValueGeneratedOnAdd();
			modelBuilder.Entity<UserEntity>().Property(x => x.AppKey).IsRequired();
			modelBuilder.Entity<UserEntity>().Property(x => x.EMail).IsRequired();
			modelBuilder.Entity<UserEntity>().Property(x => x.PasswordHash).IsRequired();
			modelBuilder.Entity<UserEntity>().Property(x => x.PasswordSalt).IsRequired();

			#endregion

			base.OnModelCreating(modelBuilder);
		}

		public bool? ForcedExecuteSeed { get; private set; }

		public bool Seed()
		{
			if (!Users.Any(a => a.EMail.Equals("demo@cp-expert.pl")))
			{
				PasswordHelper.CreatePasswordHash("P@ssw0rd", out var passwordHash, out var passwordSalt);
				var user = new UserEntity
				{
					AppKey = Guid.NewGuid(),
					EMail = "demo@cp-expert.pl",
					PasswordHash = passwordHash,
					PasswordSalt = passwordSalt
				};
				Users.Add(user);
				base.SaveChanges();
			}
			return true;
		}
	}
}