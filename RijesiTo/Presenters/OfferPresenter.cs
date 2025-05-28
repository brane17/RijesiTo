using Microsoft.EntityFrameworkCore;
using RijesiTo.Data;
using RijesiTo.Interfaces;
using RijesiTo.Models;
using RijesiTo.ViewModels;
using TaskEntity = RijesiTo.Models.Task;
namespace RijesiTo.Presenters
{
    public class OfferPresenter
    {
        private readonly AppDbContext _context;
        private readonly IOfferView _view;

        public OfferPresenter(AppDbContext context, IOfferView view)
        {
            _context = context;
            _view = view;
        }

        public async System.Threading.Tasks.Task LoadUserOffersAsync(int userId)
        {
            var offers = await _context.Offers
                .Include(o => o.Task)
                .Include(u => u.User)
                .Where(o => o.UserId == userId)
                .Select(o => new OfferViewModel
                {
                    Id = o.Id,
                    TaskId = o.TaskId,
                    TaskTitle = o.Task.Title,
                    UserId = o.UserId,
                    OfferDate = o.OfferDate,
                    OfferStatus = o.OfferStatus,
                    TaskStatus = o.Task.Status,
                    UserName = $"{o.Task.User.FirstName} {o.Task.User.LastName}"
                })
                .ToListAsync();

            _view.ShowOffers(offers);
        }


        public async System.Threading.Tasks.Task LoadOffersAsync()
        {
            var offers = await _context.Offers
                .Include(o => o.Task)
                .Include(o => o.User)
                .Select(o => new OfferViewModel
                {
                    Id = o.Id,
                    TaskId = o.TaskId,
                    TaskTitle = o.Task.Title,
                    UserId = o.UserId,
                    UserName = o.User.FirstName + " " + o.User.LastName,
                    OfferDate = o.OfferDate,
                    OfferStatus = o.OfferStatus,
                    TaskStatus = o.Task.Status
                })
                .ToListAsync();

            _view.ShowOffers(offers);
        }

        public async System.Threading.Tasks.Task LoadOfferAsync(int id)
        {
            var offer = await _context.Offers
                .Include(o => o.Task)
                .Include(o => o.User)
                .Where(o => o.Id == id)
                .Select(o => new OfferViewModel
                {
                    Id = o.Id,
                    TaskId = o.TaskId,
                    TaskTitle = o.Task.Title,
                    UserId = o.UserId,
                    UserName = o.User.FirstName + " " + o.User.LastName,
                    OfferDate = o.OfferDate,
                    OfferStatus = o.OfferStatus,
                    TaskStatus = o.Task.Status
                })
                .FirstOrDefaultAsync();

            if (offer != null)
                _view.ShowOffer(offer);
        }

        public async System.Threading.Tasks.Task CreateAsync(Offer offer)
        {
            offer.OfferDate = DateTime.UtcNow;
            offer.OfferStatus = OfferStatus.Pending;
            _context.Offers.Add(offer);
            // Ažuriraj status zadatka
            var task = await _context.Tasks.FindAsync(offer.TaskId);
            if (task != null) task.Status = Models.TaskStatus.InProgress;

            await _context.SaveChangesAsync();
        }

        public async Task<List<OfferViewModel>> GetOffersByTaskAsync(int taskId)
        {
            // Unaprijed izračunaj je li neka ponuda prihvaćena za ovaj zadatak kako bi se izbjegao N+1 upit
            bool isAnyOfferAccepted = await _context.Offers
                .AnyAsync(x => x.TaskId == taskId && x.OfferStatus == OfferStatus.Accepted);

            return await _context.Offers
                .Include(o => o.User)
                .Where(o => o.TaskId == taskId)
                .Select(o => new OfferViewModel
                {
                    Id = o.Id,
                    TaskId = o.TaskId,
                    UserName = o.User.FirstName + " " + o.User.LastName,
                    OfferDate = o.OfferDate,
                    OfferStatus = o.OfferStatus,
                    IsAnyOfferAccepted = isAnyOfferAccepted
                }).ToListAsync();
        }
        public async System.Threading.Tasks.Task DeleteAsync(int id)
        {
            var offer = await _context.Offers.FindAsync(id);
            if (offer != null)
            {
                _context.Offers.Remove(offer);
                await _context.SaveChangesAsync();
            }
        }

        public async System.Threading.Tasks.Task UpdateAsyncOfferStatus(Offer offer)
        {
            var existingOffer = await _context.Offers.FindAsync(offer.Id);
            if (existingOffer != null)
            {
                existingOffer.OfferStatus = offer.OfferStatus;
                await _context.SaveChangesAsync();
            }
        }
    }
}
