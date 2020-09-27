using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Autofac;
using AutoMapper;
using Crawl.WebAPI.Hubs;
using MediatR;
using MediatR.Pipeline;
using Module = Autofac.Module;

namespace Crawl.WebAPI.Autofac
{
	public class AutofacModule : Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			base.Load(builder);
			var assembly = Assembly.GetExecutingAssembly();

			//automapper
			builder.RegisterAssemblyTypes(assembly).Where(t => typeof(Profile).IsAssignableFrom(t)).As<Profile>();
			builder.RegisterAssemblyTypes(assembly).AsClosedTypesOf(typeof(ITypeConverter<,>)).AsSelf().InstancePerDependency();
			builder.Register(c => new MapperConfiguration(cfg =>
			{
				var profiles = c.Resolve<IEnumerable<Profile>>();
				foreach (var profile in profiles)
				{
					cfg.AddProfile(profile);
				}
			})).AsSelf().SingleInstance();
			builder.Register(c => c.Resolve<MapperConfiguration>().CreateMapper(c.Resolve)).As<IMapper>();

			//mediatr
			builder.RegisterAssemblyTypes(typeof(IMediator).GetTypeInfo().Assembly).AsImplementedInterfaces();
			var mediatrOpenTypes = new[]
			{
				typeof(IRequestHandler<,>),
				typeof(IRequestExceptionHandler<,,>),
				typeof(IRequestExceptionAction<,>),
				typeof(INotificationHandler<>),
			};
			var appAssemblies = new List<Assembly>(Directory
				.EnumerateFiles(Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location), "*.dll",
					SearchOption.AllDirectories)
				.Where(fullFilePath => Path.GetFileName(fullFilePath).Contains("Handler")).ToList()
				.Select(Assembly.LoadFrom));

			foreach (var mediatrOpenType in mediatrOpenTypes)
			{
				foreach (Assembly appAssembly in appAssemblies)
				{
					builder.RegisterAssemblyTypes(appAssembly).AsClosedTypesOf(mediatrOpenType).AsImplementedInterfaces();
				}
			}

			builder.RegisterType<NotificationHub>().AsSelf().SingleInstance();
		}
	}
}