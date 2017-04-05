using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using CnBlogs.Core.Utils;
using CnBlogs.Core.Repository;
using CnBlogs.Core.Services;
using CnBlogs.Core.Filters;

namespace CnBlogs.Core
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IEmailUtil, EmailUtil>();
            services.AddTransient<IEmailUtilSettings, EmailUtilSettings>();
            services.AddTransient<ICaptchaCodeUtil, CaptchaCodeUtil>();
            services.AddTransient<IInputValidatorUtil, InputValidatorUtil>();
            services.AddTransient<IDataProtectorUtil, DataProtectorUtil>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IEmailRepository, EmailRepository>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IRepositorySettings, RepositorySettings>();
            services.AddTransient<ICookieSettings, CookieSettings>();
            services.AddTransient<IBlogApplyRepository, BlogApplyRepository>();
            services.AddTransient<IBlogApplyService, BlogApplyService>();

            services.AddMvc();
            services.AddDataProtection();
            services.AddSession();
            services.AddScoped<LoginActionFilter>();
            services.AddScoped<ManagerActionFilter>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Shared/Error");
            }

            app.UseStaticFiles();
            app.UseSession();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
