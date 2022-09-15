using Autofac;
using Autofac.Extensions.DependencyInjection;
using Borigran.OneData.Authorization;
using Borigran.OneData.Authorization.Dependencies;
using Borigran.OneData.Authorization.Impl;
using Borigran.OneData.Platform.Dependencies;
using Borigran.OneData.WebApi.AppExtensions;
using Borigran.OneData.WebApi.Pipeline;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;

namespace Borigran.OneData.WebApi
{
    public class Startup
    {
        private AuthOptions authOptions;

        private readonly AssemblyScanner assemblyScanner = new AssemblyScanner();

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public ILifetimeScope AutofacContainer { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            authOptions = Configuration.GetSection("AuthOptions").Get<AuthOptions>();
            services.AddAutoMapper(assemblyScanner.AssembliesToScan());
            services.AddControllers();
            services.AddOneDataSwaggerGen();
            services.AddOneDataAuthentication(JwtTokenGenerator.TokenValidationParameters(authOptions));
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            // Register your own things directly with Autofac here. Don't
            // call builder.Populate(), that happens in AutofacServiceProviderFactory
            // for you.
            builder.RegisterModule(new OneDataAutofacModule(Configuration));
            builder.RegisterModule(new AuthorithationModule(authOptions));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddLog4Net();
            app.UseMiddleware<ErrorHandlingMiddleWare>();

            this.AutofacContainer = app.ApplicationServices.GetAutofacRoot();

            if (env.IsDevelopment())
            {
                app.UseOneDataSwagger();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "api/{controller}/{action=Index}/{id?}");
            });
        }
    }
}
