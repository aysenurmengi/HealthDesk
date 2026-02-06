using HealthDesk.Application.Common.Interfaces;
using HealthDesk.Application.Common.Interfaces.Repositories;
using HealthDesk.Application.Features.Auth.Dtos;
using HealthDesk.Domain.Entities;
using HealthDesk.Domain.Enums;
using MediatR;
using static HealthDesk.Domain.Common.ValueObjects;

namespace HealthDesk.Application.Features.Auth.Commands.RegisterPatient
{
    public sealed class RegisterPatientHandler : IRequestHandler<RegisterPatientCommand, RegisteredUserDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IEmailService _emailService;
        private readonly IUnitOfWork _unitOfWork;

        public RegisterPatientHandler(
            IUserRepository userRepository,
            IPasswordHasher passwordHasher,
            IEmailService emailService,
            IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _emailService = emailService;
            _unitOfWork = unitOfWork;
        }

        public async Task<RegisteredUserDto> Handle(RegisterPatientCommand request, CancellationToken cancellationToken)
        {
            var existingUser = await _userRepository.GetByEmailAsync(request.Email);
            if (existingUser != null)
                throw new Exception("Bu email zaten kayıtlı.");

            var passwordHash = _passwordHasher.Hash(request.Password);

            var user = new User(
                fullName: request.Name,
                email: new Email(request.Email),
                passwordHash: passwordHash,
                role: UserRole.Patient
            );

            await _userRepository.AddAsync(user);

            var patient = new Patient(
                fullName: request.Name,
                userId: user.Id
            );

            await _unitOfWork.Patients.AddAsync(patient);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            await _emailService.SendEmailAsync(
                to: user.Email.Address,
                subject: "Welcome to HealthDesk",
                body: $"<p>Hi {user.FullName},</p><p>Your patient account has been created successfully.</p>"
            );

            return new RegisteredUserDto(
                user.Id,
                user.Email.Address,
                user.FullName
            );
        }
    }
}
