using System.Runtime.CompilerServices;
using HealthDesk.Application.Common.Interfaces;
using HealthDesk.Application.Common.Interfaces.Repositories;
using HealthDesk.Application.Features.Auth.Commands;
using HealthDesk.Application.Features.Auth.Dtos;
using HealthDesk.Domain.Entities;
using MediatR;

public class LoginUserHandler
    : IRequestHandler<LoginUserCommand, LoginUserDto>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtService _jwtService;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    public LoginUserHandler(IUserRepository userRepository, IPasswordHasher passwordHasher, IJwtService jwtService, IRefreshTokenRepository refreshTokenRepository)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwtService = jwtService;
        _refreshTokenRepository = refreshTokenRepository;

    }
    public async Task<LoginUserDto> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email);
        if (user == null)
            throw new Exception("Kullanıcı bulunamadı.");
        
        var isPasswordValid = _passwordHasher.Verify(request.Password, user.PasswordHash);
        if (!isPasswordValid)
            throw new Exception("Geçersiz şifre.");

        var accessToken = _jwtService.GenerateAccessToken(user);
        var refreshToken = _jwtService.GenerateRefreshToken();

        var refreshTokenEntity = new RefreshToken(
            token: refreshToken,
            userId: user.Id,
            createdAt: DateTime.UtcNow,
            expiresAt: DateTime.UtcNow.AddDays(7)
        );

        await _refreshTokenRepository.AddAsync(refreshTokenEntity);
        
        return new LoginUserDto(
            user.Id,
            user.FullName,
            user.Email.Address,
            accessToken,
            refreshToken
        );
        
    }
}

