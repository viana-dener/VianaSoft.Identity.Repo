using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using VianaSoft.BuildingBlocks.Core.Configuration;
using VianaSoft.Identity.Domain.Interfaces;

namespace VianaSoft.Identity.Services.SendGrid
{
    public class SendGridEmailService : ISendGridEmail
    {
        #region Properties

        private readonly SendGridClient _client;
        private readonly ApplicationSettings _applicationSettings;

        #endregion

        #region Builders

        public SendGridEmailService(IOptions<ApplicationSettings> applicationSettings)
        {
            _applicationSettings = applicationSettings.Value;

            _client = new SendGridClient(_applicationSettings.SendGridSettings.ApiKey);
        }

        #endregion

        #region Public Methods

        public async Task<bool> SendPasswordResetEmail(string email, string subject, string link)
        {
            dynamic templateData = new
            {
                Subject = subject,
                BackendLink = link
            };

            var msg = MailHelper.CreateSingleTemplateEmail(
                new EmailAddress(_applicationSettings.SendGridSettings.FromAddress, _applicationSettings.Application),
                new EmailAddress(email), _applicationSettings.SendGridSettings.TemplateIdForgot, templateData);

            var response = await _client.SendEmailAsync(msg);
            if (response.StatusCode != System.Net.HttpStatusCode.Accepted)
                return false;

            return true;
        }

        #endregion
    }
}
