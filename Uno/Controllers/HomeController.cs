using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Uno.Data;
using Uno.Models;
using Uno.Middleware;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Builder;

namespace Uno.Controllers
{
    [MiddlewareFilter(typeof(CustomRouteLocalizationMiddleware))]
    public class HomeController : Controller
    {
        //decommentare se si implemeta backoffice con maschera login in homepage
        //[Authorize]
        //private ApplicationDbContext _ApplicationDbContext;
        //public HomeController(ApplicationDbContext context)
        //{
        //    _ApplicationDbContext = context;
        //}
        private readonly ILogger<HomeController> _logger;
        private readonly IStringLocalizer<HomeController> _localizer;

      

        public HomeController(ILogger<HomeController> logger, IStringLocalizer<HomeController> localizer)
        {
            _logger = logger;
            _localizer = localizer;
            
        }

        public IActionResult Index()

        {
            //traduzione con file di risorse...:
            //ResourceManager rm = new ResourceManager(typeof(HomeController));
            //var a = rm.GetString("First slide");


            @ViewData["First slide"] = _localizer["First slide"];
            _logger.LogInformation(1000, new Exception("messaggio eccezione"), "test log{0}{1}", "par1", "par2");
            // DbInitializer.Initialize(_ApplicationDbContext);
            return View();
        }





        ////[Authorize]
        //public IActionResult NotEmailConfirmed()
        //{
        //    return View();
        //}

        [HttpPost]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );
            string ReturnUrlReplaced = "";
            //return LocalRedirect(returnUrl);
            if (returnUrl == "/")
            { ReturnUrlReplaced = "/" + culture; }
            else
            {
                string[] ArrReturnUrlSplit = returnUrl.Split("/");
                ReturnUrlReplaced = returnUrl.Replace(ArrReturnUrlSplit[1],  culture );
                //var cultureItems = LocOptions.Value.SupportedUICultures;
                //foreach (CultureInfo ci in cultureItems)
                //    {
                 

                 
                //   // returnUrl.Replace()
                //    //if (returnUrl.Contains("/" + ci.DisplayName + "/"))
                //    //{

                //    //}
                //}

          
            }
            //return LocalRedirect("~/" +culture + @"/" + returnUrl);
            return LocalRedirect("~" + ReturnUrlReplaced);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
