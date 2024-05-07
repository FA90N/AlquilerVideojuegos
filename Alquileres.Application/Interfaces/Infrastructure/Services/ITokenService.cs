using Microsoft.AspNetCore.Identity;

namespace Alquileres.Application.Interfaces.Infrastructure.Services
{
    public interface ITokenService
    {
        Task<string> GenerateTokenAsync(IdentityUser user);
    }
}
