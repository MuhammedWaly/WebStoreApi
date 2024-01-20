using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using Org.BouncyCastle.Security;
using System.Net;
using System.Threading.Tasks;
using WebStoreApi.Settings;

namespace WebStoreApi.Services;

public class MailingService : IMailingService
{
    private readonly MailSettings _mailsettings;

    public MailingService(IOptions<MailSettings> mailsettings)
    {
        _mailsettings = mailsettings.Value;
    }

    public async Task SendEmailAsync(string mailTo, string Subject, string Body, IList<IFormFile> attachments = null)
    {
       

        var email = new MimeMessage()
        {
            Sender = MailboxAddress.Parse(_mailsettings.Email),
            Subject = Subject,
        };
        email.To.Add(MailboxAddress.Parse(mailTo));

        var bulider = new BodyBuilder();
        if (attachments!= null)
        {
            byte[] fileBytes;
            foreach (var file in attachments)
            {
                if (file.Length > 0)
                {
                    using var ms = new MemoryStream();
                    file.CopyTo(ms);
                    fileBytes = ms.ToArray();

                    bulider.Attachments.Add(file.FileName, fileBytes,ContentType.Parse(file.ContentType));
                }
            }
        }
        bulider.HtmlBody = Body;
        email.Body = bulider.ToMessageBody();
        email.From.Add( new MailboxAddress (_mailsettings.DisplayName,_mailsettings.Email));

        using var Smtp = new SmtpClient();
        Smtp.Connect(_mailsettings.Host,_mailsettings.Port, SecureSocketOptions.StartTls);
        Smtp.Authenticate(_mailsettings.Email,_mailsettings.Password);

        await Smtp.SendAsync(email);

        Smtp.Disconnect(true);
    }
}

