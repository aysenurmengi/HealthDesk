using HealthDesk.Application.Features.Auth.Dtos;
using MediatR;

public record RefreshTokenCommand(
    string AccessToken,
    string RefreshToken
) : IRequest<LoginUserDto>;