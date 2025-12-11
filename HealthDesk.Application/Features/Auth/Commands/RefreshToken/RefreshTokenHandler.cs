using System.Security.Claims;
using HealthDesk.Application.Common.Interfaces;
using HealthDesk.Application.Common.Interfaces.Repositories;
using HealthDesk.Application.Features.Auth.Dtos;
using HealthDesk.Domain.Entities;
using MediatR;

namespace HealthDesk.Application.Features.Auth.Commands;

public class RefreshTokenHandler
    : IRequestHandler<RefreshTokenCommand, LoginUserDto>
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtService _jwtService;
    private readonly IRefreshTokenRepository _refreshTokenRepository;

    public RefreshTokenHandler(
        IUserRepository userRepository,
        IJwtService jwtService,
        IRefreshTokenRepository refreshTokenRepository)
    {
        _userRepository = userRepository;
        _jwtService = jwtService;
        _refreshTokenRepository = refreshTokenRepository;
    }

    public async Task<LoginUserDto> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        //Access token claim'lerini oku
        var principal = _jwtService.GetPrincipalFromExpiredToken(request.AccessToken);
        Console.WriteLine("PRINCIPAL IS NULL? " + (principal == null));

        if (principal != null)
        {
            foreach (var c in principal.Claims)
                Console.WriteLine($"CLAIM: {c.Type} = {c.Value}");
        }

        var userIdString = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!int.TryParse(userIdString, out var userId))
            throw new Exception("Token içindeki kullanıcı bilgisi okunamadı.");

        Console.WriteLine("USERID: " + userId);

        //Refresh token DB'de var mı?
        var storedRefreshToken = await _refreshTokenRepository.GetByTokenAsync(request.RefreshToken);
        if (storedRefreshToken == null)
            throw new Exception("Refresh token geçersiz.");

        //Bu token doğru kullanıcıya mı ait?
        if (storedRefreshToken.UserId != userId)
            throw new Exception("Token kullanıcıyla eşleşmiyor.");

        //Token süresi dolmuş mu?
        if (storedRefreshToken.IsExpired)
            throw new Exception("Refresh token süresi dolmuş.");

        //Token zaten revoke edilmiş mi?
        if (storedRefreshToken.IsRevoked)
            throw new Exception("Refresh token iptal edilmiş.");

        // Kullanıcıyı DB’den çek
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
            throw new Exception("Kullanıcı bulunamadı.");

        //Yeni tokenlar oluştur
        var newAccessToken = _jwtService.GenerateAccessToken(user);
        var newRefreshToken = _jwtService.GenerateRefreshToken();

        //Eski refresh token'ı revoke et ve yeni token ile ilişkilendir
        storedRefreshToken.ReplaceWith(newRefreshToken);
        await _refreshTokenRepository.UpdateAsync(storedRefreshToken);

        //Yeni refresh token kaydet
        var refreshTokenEntity = new RefreshToken(
            token: newRefreshToken,
            userId: user.Id,
            createdAt: DateTime.UtcNow,
            expiresAt: DateTime.UtcNow.AddDays(7)
        );

        await _refreshTokenRepository.AddAsync(refreshTokenEntity);

        //Yeni tokenları döndür
        return new LoginUserDto(
            user.Id,
            user.FullName,
            user.Email.Address,
            newAccessToken,
            newRefreshToken
        );
    }
}
