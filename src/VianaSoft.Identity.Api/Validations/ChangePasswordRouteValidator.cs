using FluentValidation;
using VianaSoft.BuildingBlocks.Core.Resources.Interfaces;
using VianaSoft.Identity.App.Models.Request;

namespace VianaSoft.Identity.Api.Validations
{
    public class ChangePasswordRouteValidator : AbstractValidator<ChangePasswordRequest>
    {
        #region Properties

        private readonly ILanguageMessage _responseMessage;

        #endregion

        #region Builders

        public ChangePasswordRouteValidator(IHttpContextAccessor context, ILanguageMessage responseMessage)
        {
            _responseMessage = responseMessage;
            ValidateRoute(context);
        }

        #endregion

        #region Private Methods

        private void ValidateRoute(IHttpContextAccessor context)
        {
            RuleFor(model => model.Email)
                .NotEmpty()
                .WithMessage(_responseMessage.Required("Email"))
                .EmailAddress()
                .WithMessage(_responseMessage.InvalidEmail());

            RuleFor(model => model.OldPassword)
                .NotEmpty()
                .WithMessage(_responseMessage.Required("OldPassword"))
                .MinimumLength(8)
                .WithMessage(_responseMessage.MinValueCharacters("8"));

            RuleFor(model => model.NewPassword)
                .NotEmpty()
                .WithMessage(_responseMessage.Required("NewPassword"))
                .MinimumLength(8)
                .WithMessage(_responseMessage.MinValueCharacters("8"));

            RuleFor(model => model.ConfirmPassword)
                .NotEmpty()
                .WithMessage(_responseMessage.Required("ConfirmPassword"))
                .MinimumLength(8)
                .WithMessage(_responseMessage.MinValueCharacters("8"));

            RuleFor(model => model.ConfirmPassword)
                .Equal(model => model.NewPassword)
                .WithMessage(_responseMessage.PasswordsDontMatch());
        }

        #endregion
    }
}
