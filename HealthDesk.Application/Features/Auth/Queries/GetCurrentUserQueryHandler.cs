using HealthDesk.Application.Common.Interfaces;
using HealthDesk.Application.Common.Interfaces.Repositories;
using HealthDesk.Application.Features.Auth.Dtos;
using MediatR;

namespace HealthDesk.Application.Features.Auth.Queries;

public class GetCurrentUserQueryHandler
    : IRequestHandler<GetCurrentUserQuery, CurrentUserDto>
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IUserRepository _userRepository;

    public GetCurrentUserQueryHandler(
        ICurrentUserService currentUserService,
        IUserRepository userRepository)
    {
        _currentUserService = currentUserService;
        _userRepository = userRepository;
    }

    public async Task<CurrentUserDto> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId;

        if (userId == null)
            throw new Exception("Kullanıcı oturum açmamış.");

        var user = await _userRepository.GetByIdAsync(userId.Value);
        if (user == null)
            throw new Exception("Kullanıcı bulunamadı.");

        return new CurrentUserDto(
            user.Id,
            user.FullName,
            user.Email.Address,
            user.Role.ToString()
        );
    }
}
