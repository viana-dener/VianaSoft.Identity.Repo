using VianaSoft.Identity.App.Models.Request;
using VianaSoft.Identity.App.Models.Response;

namespace VianaSoft.Identity.App.Interfaces
{
    public interface IIdentityApplication
    {
        Task<ProfileResponse> GetProfileAsync();
        Task<UserLoginResponse> LoginAsync(UserLoginRequest request);
        Task<UserLoginResponse> RegisterUserAsync(UserRegistrationRequest request);
        Task<bool> ForgotPasswordAsync(ForgotPasswordRequest request);
        Task<bool> ResetPasswordAsync(ResetPasswordRequest request);
        Task<bool> ChangePasswordAsync(ChangePasswordRequest request);
        Task<bool> IsResetPasswordTokenValidAsync(string email, string token);
        Task LogoutAsync();
    }
}
