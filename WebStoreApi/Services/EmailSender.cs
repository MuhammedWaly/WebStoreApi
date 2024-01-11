//using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
namespace WebStoreApi.Services;

public class EmailSender /*: IEmailSender*/
{
    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        var smtpClient = new SmtpClient(host:"smtp-mail.outlook.com")
        {
            Port = 587,
            Credentials = new NetworkCredential("mohamedwaly2272000@outlook.com", "sarawaly222"),
            EnableSsl = true,
        };

        var mailMessage = new MailMessage();
        mailMessage.From = new MailAddress("mohamedwaly2272000@outlook.com");
        mailMessage.Subject = subject;
        mailMessage.To.Add(email);
        mailMessage.Body = $"<html><body>{htmlMessage}</body></html>";
        mailMessage.IsBodyHtml = true;
        

        try
        {
           smtpClient.Send(mailMessage);
        }
        catch (Exception ex)
        {
            // Handle exception (log or throw)
            Console.WriteLine($"Error sending email: {ex.Message}");
        }
        finally
        {
            smtpClient.Dispose();
            mailMessage.Dispose();
        }
    }
}

