using HealthDesk.Application.Common.Constants;
using HealthDesk.Application.Common.Exceptions;
using HealthDesk.Application.Common.Interfaces;
using HealthDesk.Application.Common.Interfaces.Repositories;
using HealthDesk.Application.Features.Auth.Dtos;
using HealthDesk.Domain.Entities;
using HealthDesk.Domain.Enums;
using MediatR;
using static HealthDesk.Domain.Common.ValueObjects;

namespace HealthDesk.Application.Features.Auth.Commands.RegisterDoctor
{
    public sealed class RegisterDoctorHandler : IRequestHandler<RegisterDoctorCommand, RegisteredUserDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IEmailService _emailService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;

        public RegisterDoctorHandler(
            IUserRepository userRepository,
            IPasswordHasher passwordHasher,
            IEmailService emailService,
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUser)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _emailService = emailService;
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }

        public async Task<RegisteredUserDto> Handle(RegisterDoctorCommand request, CancellationToken cancellationToken)
        {
            if (_currentUser.Role != UserRoles.Admin)
                throw new ForbiddenAccessException();

            var existingUser = await _userRepository.GetByEmailAsync(request.Email);
            if (existingUser != null)
                throw new Exception("Bu email zaten kayıtlı.");

            var clinic = await _unitOfWork.Clinics.GetByIdAsync(request.ClinicId)
                ?? throw new NotFoundException(request.ClinicId);

            var passwordHash = _passwordHasher.Hash(request.Password);

            var user = new User(
                fullName: request.Name,
                email: new Email(request.Email),
                passwordHash: passwordHash,
                role: UserRole.Doctor
            );

            await _userRepository.AddAsync(user);

            var doctor = new Doctor(
                fullName: request.Name,
                clinicId: clinic.Id,
                userId: user.Id,
                specialty: request.Specialty
            );

            await _unitOfWork.Doctors.AddAsync(doctor);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            await _emailService.SendEmailAsync(
                to: user.Email.Address,
                subject: "Welcome to HealthDesk",
                body: $"<p>Hi {user.FullName},</p><p>Your doctor account has been created successfully.</p>"
            );

            return new RegisteredUserDto(
                user.Id,
                user.Email.Address,
                user.FullName
            );
        }
    }
}
