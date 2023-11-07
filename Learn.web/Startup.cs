using System;
using Learn.Core.Services;
using Learn.Core.Services.Interfaces;
using Learn.DataLayer.Context;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Learn.Core.Convertors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.Features;
using System.Threading;
using ElmahCore.Mvc;
using System.IO;
using Microsoft.AspNetCore.DataProtection;
using System.Threading.Tasks;

namespace Learn.web
{
    public class Startup
    {
        public IConfiguration Configuration { get; set; }
        private readonly IHostingEnvironment _environment;

        public Startup(IConfiguration configuration, IHostingEnvironment environment)
        {
            Configuration = configuration;
            _environment = environment;

        }


        [Obsolete]
        public void ConfigureServices(IServiceCollection services)
        {


            services.AddMvc(options =>
            {
                options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
            });
            
            services.AddAntiforgery(opts => opts.Cookie.Name = "MyAntiforgeryCookie");
            #region elmah
            services.AddElmah(options =>
            {
                // دسترسی پیدا کنید elamh مسیری که توسط آن میتوانید به  
                // می باشد ~/elmah این مسیر به صورت پیشفرض     
                options.Path = "/myElmah";

                // به گونه ای که ما آن را پیاده سازی می کنیم elmah محدود کردن دسترسی به 
                options.CheckPermissionAction = CheckPermissionAction;
            });
            #endregion


            //services.Configure<FormOptions>(x =>
            //{
            //    x.ValueLengthLimit = int.MaxValue;
            //    x.MultipartBodyLengthLimit = int.MaxValue; // In case of multipart
            //});
            //services.Configure<FormOptions>(options => { options.MultipartBodyLengthLimit = 6000000; });
            #region Authentication
            //for login mandan dar host
            var KeyFolder = Path.Combine(_environment.ContentRootPath, "Keys");
            services.AddDataProtection()
                .PersistKeysToFileSystem(new DirectoryInfo(KeyFolder))
                .SetApplicationName("diving")
                .SetDefaultKeyLifetime(TimeSpan.FromDays(30));

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;

            }).AddCookie(options =>
            {

                options.LoginPath = "/Login";
                options.LogoutPath = "/Logout";
                options.ExpireTimeSpan = TimeSpan.FromMinutes(43200);
                options.Cookie.Name = "True";

            });

            #endregion

            #region DataBase Context


            services.AddDbContext<LearnContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("LearnConnection"));
            }
            );
            #endregion
            #region Ioc 
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IViewRenderService, RenderViewToString>();
            services.AddTransient<IPermissionService, PermissionService>();
            services.AddTransient<ICourseService, CourseService>();
            services.AddTransient<IOrderService, OrderService>();
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            else
            {
                //app.UseStatusCodePagesWithRedirects("/Error");
                app.UseStatusCodePagesWithReExecute("/Error/{0}");
            }
            app.UseElmah();
            app.UseStaticFiles();
            app.UseAuthentication();
            #region safeheader
    //        app.Use(async (context, next) =>
    //        {
    //            context.Response.Headers.Add("X-Xss-Protection", "1");
    //            context.Response.Headers.Add("X-Frame-Options", "DENY");
    //            context.Response.Headers.Add("Referrer-Policy", "no-referrer");
    //            context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
    //            context.Response.Headers.Add("Feature-Policy", "self");
    //            context.Response.Headers.Add("server", "none");
    //            context.Response.Headers.Add(
    //"Content-Security-Policy",
    //// "default-src 'self'; " +
    //"img-src 'self' myblobacc.blob.core.windows.net; " +
    ////"font-src 'self'; " +
    ////"style-src 'self'; " +
    ////"script-src 'self' 'nonce-KIBdfgEKjb34ueiw567bfkshbvfi4KhtIUE3IWF' " +
    ////" 'nonce-rewgljnOIBU3iu2btli4tbllwwe'; " +
    //"frame-src 'self';" +
    //"connect-src 'self';");
    //            await next();
    //        });
            #endregion

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                  name: "areas",
                  template: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                );
                routes.MapRoute("Default", "{controller=Home}/{action=Index}/{id?}");
            });

            //app.Run(async (context) =>
            //{

            //    await context.Response.WriteAsync("Hello World!");


            //});
        }
        private bool CheckPermissionAction(HttpContext httpContext)
        {
            // می باشد؟ elamh کاربری جاری سیستم دارای نقش ادمین برای دسترسی به  
            // return ( httpContext.User.Identity.IsAuthenticated && httpContext.User.IsInRole("Admin")) ;

            // در این قسمت ما تنها برای نمایش آزمایشی میگوییم که دسترسی دارند
            return true;
        }
    }

}
