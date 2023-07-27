using AutoMapper;
using System.Net;
using VianaSoft.BuildingBlocks.Core.Notifications.Enumerators;
using VianaSoft.BuildingBlocks.Core.Notifications.Interfaces;
using VianaSoft.BuildingBlocks.Core.Resources.Interfaces;
using VianaSoft.BuildingBlocks.Core.User.Dto.Request;
using VianaSoft.Identity.App.Interfaces;
using VianaSoft.Identity.App.Models.Request;
using VianaSoft.Identity.App.Models.Response;
using VianaSoft.Identity.Domain.Interfaces;

namespace VianaSoft.Identity.App.Services
{
    public class IdentityApplication : IIdentityApplication
    {
        #region Properties

        private readonly INotifier _notifier;
        private readonly ILanguageMessage _responseMessage;
        private readonly IMapper _mapper;
        private readonly IIdentityService _service;

        #endregion

        #region Builders

        public IdentityApplication(INotifier notifier, ILanguageMessage responseMessage, IMapper mapper, IIdentityService service)
        {
            _notifier = notifier;
            _responseMessage = responseMessage;
            _mapper = mapper;
            _service = service;

            _notifier.Add(_responseMessage.RequestSuccessfullyReceivedOrchestrator(), false, HttpStatusCode.OK, TypeError.Success);
        }

        #endregion

        #region Public Methods

        public async Task<ProfileResponse> GetProfileAsync()
        {
            return _mapper.Map<ProfileResponse>(await _service.GetProfileAsync());
        }
        public async Task<UserLoginResponse> LoginAsync(UserLoginRequest request)
        {
            return _mapper.Map<UserLoginResponse>(await _service.LoginAsync(_mapper.Map<UserLoginRequestDto>(request)));
        }
        public async Task<UserLoginResponse> RegisterUserAsync(UserRegistrationRequest request)
        {
            return _mapper.Map<UserLoginResponse>(await _service.RegisterUserAsync(_mapper.Map<UserRegistrationRequestDto>(request)));
        }
        public async Task<bool> ForgotPasswordAsync(ForgotPasswordRequest request)
        {
            return await _service.ForgotPasswordAsync(_mapper.Map<ForgotPasswordRequestDto>(request));
        }
        public async Task<bool> ResetPasswordAsync(ResetPasswordRequest request)
        {
            return await _service.ResetPasswordAsync(_mapper.Map<ResetPasswordRequestDto>(request));
        }
        public async Task<bool> ChangePasswordAsync(ChangePasswordRequest request)
        {
            return await _service.ChangePasswordAsync(_mapper.Map<ChangePasswordRequestDto>(request));
        }
        public async Task<bool> IsResetPasswordTokenValidAsync(string email, string token)
        {
            return await _service.IsResetPasswordTokenValidAsync(email, token);
        }
        public async Task LogoutAsync()
        {
            await _service.LogoutAsync();
        }
        #endregion
    }
}
