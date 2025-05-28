using RijesiTo.ViewModels;

namespace RijesiTo.Interfaces
{
    public interface IReviewView
    {
        void ShowReview(ReviewViewModel review);
        void ShowReviews(List<ReviewViewModel> reviews);
    }
}
