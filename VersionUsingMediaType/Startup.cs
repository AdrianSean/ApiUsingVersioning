using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace VersionUsingMediaType
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
            services.AddMvc();
            services.AddApiVersioning(o =>
            {
                o.ReportApiVersions = true;
                o.AssumeDefaultVersionWhenUnspecified = true;
                o.ApiVersionReader = new MediaTypeApiVersionReader();                
                o.DefaultApiVersion = new ApiVersion(2, 0);

                // alternative to setting [ApiVersion("x.x")] in the controller, use the conventions property
                o.Conventions.Controller<Controllers.v1.ValuesController>()
                    .HasApiVersion(new ApiVersion(1, 0)).HasDeprecatedApiVersion(new ApiVersion(1, 0));

                o.Conventions.Controller<Controllers.v2.ValuesController>().HasApiVersion(new ApiVersion(2, 0));
                o.Conventions.Controller<Controllers.v3.ValuesController>().HasApiVersion(new ApiVersion(3, 0));
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
        }
    }
}
