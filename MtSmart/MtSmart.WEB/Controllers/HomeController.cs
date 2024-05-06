using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MtSmart.WEB.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class HomeController : Controller
    {

        [AllowAnonymous]
        public IActionResult Index()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            var currentUserId = Convert.ToInt32(User.Claims.FirstOrDefault(c => c.Type == "Id").Value);

            return RedirectToAction("ListEmployeeCards", "UserBoard", new { employeeId = currentUserId });
        }

        public IActionResult Error()
        {
            // Здесь можно добавить логику обработки ошибки, например, запись в лог или отображение пользователю сообщения об ошибке
            return View();
        }

        public IActionResult AccessDenied()
        {
            // Здесь можно добавить логику обработки ошибки, например, запись в лог или отображение пользователю сообщения об ошибке
            return View();
        }
    }
}