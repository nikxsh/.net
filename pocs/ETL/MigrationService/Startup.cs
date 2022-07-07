using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MigrationService.Repository;

namespace MigrationService
{
	public class Startup
	{
		readonly string MigrationAllowSpecificOrigins = "_migrationAllowSpecificOrigins";

		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddControllers();
			services.AddSingleton<ISqlRepository, SqlRepository>();
			services.AddSingleton<IMongoRepository, MongoRepository>();

			services.AddCors(options =>
			{
				options.AddPolicy(MigrationAllowSpecificOrigins, builder =>
				{
					builder
					.WithOrigins("http://localhost:4200")
					.AllowAnyHeader()
					.AllowAnyMethod();
				});
			});
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseHttpsRedirection();

			app.UseCors(MigrationAllowSpecificOrigins);

			app.UseRouting();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}
