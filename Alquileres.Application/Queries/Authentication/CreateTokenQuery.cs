using Alquileres.Application.Configuration.CQRS;
using Alquileres.Application.Interfaces.Infrastructure.Services;
using Alquileres.Application.Models;
using CommunityToolkit.Diagnostics;
using Microsoft.AspNetCore.Identity;

namespace Alquileres.Application.Queries.Authentication
{
    public class CreateTokenQuery : IQuery<TokenResultDto>
    {
        public string UserId => throw new NotImplementedException();

        public UserSignInDto UserSignInDto { get; }

        public CreateTokenQuery(UserSignInDto userSignInDto)
        {
            UserSignInDto = userSignInDto;
        }
    }

    public class CreateTokenQueryHandler : IQueryHandler<CreateTokenQuery, TokenResultDto>
    {
        private readonly ITokenService _tokenServices;
        private readonly UserManager<IdentityUser> _userManager;

        public CreateTokenQueryHandler(
            ITokenService tokenServices,
            UserManager<IdentityUser> userManager)
        {
            _tokenServices = tokenServices;
            _userManager = userManager;
        }

        public async Task<TokenResultDto> Handle(CreateTokenQuery request, CancellationToken cancellationToken)
        {
            // Verificamos credenciales con Identity
            var user = await _userManager.FindByNameAsync(request.UserSignInDto.UserName);

            if (user is null || !await _userManager.CheckPasswordAsync(user, request.UserSignInDto.Password))
            {
                ThrowHelper.ThrowArgumentException("El usuario o la contraseña no son válidos.");
            }

            var jwt = await _tokenServices.GenerateTokenAsync(user);

            return new TokenResultDto
            {
                AccessToken = jwt
            };
        }
    }
}
