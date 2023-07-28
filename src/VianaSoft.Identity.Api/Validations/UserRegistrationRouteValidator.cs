using FluentValidation;
using VianaSoft.BuildingBlocks.Core.Resources.Interfaces;
using VianaSoft.Identity.App.Models.Request;

namespace VianaSoft.Identity.Api.Validations
{
    public class UserRegistrationRouteValidator : AbstractValidator<UserRegistrationRequest>
    {
        #region Properties

        private readonly ILanguageMessage _responseMessage;

        #endregion

        #region Builders

        public UserRegistrationRouteValidator(IHttpContextAccessor context, ILanguageMessage responseMessage)
        {
            _responseMessage = responseMessage;
            ValidateRoute(context);
        }

        #endregion

        #region Private Methods

        private void ValidateRoute(IHttpContextAccessor context)
        {
            RuleFor(model => model.Name)
                .NotEmpty()
                .WithMessage(_responseMessage.Required("Name"));

            RuleFor(model => model.Phone)
                .NotEmpty()
                .WithMessage(_responseMessage.Required("Phone"));

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

            RuleFor(model => model.ConfirmPassword)
                .NotEmpty()
                .WithMessage(_responseMessage.Required("ConfirmPassword"))
                .MinimumLength(8)
                .WithMessage(_responseMessage.MinValueCharacters("8"));

            RuleFor(model => model.ConfirmPassword)
                .Equal(model => model.Password)
                .WithMessage(_responseMessage.PasswordsDontMatch());
        }

        #endregion
    }
}
