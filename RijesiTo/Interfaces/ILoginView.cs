namespace RijesiTo.Interfaces
{
    public interface ILoginView
    {
        void ShowLoginError(string message);
        void RedirectToHome();
        void RedirectToDashboard(); 
    }
}
