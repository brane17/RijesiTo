using Microsoft.EntityFrameworkCore;
using RijesiTo.Data;
using RijesiTo.Interfaces;
using RijesiTo.Models;
using RijesiTo.ViewModels;
using Task = System.Threading.Tasks.Task;

namespace RijesiTo.Presenters
{
    public class ReviewPresenter
    {
        private readonly AppDbContext _context;
        private readonly IReviewView _view;

        public ReviewPresenter(AppDbContext context, IReviewView view)
        {
            _context = context;
            _view = view;
        }

        public async Task LoadReviewsAsync()
        {
            var reviews = await _context.Reviews
                .Include(r => r.User)
                .Select(r => new ReviewViewModel
                {
                    Id = r.Id,
                    TaskId = r.TaskId,
                    Rating = r.Rating,
                    Comment = r.Comment,
                    ReviewDate = r.Date,
                    ReviewerName = r.User.FirstName + " " + r.User.LastName,
                    ReviewerType = r.User.Role == UserRole.Worker ? UserRole.Worker.ToString() : UserRole.Client.ToString()
                }).ToListAsync();

            _view.ShowReviews(reviews);
        }

        public async Task LoadReviewAsync(int id)
        {
            var r = await _context.Reviews
                .Include(r => r.User)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (r == null) return;

            var vm = new ReviewViewModel
            {
                Id = r.Id,
                TaskId = r.TaskId,
                Rating = r.Rating,
                Comment = r.Comment,
                ReviewDate = r.Date,
                ReviewerName = r.User.FirstName + " " + r.User.LastName,
                ReviewerType = r.User.Role == UserRole.Worker ? UserRole.Worker.ToString() : UserRole.Client.ToString()
            };

            _view.ShowReview(vm);
        }

        public async Task CreateAsync(ReviewViewModel model, int reviewerId)
        {
            var review = new Review
            {
                TaskId = model.TaskId,
                Rating = model.Rating,
                Comment = model.Comment,
                UserId = reviewerId,
                Date = DateTime.UtcNow
            };

            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var review = await _context.Reviews.FindAsync(id);
            if (review != null)
            {
                _context.Reviews.Remove(review);
                await _context.SaveChangesAsync();
            }
        }

        public async System.Threading.Tasks.Task SubmitReviewAsync(ReviewViewModel vm, int reviewerId)
        {
            var review = new Review
            {
                TaskId = vm.TaskId,
                Rating = vm.Rating,
                Comment = vm.Comment,
                UserId = reviewerId,
                Date = DateTime.UtcNow
            };

            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();
        }
        public async Task<List<ReviewViewModel>> GetReviewsForTaskAsync(int taskId)
        {
            return await _context.Reviews
                .Include(r => r.User)
                .Include(r => r.Task)
                .Where(r => r.TaskId == taskId)
                .Select(r => new ReviewViewModel
                {
                    TaskId = r.TaskId,
                    Rating = r.Rating,
                    Comment = r.Comment,
                    ReviewerName = r.User.FirstName + " " + r.User.LastName,
                    ReviewDate = r.Date,
                    ReviewerType = r.User.Role == UserRole.Worker ? UserRole.Worker.ToString() : UserRole.Client.ToString()
                }).ToListAsync();
        }
    }
}
