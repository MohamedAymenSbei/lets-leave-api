using lets_leave.Models;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;

namespace lets_leave.Services.MailService;

public class MailService : IMailService
{
    private readonly IConfiguration _configuration;

    public MailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public async Task<ServerResponse<string>> SendCredentials(string sendTo, string registerMail, string password)
    {
        var response = new ServerResponse<string>();
        var email = new MimeMessage();

        email.From.Add(MailboxAddress.Parse("leave-system@example.com"));
        email.To.Add(MailboxAddress.Parse(sendTo));
        email.Subject = "Leave Management System - Login Credentials";
        email.Body = new TextPart(TextFormat.Html)
        {
            Text = "<h3>Your login credentials :</h3>" +
                   "<ul>" +
                   $"<li><b>Email Address: </b>{registerMail}</li>" +
                   $"<li><b>Password: </b>{password}</li>" +
                   "</ul>"
        };
        using var smtp = new SmtpClient();
        try
        {
            await smtp.ConnectAsync(_configuration["Smtp:Host"],587,SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_configuration["Smtp:UserName"],_configuration["Smtp:Password"]);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
            response.Data = "Email has been send";
        }
        catch (Exception e)
        {
            response.Success = false;
            response.Message = e.Message;
        }
        return response;
    }
}