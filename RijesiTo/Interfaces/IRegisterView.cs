namespace RijesiTo.Interfaces
{
    public interface IRegisterView
    {
        void ShowRegistrationError(string message);
        void RedirectToLogin();
    }
}
