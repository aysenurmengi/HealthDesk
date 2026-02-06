using HealthDesk.Application.Common.Interfaces;
using HealthDesk.Application.Common.Interfaces.Repositories;
using MediatR;

namespace HealthDesk.Application.Features.Auth.Commands
{
    public sealed class LogoutUserHandler : IRequestHandler<LogoutUserCommand>
    {
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly ICurrentUserService _currentUser;

        public LogoutUserHandler(IRefreshTokenRepository refreshTokenRepository, ICurrentUserService currentUser)
        {
            _refreshTokenRepository = refreshTokenRepository;
            _currentUser = currentUser;
        }

        public async Task Handle(LogoutUserCommand request, CancellationToken cancellationToken)
        {
            if (_currentUser.UserId is null)
                throw new UnauthorizedAccessException("User not authenticated.");

            if (string.IsNullOrWhiteSpace(request.RefreshToken))
                throw new UnauthorizedAccessException("Refresh token is invalid.");

            var stored = await _refreshTokenRepository.GetByTokenAsync(request.RefreshToken);
            if (stored is null || stored.UserId != _currentUser.UserId.Value)
                throw new UnauthorizedAccessException("Refresh token is invalid.");

            if (!stored.IsRevoked)
            {
                stored.Revoke();
                await _refreshTokenRepository.UpdateAsync(stored);
            }
        }
    }
}
