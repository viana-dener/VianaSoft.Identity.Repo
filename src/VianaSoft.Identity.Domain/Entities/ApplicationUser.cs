using Microsoft.AspNetCore.Identity;

namespace VianaSoft.Identity.Domain.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string? Name { get; set; }
        public string? UrlImage { get; set; }
    }
}
