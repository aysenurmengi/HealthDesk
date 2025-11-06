using MediatR;
using HealthDesk.Application.Common.Exceptions;
using HealthDesk.Application.Common.Interfaces;

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
            // Eğer kullanıcı girişi yoksa -> Unauthorized
            if (_currentUser.UserId is null)
                throw new UnauthorizedAccessException("User not authenticated.");

            // Eğer rol kontrolü gerekiyorsa burada yapılabilir.
            // (İleri seviye: Custom attribute ile [Authorize(Roles="Admin")])

            return await next();
        }
    }
}
