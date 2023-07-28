using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using VianaSoft.BuildingBlocks.Core.Controllers;
using VianaSoft.BuildingBlocks.Core.Notifications.Enumerators;
using VianaSoft.BuildingBlocks.Core.Notifications.Interfaces;
using VianaSoft.BuildingBlocks.Core.Notifications;
using VianaSoft.BuildingBlocks.Core.Resources.Interfaces;
using VianaSoft.Identity.App.Interfaces;
using VianaSoft.Identity.App.Models.Request;
using VianaSoft.Identity.App.Models.Response;
using VianaSoft.BuildingBlocks.Core.Identity;
using Microsoft.AspNetCore.Authentication;

namespace VianaSoft.Identity.Api.Controllers
{
    [Route("v1/Identity")]
    [Authorize]
    public class IdentityController : MainControllerBase
    {
        #region Properties

        private readonly INotifier _notifier;
        private readonly ILanguageMessage _responseMessage;
        private readonly IIdentityApplication _application;

        #endregion

        #region Builders
        public IdentityController(INotifier notifier,
                                  ILanguageMessage responseMessage,
                                  IIdentityApplication application) : base(notifier)
        {
            _notifier = notifier;
            _responseMessage = responseMessage;
            _application = application;

            _notifier.Add(_responseMessage.RequestSuccessfullyReceivedController(), false, HttpStatusCode.OK, TypeError.Success);
        }

        #endregion

        #region Public Methods

        [HttpGet]
        [Route("User/Profile")]
        [ClaimsAuthorize("BackOffice", "Read")]
        [ProducesResponseType(typeof(ProfileResponse), 200)]
        [ProducesResponseType(typeof(MessageErrors), 400)]
        [SwaggerOperation(Summary = "Get Profile")]
        public async Task<IActionResult> GetProfileAsync()
        {
            var result = await _application.GetProfileAsync();
            return CustomResponse(result);
        }

        [HttpPost]
        [Route("User/Login")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(UserLoginResponse), 200)]
        [ProducesResponseType(typeof(MessageErrors), 400)]
        [SwaggerOperation(Summary = "Login")]
        public async Task<IActionResult> LoginUserAsync(UserLoginRequest request)
        {
            var result = await _application.LoginAsync(request);
            return CustomResponse(result);
        }

        [HttpPost]
        [Route("User/Register")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(UserLoginResponse), 200)]
        [ProducesResponseType(typeof(MessageErrors), 400)]
        [SwaggerOperation(Summary = "New Account")]
        public async Task<IActionResult> RegisterUserAsync(UserRegistrationRequest request)
        {
            var result = await _application.RegisterUserAsync(request);
            return CustomResponse(result);
        }

        [HttpPost()]
        [Route("User/Forgot-Password")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(Message), 200)]
        [ProducesResponseType(typeof(MessageErrors), 400)]
        [SwaggerOperation(Summary = "Forgot Password")]
        public async Task<IActionResult> ForgotPasswordAsync(ForgotPasswordRequest model)
        {
            var result = await _application.ForgotPasswordAsync(model);
            return CustomResponse(result);
        }

        [HttpPost()]
        [Route("User/Reset-Password")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(Message), 200)]
        [ProducesResponseType(typeof(MessageErrors), 400)]
        [SwaggerOperation(Summary = "Reset Password")]
        public async Task<IActionResult> ResetPasswordAsync(ResetPasswordRequest model)
        {
            var result = await _application.ResetPasswordAsync(model);
            return CustomResponse(result);
        }

        [HttpPost()]
        [Route("User/Change-Password")]
        [ClaimsAuthorize("BackOffice", "Update")]
        [ProducesResponseType(typeof(Message), 200)]
        [ProducesResponseType(typeof(MessageErrors), 400)]
        [SwaggerOperation(Summary = "Change Password")]
        public async Task<IActionResult> ChangePasswordAsync(ChangePasswordRequest model)
        {
            var result = await _application.ChangePasswordAsync(model);
            return CustomResponse(result);
        }

        [HttpPost]
        [Route("User/ResetPasswordTokenValid")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(Message), 200)]
        [ProducesResponseType(typeof(MessageErrors), 400)]
        [SwaggerOperation(Summary = "Checks if the password reset token is valid")]
        public async Task<IActionResult> IsResetPasswordTokenValidAsync(ResetPasswordTokenValidRequest request)
        {
            var result = await _application.IsResetPasswordTokenValidAsync(request.Email, request.Token);
            return CustomResponse(result);
        }

        [HttpPost()]
        [Route("User/Logout")]
        [ClaimsAuthorize("BackOffice", "Read")]
        [ProducesResponseType(typeof(Message), 200)]
        [ProducesResponseType(typeof(MessageErrors), 400)]
        [SwaggerOperation(Summary = "Logout")]
        public async Task<IActionResult> LogoutAsync()
        {
            await _application.LogoutAsync();
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            return CustomResponse(Ok());
        }

        #endregion
    }
}