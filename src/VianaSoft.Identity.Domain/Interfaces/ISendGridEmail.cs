namespace VianaSoft.Identity.Domain.Interfaces
{
    public interface ISendGridEmail
    {
        Task<bool> SendPasswordResetEmail(string email, string subject, string link);
    }
}
