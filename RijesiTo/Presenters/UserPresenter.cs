using Microsoft.EntityFrameworkCore;
using RijesiTo.Data;
using RijesiTo.Interfaces;
using RijesiTo.Models;
using RijesiTo.ViewModels;
using Task = System.Threading.Tasks.Task;

namespace RijesiTo.Presenters
{
    public class UserPresenter
    {
        private readonly AppDbContext _context;
        private readonly IUserView _view;

        public UserPresenter(AppDbContext context, IUserView view)
        {
            _context = context;
            _view = view;
        }

        public async Task GetAllUsersAsync()
        {
            var users = await _context.Users
                .Where(x=>x.Role != UserRole.Admin)
                .Select(u => new UserViewModel
                {
                    Id = u.Id,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Email = u.Email,
                    Role = u.Role
                }).ToListAsync();

            _view.ShowUsers(users);
        }

        public async Task GetUserByIdAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return;

            _view.ShowUser(new UserViewModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Role = user.Role
            });
        }

        public async Task CreateUserAsync(UserViewModel vm)
        {
            var user = new User
            {
                FirstName = vm.FirstName,
                LastName = vm.LastName,
                Email = vm.Email,
                Role = vm.Role,
                Password = vm.Password 
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateUserAsync(UserViewModel vm)
        {
            var user = await _context.Users.FindAsync(vm.Id);
            if (user == null) return;

            user.FirstName = vm.FirstName;
            user.LastName = vm.LastName;
            user.Email = vm.Email;
            user.Role = vm.Role;
            user.Password = vm.Password; 

            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUserAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }
    }
}
