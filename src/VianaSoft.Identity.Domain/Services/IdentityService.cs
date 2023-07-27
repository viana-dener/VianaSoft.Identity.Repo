using System.Net;
using VianaSoft.BuildingBlocks.Core.Notifications.Enumerators;
using VianaSoft.BuildingBlocks.Core.Notifications.Interfaces;
using VianaSoft.BuildingBlocks.Core.Resources.Interfaces;
using VianaSoft.BuildingBlocks.Core.User.Dto.Request;
using VianaSoft.BuildingBlocks.Core.User.Dto.Response;
using VianaSoft.Identity.Domain.Interfaces;

namespace VianaSoft.Identity.Domain.Services
{
    public class IdentityService : IIdentityService
    {
        #region Properties

        private readonly INotifier _notifier;
        private readonly ILanguageMessage _responseMessage;
        private readonly IIdentityRepository _repository;

        #endregion

        #region Builders

        public IdentityService(INotifier notifier, ILanguageMessage responseMessage, IIdentityRepository repository)
        {
            _notifier = notifier;
            _responseMessage = responseMessage;
            _repository = repository;

            _notifier.Add(_responseMessage.RequestSuccessfullyReceivedDomain(), false, HttpStatusCode.OK, TypeError.Success);
        }

        #endregion

        #region Public Methods

        public async Task<ProfileResponseDto> GetProfileAsync()
        {
            return await _repository.GetProfileAsync();
        }
        public async Task<UserLoginResponseDto> LoginAsync(UserLoginRequestDto request)
        {
            return await _repository.LoginAsync(request);
        }
        public async Task<UserLoginResponseDto> RegisterUserAsync(UserRegistrationRequestDto request)
        {
            return await _repository.RegisterUserAsync(request);
        }
        public async Task<bool> ForgotPasswordAsync(ForgotPasswordRequestDto request)
        {
            return await _repository.ForgotPasswordAsync(request);
        }
        public async Task<bool> ResetPasswordAsync(ResetPasswordRequestDto request)
        {
            return await _repository.ResetPasswordAsync(request);
        }
        public async Task<bool> ChangePasswordAsync(ChangePasswordRequestDto request)
        {
            return await _repository.ChangePasswordAsync(request);
        }
        public async Task<bool> IsResetPasswordTokenValidAsync(string email, string token)
        {
            return await _repository.IsResetPasswordTokenValidAsync(email, token);
        }
        public async Task LogoutAsync()
        {
            await _repository.LogoutAsync();
        }
        #endregion
    }
}
