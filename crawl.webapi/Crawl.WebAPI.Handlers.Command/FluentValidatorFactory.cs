using System;
using Autofac;
using FluentValidation;

namespace Crawl.WebAPI.Handlers.Command
{
	public class FluentValidatorFactory : ValidatorFactoryBase
	{
		private readonly IComponentContext context;

		public FluentValidatorFactory(IComponentContext context)
		{
			this.context = context;
		}

		public override IValidator CreateInstance(Type validatorType)
		{
			return context.Resolve(validatorType) as IValidator;
		}
	}
}