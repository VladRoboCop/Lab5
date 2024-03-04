using Microsoft.AspNetCore.Mvc;
using System;

namespace MyAspNetApp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SubmitFormData(string dataValue, DateTime expirationDate)
        {

            Response.Cookies.Append("FormData", dataValue, new CookieOptions
            {
                Expires = expirationDate,
                HttpOnly = true,
                Secure = true
            });


            Response.Cookies.Append("FormData_ExpiresAt", expirationDate.ToString(), new CookieOptions
            {
                Expires = expirationDate,
                HttpOnly = true,
                Secure = true
            });

            return RedirectToAction(nameof(CheckCookies));
        }


        public IActionResult CheckCookies()
        {

            ViewBag.HasFormData = Request.Cookies.ContainsKey("FormData");

            if (ViewBag.HasFormData)
            {
                ViewBag.FormData = Request.Cookies["FormData"];
                ViewBag.ExpiresAt = Request.Cookies["FormData_ExpiresAt"];
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ClearCookies()
        {

            Response.Cookies.Delete("FormData");
            Response.Cookies.Delete("FormData_ExpiresAt");

            return RedirectToAction(nameof(CheckCookies));
        }
    }
}
