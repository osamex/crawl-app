using Autofac;
using Crawl.WebAPI.Common.Contract.Crawl;
using Crawl.WebAPI.Handlers.Command.Crawl;
using FluentValidation;

namespace Crawl.WebAPI.Handlers.Command.Autofac
{
	public class AutofacModule : Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterType<FluentValidatorFactory>().As<IValidatorFactory>();
			builder.RegisterType<CrawlExecuteValidator>().As<IValidator<CrawlRequestData>>();
			base.Load(builder);
		}
	}
}