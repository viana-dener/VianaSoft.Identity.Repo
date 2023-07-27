using VianaSoft.BuildingBlocks.Core.User.Dto.Request;
using VianaSoft.BuildingBlocks.Core.User.Dto.Response;

namespace VianaSoft.Identity.Domain.Interfaces
{
    public interface IIdentityRepository
    {
        Task<ProfileResponseDto> GetProfileAsync();
        Task<UserLoginResponseDto> LoginAsync(UserLoginRequestDto request);
        Task<UserLoginResponseDto> RegisterUserAsync(UserRegistrationRequestDto request);
        Task<bool> ForgotPasswordAsync(ForgotPasswordRequestDto request);
        Task<bool> ResetPasswordAsync(ResetPasswordRequestDto request);
        Task<bool> ChangePasswordAsync(ChangePasswordRequestDto request);
        Task<bool> IsResetPasswordTokenValidAsync(string email, string token);
        Task LogoutAsync();
    }
}
