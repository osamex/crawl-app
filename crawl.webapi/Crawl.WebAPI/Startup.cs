using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using Crawl.WebAPI.DAL.MsSQL;
using Crawl.WebAPI.Hubs;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Serilog;

namespace Crawl.WebAPI
{
	public class Startup
	{
		public Startup(IHostEnvironment environment)
		{
			_configuration = new ConfigurationBuilder()
				.AddJsonFile("appsettings.json", false, true)
				.AddJsonFile($"appsettings.{environment.EnvironmentName}.json", true)
				.AddEnvironmentVariables()
				.Build();
		}

		public readonly IConfiguration _configuration;
		public IContainer ApplicationContainer { get; private set; }

		public IServiceProvider ConfigureServices(IServiceCollection services)
		{
			try
			{
				Log.Logger = new LoggerConfiguration()
					.ReadFrom.Configuration(_configuration)
					.CreateLogger();
				Log.Information("Application started...");

				var builder = new ContainerBuilder();

				var appAssemblies = new List<Assembly>(Directory
					.EnumerateFiles(Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location), "*.dll",
						SearchOption.AllDirectories)
					.Where(fullFilePath => Path.GetFileName(fullFilePath).StartsWith("Crawl.")).ToList()
					.Select(Assembly.LoadFrom));

				appAssemblies.ForEach(assembly => builder.RegisterAssemblyModules(assembly));
				services.AddMvc()
					.AddFluentValidation(fvc => fvc.RegisterValidatorsFromAssemblyContaining<Startup>())
					.AddJsonOptions(options => {
						options.JsonSerializerOptions.PropertyNamingPolicy = null;
						options.JsonSerializerOptions.PropertyNameCaseInsensitive = false;
					});
				services.AddMemoryCache();
				services.AddResponseCaching();
				services.AddSingleton(Log.Logger);
				services.AddControllers();
				services.AddCors(options =>
				{
					options.AddPolicy("CorsPolicy", corsPolicyBuilder => corsPolicyBuilder
						.WithOrigins(_configuration.GetSection("Cors:Origins").Value.Split(';'))
						.AllowAnyMethod()
						.AllowAnyHeader()
						.AllowCredentials());
				});

				services.AddMediatR(Assembly.GetExecutingAssembly());
				services.AddDbContext<DataBaseContext>();
				services.AddSignalR().AddJsonProtocol(options =>
				{
					options.PayloadSerializerOptions.PropertyNamingPolicy = null;
					options.PayloadSerializerOptions.PropertyNameCaseInsensitive = false;
				});

				services.AddAuthentication(cfg =>
				{
					cfg.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
					cfg.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
					cfg.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
				}).AddJwtBearer(cfg =>
				{
					cfg.RequireHttpsMetadata = false;
					cfg.SaveToken = true;
					cfg.TokenValidationParameters = new TokenValidationParameters
					{
						ValidateIssuer = false,
						ValidateAudience = false,
						ValidateLifetime = true,
						IssuerSigningKey = new SymmetricSecurityKey(
							Encoding.UTF8.GetBytes(_configuration.GetSection("JwtToken:SystemAuthSecretKey").Value))
					};
				});
				services.AddAutoMapper(appAssemblies);
				services.AddSwaggerGen();

				builder.Populate(services);
				ApplicationContainer = builder.Build();
				return new AutofacServiceProvider(ApplicationContainer);
			}
			catch (Exception e)
			{
				Log.Error(e, "Error when start WebAPI");
				throw;
			}
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			try
			{
				if (env.IsDevelopment())
				{
					app.UseDeveloperExceptionPage();
				}

				app.UseHttpsRedirection();
				app.UseRouting();
				app.UseCors("CorsPolicy");
				app.UseAuthentication();
				app.UseAuthorization();
				app.UseEndpoints(endpoints =>
					{
						endpoints.MapControllers();
						endpoints.MapHub<NotificationHub>("/notificationhub");
					});

				app.UseSwagger();
				app.UseSwaggerUI(c =>
				{
					c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
				});

				MigrateAndSeedDataBase(app);

				Log.Information("Application start SUCCESS");
			}
			catch (Exception e)
			{
				Log.Error(e, "Application start ERROR");
				throw;
			}
		}

		private static void MigrateAndSeedDataBase(IApplicationBuilder app)
		{
			using var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
			using var dbContext = serviceScope.ServiceProvider.GetService<DataBaseContext>();
			if (dbContext.Migrate())
			{
				dbContext.Seed();
			}
		}
	}
}
