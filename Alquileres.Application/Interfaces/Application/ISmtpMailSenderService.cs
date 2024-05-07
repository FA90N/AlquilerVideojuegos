using Alquileres.Application.Models;

namespace Alquileres.Application.Interfaces.Application;

public interface ISmtpMailSenderService
{
    Task SendEmailAsync(MessageDto message);
}