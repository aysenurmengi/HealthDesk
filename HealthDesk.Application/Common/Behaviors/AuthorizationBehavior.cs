using MediatR;
using HealthDesk.Application.Common.Exceptions;
using HealthDesk.Application.Common.Interfaces;
using HealthDesk.Application.Features.Auth.Commands;

namespace HealthDesk.Application.Common.Behaviors
{
    public sealed class AuthorizationBehavior<TRequest, TResponse> 
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly ICurrentUserService _currentUser;

        public AuthorizationBehavior(ICurrentUserService currentUser)
        {
            _currentUser = currentUser;
        }

        public async Task<TResponse> Handle(
            TRequest request, 
            RequestHandlerDelegate<TResponse> next, 
            CancellationToken cancellationToken)
        {
            // Bu işlemler anonim olmalıdır → Authorization atlanır
            if (request is RegisterUserCommand ||
                request is LoginUserCommand ||
                request is RefreshTokenCommand)
            {
                return await next();
            }

            // Kullanıcı giriş yapmamışsa → engelle
            if (_currentUser.UserId is null)
                throw new UnauthorizedAccessException("User not authenticated.");

            return await next();
        }
    }
}
