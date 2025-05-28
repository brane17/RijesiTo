using RijesiTo.Data;
using RijesiTo.Interfaces;
using RijesiTo.Models;

namespace RijesiTo.Presenters
{
    public class AuthPresenter
    {
        private readonly AppDbContext _context;
        private readonly ILoginView _view;

        public AuthPresenter(AppDbContext context, ILoginView view)
        {
            _context = context;
            _view = view;
        }

        public void Login(string email, string password, HttpContext httpContext)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == email && u.Password == password);

            if (user == null)
            {
                _view.ShowLoginError("Invalid credentials");
                return;
            }

            httpContext.Session.SetInt32("UserId", user.Id);
            httpContext.Session.SetString("UserRole", user.Role.ToString());
            httpContext.Session.SetString("UserName", $"{user.FirstName} {user.LastName}");
            if(user.Role.Equals(UserRole.Worker))
            {
                _view.RedirectToDashboard();
            }
            else
            {
                _view.RedirectToHome();
            }
        }
    }
}
