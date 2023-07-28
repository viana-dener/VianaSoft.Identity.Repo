using FluentValidation;
using VianaSoft.BuildingBlocks.Core.Resources.Interfaces;
using VianaSoft.Identity.App.Models.Request;

namespace VianaSoft.Identity.Api.Validations
{
    public class ForgotPasswordRouteValidator : AbstractValidator<ForgotPasswordRequest>
    {
        #region Properties

        private readonly ILanguageMessage _responseMessage;

        #endregion

        #region Builders

        public ForgotPasswordRouteValidator(IHttpContextAccessor context, ILanguageMessage responseMessage)
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
        }

        #endregion
    }
}
