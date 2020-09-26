using Autofac.Extensions.DependencyInjection;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Crawl.WebAPI
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			var webHost = CreateWebHostBuilder(args).Build();

			using (var scope = webHost.Services.CreateScope())
			{
				var services = scope.ServiceProvider;
				if (services != null)
				{
					Log.Information("Starting web host");
				}
			}

			await webHost.RunAsync();
		}
		public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
			new WebHostBuilder()
				.UseKestrel()
				.ConfigureServices(services => services.AddAutofac())
				.UseContentRoot(Directory.GetCurrentDirectory())
				.UseIISIntegration()
				.UseSerilog()
				.UseStartup<Startup>();
	}
}
