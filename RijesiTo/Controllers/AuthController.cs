using Microsoft.AspNetCore.Mvc;
using RijesiTo.Data;
using RijesiTo.Interfaces;
using RijesiTo.Presenters;
using RijesiTo.ViewModels;

namespace RijesiTo.Controllers
{
    public class AuthController : Controller, ILoginView, IRegisterView
    {
        private readonly AuthPresenter _presenter;
        private readonly RegisterPresenter _registerPresenter;

        public AuthController(AppDbContext context)
        {
            _presenter = new AuthPresenter(context, this);
            _registerPresenter = new RegisterPresenter(context, this);
        }

        public IActionResult Login() => View();

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                _presenter.Login(model.Email, model.Password, HttpContext);
            }
            return View(model);
        }

        public void ShowLoginError(string message)
        {
            ModelState.AddModelError("", message);
        }


        public void RedirectToDashboard()
        {
            Response.Redirect(Url.Action("Dashboard", "Worker"));
        }

        public void RedirectToHome()
        {
            Response.Redirect(Url.Action("Index", "Home"));
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }


        // GET
        public IActionResult Register() => View();

        // POST
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                await _registerPresenter.RegisterAsync(model);
            }
            return View(model);
        }

        public void ShowRegistrationError(string message)
        {
            ModelState.AddModelError("", message);
        }

        public void RedirectToLogin()
        {
            Response.Redirect(Url.Action("Login", "Auth"));
        }
    }

}
