using FluentValidation;
using VianaSoft.BuildingBlocks.Core.Resources.Interfaces;
using VianaSoft.Identity.App.Models.Request;

namespace VianaSoft.Identity.Api.Validations
{
    public class ResetPasswordTokenValidRouteValidator : AbstractValidator<ResetPasswordTokenValidRequest>
    {
        #region Properties

        private readonly ILanguageMessage _responseMessage;

        #endregion

        #region Builders

        public ResetPasswordTokenValidRouteValidator(IHttpContextAccessor context, ILanguageMessage responseMessage)
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

            RuleFor(model => model.Token)
                .NotEmpty()
                .WithMessage(_responseMessage.Required("Token"));
        }

        #endregion
    }
}
