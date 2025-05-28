using RijesiTo.ViewModels;

namespace RijesiTo.Interfaces
{
    public interface IOfferView
    {
        void ShowOffers(List<OfferViewModel> offers);
        void ShowOffer(OfferViewModel offer);
    }
}
