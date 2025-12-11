namespace HealthDesk.Application.Features.Auth.Dtos;

public record CurrentUserDto(
    int Id,
    string FullName,
    string Email,
    string Role
);
