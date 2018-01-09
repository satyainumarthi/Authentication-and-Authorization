using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace WebAPI
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
            services.AddMvcCore().AddAuthorization().AddJsonFormatters();

            services.AddAuthentication("Bearer") // it is a Bearer token
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = "http://localhost:49381"; //Identity Server URL
                    options.RequireHttpsMetadata = false; // make it false since we are not using https
                    options.ApiName = "api1"; //api name which should be registered in IdentityServer
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseAuthentication(); // add the Authentication middleware

            app.UseMvc();
        }
    }
}
