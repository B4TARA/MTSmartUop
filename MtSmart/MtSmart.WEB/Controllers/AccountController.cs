using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using MtSmart.BLL.DTO.AccountDTOs;
using MtSmart.BLL.Interfaces;
using System.Security.Claims;
using StatusCodes = MtSmart.BLL.Enums.StatusCodes;

namespace MtSmart.WEB.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            var changePasswordLogin = TempData["changePasswordLogin"]?.ToString();
            var changePasswordMessage = TempData["changePasswordMessage"]?.ToString();

            if (!string.IsNullOrEmpty(changePasswordLogin))
            {
                ViewBag.changePasswordLogin = changePasswordLogin;
            }

            if (!string.IsNullOrEmpty(changePasswordMessage))
            {
                ModelState.AddModelError("", changePasswordMessage);
            }

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Login", "Account");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(UserLoginDTO model)
        {
            if (ModelState.IsValid)
            {
                var response = await _accountService.Login(model);

                if (response.StatusCode == StatusCodes.OK && response.Data != null)
                {
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(response.Data),
                        new AuthenticationProperties { IsPersistent = true });

                    return RedirectToRoute(new
                    {
                        httpMethod = "POST",
                        controller = "Home",
                        action = "Index",
                    });
                }
                else
                {
                    ModelState.AddModelError("", response.Description);
                }
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemindPassword(UserChangePasswordDTO model)
        {
            TempData["changePasswordLogin"] = model.Email;

            if (ModelState.IsValid)
            {
                var isReminded = await _accountService.RemindPassword(model.Email);

                if (!isReminded)
                {
                    TempData["changePasswordMessage"] = "Пользователя с таким email-ом не существует";
                    return RedirectToAction("Login", "Account");
                }

                TempData["changePasswordMessage"] = "Ваши учетные данные успешно высланы на почту";
                return RedirectToAction("Login", "Account");
            }

            TempData["changePasswordMessage"] = "Invalid ModelState";
            return RedirectToAction("Login", "Account");
        }
    }
}
