using webkino.ViewModels;

namespace webkino.Services.Interfaces
{
    public interface IUserService
    {
        bool Register(UserRegisterViewModel model, out string errorMessage);

        bool Login(UserLoginViewModel model, out string errorMessage);

        void Logout();

        UserProfileViewModel? GetCurrentUser();
    }
}