using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Mvc;

namespace Client.Controllers
{
    public class AccountController : Controller
    {
        private readonly IConfiguration _config;

        public AccountController(IConfiguration config)
        {
            _config = config;
        }
        public IActionResult Login(string redirectUri)
        {
            if (string.IsNullOrWhiteSpace(redirectUri))
            {
                redirectUri= Url.Content("~/");
            }

            if (HttpContext.User.Identity.IsAuthenticated)
            {
                Response.Redirect(redirectUri);
            }

            return Challenge(new AuthenticationProperties
            {
                RedirectUri= redirectUri,
            },
            OpenIdConnectDefaults.AuthenticationScheme);
        }


        public IActionResult LogOut(string redirectUri)
        {
            return SignOut(new AuthenticationProperties { 
                RedirectUri = _config["applicationUrl"] 
            },
            OpenIdConnectDefaults.AuthenticationScheme,
            CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}
