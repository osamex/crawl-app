using Autofac;
using Crawl.WebAPI.Common.DAL.Repositories;
using Crawl.WebAPI.DAL.MsSQL.Repositories;

namespace Crawl.WebAPI.DAL.MsSQL.Autofac
{
	public class AutofacModule : Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterType<UsersRepositoryAsync>().As<IUsersRepositoryAsync>();
			base.Load(builder);
		}
	}
}