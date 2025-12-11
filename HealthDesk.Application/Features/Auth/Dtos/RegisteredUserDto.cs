namespace HealthDesk.Application.Features.Auth.Dtos;
public record RegisteredUserDto(
    int Id,
    string Email,
    string Name
);