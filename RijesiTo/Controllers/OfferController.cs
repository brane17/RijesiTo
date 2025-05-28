using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RijesiTo.Data;
using RijesiTo.Interfaces;
using RijesiTo.Models;
using RijesiTo.Presenters;
using RijesiTo.ViewModels;

namespace RijesiTo.Controllers
{
    public class OfferController : Controller, IOfferView
    {
        private readonly OfferPresenter _presenter;
        private List<OfferViewModel> _offers;
        private OfferViewModel _singleOffer;

        public OfferController(AppDbContext context)
        {
            _presenter = new OfferPresenter(context, this);
            _offers = new List<OfferViewModel>();
        }

        public void ShowOffers(List<OfferViewModel> offers)
        {
            _offers = offers;
        }

        public void ShowOffer(OfferViewModel offer)
        {
            _singleOffer = offer;
        }



        public async Task<IActionResult> Index()
        {
            await _presenter.LoadOffersAsync();
            return View(_offers);
        }

        [HttpGet]
        public async Task<IActionResult> GetByTask(int id)
        {
            var offers = await _presenter.GetOffersByTaskAsync(id);
            return Json(offers);
        }

        public async Task<IActionResult> MyOffers()
        {
            var userId = HttpContext.Session.GetInt32("UserId") ?? 0;
            await _presenter.LoadUserOffersAsync(userId);
            return View("Index", _offers);
        }

        public async Task<IActionResult> Details(int id)
        {
            await _presenter.LoadOfferAsync(id);
            return View(_singleOffer);
        }

        [HttpGet]
        public IActionResult Create(int taskId)
        {
            var offer = new Offer { TaskId = taskId };
            return View(offer);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Offer offer)
        {
            await _presenter.CreateAsync(offer);
            return Ok(new { success = true });
        }

        [HttpPost]
        public async Task<IActionResult> UpdateOfferStatus([FromBody] Offer offer)
        {
            await _presenter.UpdateAsyncOfferStatus(offer);
            return Ok(new { success = true });
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            await _presenter.LoadOfferAsync(id);
            return View(_singleOffer);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _presenter.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
