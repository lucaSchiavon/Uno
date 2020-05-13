﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Uno.Entities;
//-----------------LS
//using System.Security.Claims;
//using Microsoft.AspNetCore.Authentication.Cookies;

namespace Uno.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<LoginModel> _logger;
        private readonly UserManager<ApplicationUser> _userManager;

        public LoginModel(SignInManager<ApplicationUser> signInManager, ILogger<LoginModel> logger,
            UserManager<ApplicationUser> userManager)
        {
            _signInManager = signInManager;
            _logger = logger;
            _userManager = userManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl = returnUrl ?? Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");

            if (ModelState.IsValid)
            {
                ////------------LS
                //var claims = new List<Claim>
                //{
                //    new Claim(ClaimTypes.Name, ""),
                //    new Claim("FullName", ""),
                //    new Claim(ClaimTypes.Role, "Administrator"),
                //};

                //var claimsIdentity = new ClaimsIdentity(
                //    claims, CookieAuthenticationDefaults.AuthenticationScheme);
                //var authProperties = new AuthenticationProperties
                //{
                //    //AllowRefresh = <bool>,
                //    // Refreshing the authentication session should be allowed.

                //    //ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
                //    // The time at which the authentication ticket expires. A 
                //    // value set here overrides the ExpireTimeSpan option of 
                //    // CookieAuthenticationOptions set with AddCookie.

                //    //IsPersistent = true,
                //    // Whether the authentication session is persisted across 
                //    // multiple requests. When used with cookies, controls
                //    // whether the cookie's lifetime is absolute (matching the
                //    // lifetime of the authentication ticket) or session-based.

                //    //IssuedUtc = <DateTimeOffset>,
                //    // The time at which the authentication ticket was issued.

                //    //RedirectUri = <string>
                //    // The full path or absolute URI to be used as an http 
                //    // redirect response value.
                //};

                // await HttpContext.SignInAsync(
                //    CookieAuthenticationDefaults.AuthenticationScheme,
                //    new ClaimsPrincipal(claimsIdentity),
                //    authProperties);
                ////-------------LS
                ///

                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true

                //TODO:qui l'utente non dovrebbe loggarsi se prima non ha confermato la mail
                //---------------------------------LS
              
                var userid = await _userManager.FindByEmailAsync(Input.Email);
                bool IsEmailConf = await _userManager.IsEmailConfirmedAsync(userid);

                Microsoft.AspNetCore.Identity.SignInResult result;

                if (!IsEmailConf)
                {
                    //return View("EmailToConfirm");
                    //return RedirectToPage("./NotEmailConfirmed");
                    return RedirectToAction("NotEmailConfirmed", "FrontIdentity");
                }
                else
                {
                    result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: true);
                }
                //--------------------------------

                //var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: true);
                if (result.Succeeded)
                {
                    bool r = User.IsInRole("Administrator");
                    _logger.LogInformation("User logged in.");
                    return LocalRedirect(returnUrl);
                }
                if (result.RequiresTwoFactor)
                {
                    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning("User account locked out.");
                    return RedirectToPage("./Lockout");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return Page();
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
