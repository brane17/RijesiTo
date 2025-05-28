using Microsoft.AspNetCore.Mvc;
using RijesiTo.Data;
using RijesiTo.Interfaces;
using RijesiTo.Presenters;
using RijesiTo.ViewModels;

namespace RijesiTo.Controllers
{
    public class UserController : Controller, IUserView
    {
        private readonly UserPresenter _presenter;
        private readonly AppDbContext _context;

        private List<UserViewModel> _users;
        private UserViewModel _user;

        public UserController(AppDbContext context)
        {
            _context = context;
            _presenter = new UserPresenter(context, this);
            _users = new List<UserViewModel>();
        }

        public void ShowUsers(List<UserViewModel> users) => _users = users;
        public void ShowUser(UserViewModel user) => _user = user;

        public async Task<IActionResult> Index()
        {
            await _presenter.GetAllUsersAsync();
            return View(_users);
        }

        public async Task<IActionResult> Details(int id)
        {
            await _presenter.GetUserByIdAsync(id);
            return View(_user);
        }

        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(UserViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            await _presenter.CreateUserAsync(vm);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            await _presenter.GetUserByIdAsync(id);
            return View(_user);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UserViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            await _presenter.UpdateUserAsync(vm);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            await _presenter.GetUserByIdAsync(id);
            return View(_user);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _presenter.DeleteUserAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }

}
