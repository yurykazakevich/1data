using Autofac;
using Autofac.Extensions.DependencyInjection;
using Borigran.OneData.Authorization;
using Borigran.OneData.Authorization.Dependencies;
using Borigran.OneData.Authorization.Impl;
using Borigran.OneData.Platform.Dependencies;
using Borigran.OneData.WebApi.AppExtensions;
using Borigran.OneData.WebApi.Pipeline.ExceptionHandling;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Reflection;
using AssemblyScanner = Borigran.OneData.Platform.Dependencies.AssemblyScanner;

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
            services.AddCors(options =>
            {
                string[] allowedHosts = Configuration.GetValue<string>("ClientUrls")
                    .Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

                options.AddPolicy(CorsPolicyNames.ClientApp, builder => builder
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .WithOrigins(allowedHosts));

                options.AddPolicy(CorsPolicyNames.ExternalEndPoint, builder => builder
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowAnyOrigin());

                options.DefaultPolicyName = CorsPolicyNames.ClientApp;

            });
            services.AddControllers();
            services.AddOneDataSwaggerGen();
            services.AddOneDataAuthentication(JwtTokenGenerator.TokenValidationParameters(authOptions));

            services.AddRouting(r => r.SuppressCheckForUnhandledSecurityMetadata = true);

            //services.AddFluentValidationAutoValidation();
            //services.AddValidatorsFromAssembly(Assembly.GetCallingAssembly());
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            // Register your own things directly with Autofac here. Don't
            // call builder.Populate(), that happens in AutofacServiceProviderFactory
            // for you.
            builder.RegisterModule(new OneDataAutofacModule(Configuration));
            builder.RegisterModule(new AuthorithationModule(authOptions));

            builder.RegisterAssemblyTypes(this.GetType().Assembly)
                .Where(t => t.IsClosedTypeOf(typeof(IValidator<>)))
                .AsImplementedInterfaces().InstancePerDependency();

            builder.RegisterAssemblyTypes(this.GetType().Assembly)
                .Where(t => t.IsClosedTypeOf(typeof(IExceptionHandler<>)))
                .AsImplementedInterfaces().InstancePerDependency();
            
            builder.RegisterType<ExceptionHandlerFactory>()
                .As<IExceptionHandlerFactory>()
                .InstancePerLifetimeScope();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseCors();
            app.Use((context, next) =>
            {
                context.Items["__CorsMiddlewareInvoked"] = true;
                return next();
            });

            loggerFactory.AddLog4Net();
            app.UseMiddleware<ExceptionHandlingMiddleware>();

            AutofacContainer = app.ApplicationServices.GetAutofacRoot();

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
