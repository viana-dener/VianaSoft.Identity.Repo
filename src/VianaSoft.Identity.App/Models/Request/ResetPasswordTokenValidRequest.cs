namespace VianaSoft.Identity.App.Models.Request
{
    public class ResetPasswordTokenValidRequest
    {
        public string? Email { get; set; }
        public string? Token { get; set; }
    }
}
