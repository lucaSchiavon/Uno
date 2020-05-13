using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Uno.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Uno.Entities;
using Uno.Services;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using AutoMapper;
using System.Globalization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Localization.Routing;
using Uno.Middleware;
using Uno.CustomProviders;
//using Microsoft.AspNetCore.Internal;
//using Microsoft.AspNetCore.Internal;


namespace Uno
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

           
            #region cookies configuration
            //configurazione consenso GDPR cookies
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            //--------------------------------------------
            //aggiunti LS
            //configurazione cookies (queste opzioni avranno effetto solo spuntando la checkbox  Remember me)
            //in caso contrario verrà emesso un cookies di sessione che terminerà alla chiusura del browser
            services.ConfigureApplicationCookie(options =>
            {
                //options.Cookie.HttpOnly = true;
                //il cookies rimarrà in vita 1 giorno
                //alla morte del cookies l'utente dovrà riloggarsi (e non gliene verrà assegnato automaticamente un'altro).
                options.ExpireTimeSpan = TimeSpan.FromDays(1);

                //options.LoginPath = "/Identity/Account/Login";
                //options.AccessDeniedPath = "/Identity/Account/AccessDenied";
                options.SlidingExpiration = false;

            });
            #endregion


            #region  setting configuration
            //leggo il settings per invio mail
            services.Configure<EmailSettings>(Configuration.GetSection("EmailSettings"));
            services.Configure<ExternalLoginSettings>(Configuration.GetSection("ExternalLoginSettings"));
            #endregion

            #region asp net core identity configuration Password, lockout and user setting)
            services.Configure<IdentityOptions>(options =>
            {
                // Password settings.
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;

                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // User settings.
                options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = false;
            });
            #endregion

            #region dbcontex configuration (connectionstring, provider, mapping asp net core identity)

            //--------------------------------------------
            //configurazione del contesto di entity framework usato
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            //configura i servizi usati da asp core identity
            //Identityuser rappresenta l'entità utente con tutte le sue proprietà
            //ApplicationDbContext è il contest che gestisce la persistenza dei dati inerenti la sicurezza
            //su database e qui viene registrato
            services.AddDefaultIdentity<ApplicationUser>()
                 //.AddDefaultUI(UIFramework.Bootstrap4)
                 .AddRoles<IdentityRole>() //indispensabile per abilitare la gestione dei ruoli
                .AddEntityFrameworkStores<ApplicationDbContext>();
            #endregion
            //         services.Configure<IdentityOptions>(options =>
            //{
            //    // Password settings.
            //    options.Password.RequireDigit = true;
            //    options.Password.RequireLowercase = true;
            //    options.Password.RequireNonAlphanumeric = true;
            //    options.Password.RequireUppercase = true;
            //    options.Password.RequiredLength = 6;
            //    options.Password.RequiredUniqueChars = 1;

            //    // Lockout settings.
            //    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            //    options.Lockout.MaxFailedAccessAttempts = 5;
            //    options.Lockout.AllowedForNewUsers = true;

            //    // User settings.
            //    options.User.AllowedUserNameCharacters =
            //    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
            //    options.User.RequireUniqueEmail = false;
            //});

            //IOptions<ExternalLoginSettings> ioext = (IOptions<ExternalLoginSettings>)Configuration["ExternalLoginSettings"].va;


            services.AddAuthentication()
        ////.AddMicrosoftAccount(microsoftOptions => { ... })
        //    .AddGoogle(googleOptions => {
        //        googleOptions.ClientId = "251692794785-d7ihg5a1lik4p4rvjskqlqfe9nsrshrv.apps.googleusercontent.com";
        //        googleOptions.ClientSecret = "aFZebR0iYzFSdwHNyA1gkoNE";
        //    })
        ////.AddTwitter(twitterOptions => { ... })
        //    .AddFacebook(facebookOptions => {
        //        facebookOptions.AppId = "153707982488465";
        //        facebookOptions.AppSecret = "52cf36edc9c364cae4c4b0d196404de4";
        //    });

        //.AddMicrosoftAccount(microsoftOptions => { ... })
        .AddGoogle(googleOptions => {
            googleOptions.ClientId = Configuration["ExternalLoginSettings:Google_ClientId"];
            googleOptions.ClientSecret = Configuration["ExternalLoginSettings:Google_ClientSecret"];
        })
        //.AddTwitter(twitterOptions => { ... })
        .AddFacebook(facebookOptions => {
            facebookOptions.AppId = Configuration["ExternalLoginSettings:FB_AppId"];
            facebookOptions.AppSecret = Configuration["ExternalLoginSettings:FB_AppSecret"];
        });

            services.Configure<RequestLocalizationOptions>(opts => {
                //viene aggiunto il provider per gestire la cultura con il routing
                //opts.RequestCultureProviders.Clear();
                //opts.RequestCultureProviders.Insert(0, new RouteDataRequestCultureProvider());
                //opts.RequestCultureProviders.Insert(1, new CookieRequestCultureProvider());
                ////-------------------------
                ////uso il dbcontext per recuperare le culture dal db per generare una lista di culture
                //var DbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                // .UseSqlServer(Configuration["ConnectionStrings:DefaultConnection"])
                // .Options;


                ////in caso di gestione culture dinamica
                //using (var ctx = new ApplicationDbContext(DbContextOptions))
                //{
                //    IdentityUserRole<string> iur = ctx.UserRoles.First();
                //    // Your code here
                //};
                ////-------------------------
                var supportedCultures = new List<CultureInfo> {


                      new CultureInfo("en-US"),
                      new CultureInfo("ru"),
                        new CultureInfo("fr-FR")
                };
                opts.DefaultRequestCulture = new RequestCulture("en-US");
                opts.SupportedCultures = supportedCultures;
                opts.SupportedUICultures = supportedCultures;
                opts.RequestCultureProviders.Insert(0, new CustomRouteDataCultureProvider());
               // opts.RequestCultureProviders.Insert(0, new RouteDataRequestCultureProvider());
                //opts.AddInitialRequestCultureProvider(new CustomRequestCultureProvider(async context =>
                //{
                //    // My custom request culture logic
                //    return new ProviderCultureResult("en");
                //}));

            });

          

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddSingleton<IEmailSender, EmailSender>();

            services.AddMvc()
                .AddViewLocalization(opts => { opts.ResourcesPath = "Resources"; })
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
                .AddDataAnnotationsLocalization()
                .AddRazorPagesOptions(options =>
            {
                options.Conventions.AddAreaPageRoute("Identity", "/Account/Login", "Login");
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            //https://www.youtube.com/watch?v=33okCuCK3Ik
            //https://docs.microsoft.com/it-it/aspnet/core/fundamentals/localization?view=aspnetcore-3.1

           

   
           
            services.AddAutoMapper(typeof(Startup));

            #region "localizzazione"

            services.AddLocalization(options => options.ResourcesPath = "Resources");
            services.AddCloudscribePagination();
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
          

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            //var supportedCultures = new[]
            //{
            //    new CultureInfo("en-US"),
            //    new CultureInfo("fr-FR"),
            //};

            //app.UseRequestLocalization(new RequestLocalizationOptions
            //{
            //    DefaultRequestCulture = new RequestCulture("en-US"),
            //    // Formatting numbers, dates, etc.
            //    SupportedCultures = supportedCultures,
            //    // UI strings that we have localized.
            //    SupportedUICultures = supportedCultures
            //});

            var options = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(options.Value);



            app.UseStaticFiles();
            app.UseCookiePolicy();
            //viene aggiunto il middleware della autenticazione nella pipeline
            //il middleware verifioca l'identità e crea un claim principal
            app.UseAuthentication();



            app.UseMvc(routes =>
            {

                routes.MapRoute(
                  name: "Backoffice",
                  template: "{area}/{controller=Home}/{action=Index}/{id?}");
                //routes.MapRoute(
                //   name: "Backoffice",
                //   template: "{area=Backoffice}/{controller=Home}/{action=Index}/{id?}");
                //routes.MapRoute(
                //    name: "pagingdemo1",
                //    template: "pager/{p:int?}",
                //    defaults: new { controller = "Home", action = "Index" }
                //    );

                //routes.MapRoute(
                //    name: "pagingdemo1",
                //    template: "{culture}/{controller=Home}/{action=Index}/pager/{p:int?}"
                //    );

                routes.MapRoute(
                    name: "default",
                    template: "{culture}/{controller=Home}/{action=Index}/{id?}");
            });

            app.UseMvcWithDefaultRoute();
        }
    }
}
