using HealthDesk.Application.Common.Interfaces;
using HealthDesk.Application.Common.Interfaces.Repositories;
using HealthDesk.Application.Features.Auth.Dtos;
using MediatR;
using HealthDesk.Domain.Entities;
using static HealthDesk.Domain.Common.ValueObjects;
using HealthDesk.Domain.Enums;

namespace HealthDesk.Application.Features.Auth.Commands;

public class RegisterUserHandler 
    : IRequestHandler<RegisterUserCommand, RegisteredUserDto>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IEmailService _emailService;

    public RegisterUserHandler(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IEmailService emailService)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _emailService = emailService;
    }

    public async Task<RegisteredUserDto> Handle(
        RegisterUserCommand request, 
        CancellationToken cancellationToken)
    {
        // email kontrol
        var existingUser = await _userRepository.GetByEmailAsync(request.Email);
        if (existingUser != null)
            throw new Exception("Bu email zaten kayıtlı.");

        // şifre hash
        var passwordHash = _passwordHasher.Hash(request.Password);

        // domain user entity oluştur
        var user = new User(
            fullName: request.Name,
            email: new Email(request.Email),
            passwordHash: passwordHash,
            role: UserRole.Patient
        );

        await _userRepository.AddAsync(user);

        await _emailService.SendEmailAsync(
            to: user.Email.Address,
            subject: "Welcome to HealthDesk",
            body: $"<p>Hi {user.FullName},</p><p>Your account has been created successfully.</p>"
        );

        // dto dönüş
        return new RegisteredUserDto(
            user.Id,          
            user.Email.Address,     
            user.FullName      
        );
    }
}
