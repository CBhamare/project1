using MoviesList.Models.DTO;

namespace MoviesList.Repositories.Abstract
{
    public interface IUserAuthenticationService
    {
        Task<Status> LoginAsync(LoginModel model);
        Task LogOutAsysnc();
        Task<Status> RegisterAsync(RegistrationModel model);
       // Task<Status> ChangePasswordAsync(ChangePasswordModel model);
    }
}
