using RijesiTo.ViewModels;

namespace RijesiTo.Interfaces
{
    public interface IUserView
    {
        void ShowUsers(List<UserViewModel> users);
        void ShowUser(UserViewModel user);
    }
}
