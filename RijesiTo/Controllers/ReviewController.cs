using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RijesiTo.Data;
using RijesiTo.Interfaces;
using RijesiTo.Presenters;
using RijesiTo.ViewModels;

namespace RijesiTo.Controllers
{
    public class ReviewController : Controller, IReviewView
    {
        private readonly AppDbContext _context;
        private readonly ReviewPresenter _presenter;

        private List<ReviewViewModel> _reviews = new();
        private ReviewViewModel _review;


        public ReviewController(AppDbContext context)
        {
            _presenter = new ReviewPresenter(context, this);
            _context = context;
        }

        public void ShowReviews(List<ReviewViewModel> reviews) => _reviews = reviews;
        public void ShowReview(ReviewViewModel review) => _review = review;

        public async Task<IActionResult> Index()
        {
            await _presenter.LoadReviewsAsync();
            return View(_reviews);
        }

        public async Task<IActionResult> Details(int id)
        {
            await _presenter.LoadReviewAsync(id);
            return View(_review);
        }

        [HttpGet]
        public IActionResult Create(int taskId)
        {
            return View(new ReviewViewModel { TaskId = taskId });
        }

        [HttpPost]
        public async Task<IActionResult> Create(ReviewViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var reviewerId = HttpContext.Session.GetInt32("UserId") ?? 0;
            var task = await _context.Tasks.FindAsync(model.TaskId);
            var workerId = task?.UserId ?? 0;

            await _presenter.CreateAsync(model, reviewerId);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            await _presenter.LoadReviewAsync(id);
            return View(_review);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _presenter.DeleteAsync(id);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Leave(int taskId)
        {
            return View(new ReviewViewModel { TaskId = taskId });
        }

        [HttpPost]
        public async Task<IActionResult> SubmitAjax([FromBody] ReviewViewModel model)
        {
            if (model.Rating < 1 || model.Rating > 5)
                return BadRequest("Invalid rating.");

            var reviewerId = HttpContext.Session.GetInt32("UserId") ?? 0;
            var task = await _context.Tasks.FindAsync(model.TaskId);
            if (task == null || task.Status != Models.TaskStatus.Completed)
                return BadRequest("Task not found or not completed.");

            var existing = await _context.Reviews
                .AnyAsync(r => r.TaskId == model.TaskId && r.UserId == reviewerId);

            if (existing)
                return BadRequest("You already submitted a review for this task.");

           
            await _presenter.SubmitReviewAsync(model, reviewerId);

            return Ok(new { success = true });
        }

        [HttpGet]
        public async Task<IActionResult> GetReviewsJson(int taskId)
        {
            var reviews = await _presenter.GetReviewsForTaskAsync(taskId);
            return Json(reviews);
        }
    }
}
