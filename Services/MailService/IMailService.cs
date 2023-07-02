using lets_leave.Models;

namespace lets_leave.Services.MailService;

public interface IMailService
{
    Task<ServerResponse<string>> SendCredentials(string sendTo,string email, string password);
}