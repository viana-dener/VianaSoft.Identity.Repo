using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VianaSoft.Identity.Domain.Entities;

namespace VianaSoft.Identity.Data.Context
{
    public class IdentityDataContext : IdentityDbContext
    {
        #region Builders
        public IdentityDataContext(DbContextOptions<IdentityDataContext> options) : base(options)
        {
        }

        public DbSet<ApplicationUser> Users { get; set; }

        #endregion
    }
}
