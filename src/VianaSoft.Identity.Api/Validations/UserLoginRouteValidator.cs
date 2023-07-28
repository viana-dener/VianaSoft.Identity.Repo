using FluentValidation;
using VianaSoft.BuildingBlocks.Core.Resources.Interfaces;
using VianaSoft.Identity.App.Models.Request;

namespace VianaSoft.Identity.Api.Validations
{
    public class UserLoginRouteValidator : AbstractValidator<UserLoginRequest>
    {
        #region Properties

        private readonly ILanguageMessage _responseMessage;

        #endregion

        #region Builders

        public UserLoginRouteValidator(ILanguageMessage responseMessage)
        {
            _responseMessage = responseMessage;
            ValidateRoute();
        }

        #endregion

        #region Private Methods

        private void ValidateRoute()
        {
            RuleFor(model => model.Email)
                .NotEmpty()
                .WithMessage(_responseMessage.Required("Email"))
                .EmailAddress()
                .WithMessage(_responseMessage.InvalidEmail());

            RuleFor(model => model.Password)
                .NotEmpty()
                .WithMessage(_responseMessage.Required("Password"))
                .MinimumLength(8)
                .WithMessage(_responseMessage.MinValueCharacters("8"));
        }

        #endregion
    }
}
