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
            // Сохранение данных в Cookies
            Response.Cookies.Append("FormData", dataValue, new CookieOptions
            {
                Expires = expirationDate,
                HttpOnly = true,
                Secure = true
            });

            // Добавляем информацию о сроке действия данных
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
            // Проверка наличия значения в Cookies
            ViewBag.HasFormData = Request.Cookies.ContainsKey("FormData");

            // Получение данных из Cookies и их срока действия, если они есть
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
            // Удаление Cookies
            Response.Cookies.Delete("FormData");
            Response.Cookies.Delete("FormData_ExpiresAt");

            return RedirectToAction(nameof(CheckCookies));
        }
    }
}
