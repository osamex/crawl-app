using System;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace Crawl.WebAPI.Common.Helpers
{
	public static class ConfigurationHelper
	{
		public static T GetConfigSection<T>(string configSectionKey)
		{
			return new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json", false, true)
				.AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", true)
				.AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT")}.json", true)
				.AddEnvironmentVariables()
				.Build()
				.GetSection(configSectionKey).Get<T>();
		}

		public static T GetConfigSection<T>()
		{
			return new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json", false, true)
				.AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", true)
				.AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT")}.json", true)
				.AddEnvironmentVariables()
				.Build()
				.GetSection(typeof(T).Name).Get<T>();
		}
	}
}