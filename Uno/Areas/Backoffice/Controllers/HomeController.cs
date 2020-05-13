using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Uno.Models;

namespace Uno.Backoffice.Controllers
{
    [Area("Backoffice")]
    public class HomeController : Controller
    {
        //private readonly IStringLocalizer<HomeController> _localizer;
       
        public HomeController()
        {
            //_localizer = localizer;
           
        }

        //[HttpGet]
        //public string Get()
        //{
        //    return _localizer["About Title"];
        //}

        [Authorize]
        public IActionResult Index()

        {
            // @ViewData["BackofficeHomePage"] = _localizer["Backoffice homepage"];
            return View();
        }



    }
}
