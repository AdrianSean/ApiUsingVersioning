using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using System.Linq;

namespace VersionUsingUrl
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // format the version as "'v'major[.minor][-status]"
            services.AddMvc();
            services.AddMvcCore().AddVersionedApiExplorer(o => o.GroupNameFormat = "'v'VVV");

            services.AddApiVersioning(o =>
            {
                o.ReportApiVersions = true;
                o.AssumeDefaultVersionWhenUnspecified = true;
                o.ApiVersionReader = new UrlSegmentApiVersionReader();
                o.DefaultApiVersion = new ApiVersion(2, 0);

                // alternative to setting [ApiVersion("x.x")] in the controller, use the conventions property
                o.Conventions.Controller<Controllers.v1.ValuesController>()
                    .HasApiVersion(new ApiVersion(1, 0)).HasDeprecatedApiVersion(new ApiVersion(1, 0));

                o.Conventions.Controller<Controllers.v2.ValuesController>().HasApiVersion(new ApiVersion(2, 0));

                o.Conventions.Controller<Controllers.v3.ValuesController>().HasApiVersion(new ApiVersion(3, 0));
            });

            services.AddSwaggerGen(
             options =>
             {
                 var provider = services.BuildServiceProvider()
                              .GetRequiredService<IApiVersionDescriptionProvider>();

                 foreach (var description in provider.ApiVersionDescriptions)
                 {
                     options.SwaggerDoc(
                     description.GroupName,
                       new Info()
                       {
                           Title = $"Sample API {description.ApiVersion}",
                           Version = description.ApiVersion.ToString()
                       });
                 }
             });
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IApiVersionDescriptionProvider provider)
        {
            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUI(
                options =>
                {
                    foreach (var description in provider.ApiVersionDescriptions)
                    {
                        options.SwaggerEndpoint(
                            $"/swagger/{description.GroupName}/swagger.json",
                            description.GroupName.ToUpperInvariant());
                    }
                });
        }
    }
}
