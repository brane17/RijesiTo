using RijesiTo.Data;
using RijesiTo.Interfaces;
using RijesiTo.Models;
using RijesiTo.ViewModels;

namespace RijesiTo.Presenters
{
    public class RegisterPresenter
    {
        private readonly AppDbContext _context;
        private readonly IRegisterView _view;

        public RegisterPresenter(AppDbContext context, IRegisterView view)
        {
            _context = context;
            _view = view;
        }

        public async System.Threading.Tasks.Task RegisterAsync(RegisterViewModel model)
        {
            // Email uniqueness check
            if (_context.Users.Any(u => u.Email == model.Email))
            {
                _view.ShowRegistrationError("Email already exists.");
                return;
            }

            var user = new User
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                Password = model.Password,
                Role = model.Role
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            _view.RedirectToLogin();
        }
    }
}
