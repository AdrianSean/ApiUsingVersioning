using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
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
            services.AddMvc(c =>
               c.Conventions.Add(new ApiExplorerGroupPerVersionConvention()) //  need this to ensure swagger maps to correct action
           );

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

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Web API", Version = "v1" });               
            });
        }   
    

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Web API");                
            });
        }
    }


    public class ApiExplorerGroupPerVersionConvention : IControllerModelConvention
    {
        public void Apply(ControllerModel controller)
        {
            var controllerNamespace = controller.ControllerType.Namespace; // e.g. "Controllers.v1"
            var apiVersion = controllerNamespace.Split('.').Last().ToLower();

            controller.ApiExplorer.GroupName = apiVersion;
        }
    }
}
