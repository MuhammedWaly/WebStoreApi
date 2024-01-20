namespace WebStoreApi.Services
{
    public interface IMailingService
    {
        Task SendEmailAsync(string mailTo, string Subject, string Body, IList<IFormFile> attachments = null);
    }
}
