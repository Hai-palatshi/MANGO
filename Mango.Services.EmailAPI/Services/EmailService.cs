using Mango.Services.EmailAPI.Data;
using Mango.Services.EmailAPI.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace Mango.Services.EmailAPI.Services
{
    public class EmailService : IEmailService
    {

        private DbContextOptions<AppDbContext> dbOptions;
        public EmailService(DbContextOptions<AppDbContext> _dbOptions)
        {
            dbOptions = _dbOptions;
        }


        public async Task EmailCartAndLog(CartDto cartDto)
        {
            StringBuilder message = new StringBuilder();

            message.AppendLine("<br/>Cart Email Requested ");
            message.AppendLine("<br/>Total " + cartDto.CartHeader.CardTotal);
            message.Append("<br/>");
            message.Append("<ul>");
            foreach (var item in cartDto.CardDetail)
            {
                message.Append("<li>");
                message.Append(item.Product.Name + " x " + item.Count);
                message.Append("</li>");
            }
            message.Append("</ul>");
            await LogAndEmail(message.ToString(), cartDto.CartHeader.Email);
        }

        public async Task RegisterEmailAndLog(string email)
        {
            string message = "Email Registeration Sucessful . <br/> Email : " + email;
            await LogAndEmail(message, "Hai_Platchi");
        }

        private async Task<bool> LogAndEmail(string message, string email)
        {
            try
            {
                EmailLogger emailLog = new()
                {
                    Email = email,
                    EmailSend = DateTime.Now,
                    Message = message
                };

                await using var _db = new AppDbContext(dbOptions);
                await _db.EmailLoggers.AddAsync(emailLog);
                await _db.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

    }


}
