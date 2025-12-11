namespace HealthDesk.Application.Features.Auth.Dtos;
public record LoginUserDto(
    int Id,
    string FullName,
    string Email,
    string AccessToken,
    string RefreshToken
);

