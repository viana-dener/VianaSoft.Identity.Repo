namespace VianaSoft.Identity.App.Models.Response
{
    public class UserTokenResponse
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? UrlImage { get; set; }
        public IEnumerable<UserClaimResponse>? Claims { get; set; }
    }
}
