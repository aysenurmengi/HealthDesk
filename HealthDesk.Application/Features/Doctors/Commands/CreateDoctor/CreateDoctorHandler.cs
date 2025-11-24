using AutoMapper;
using HealthDesk.Application.Common.Constants;
using HealthDesk.Application.Common.Interfaces;
using HealthDesk.Application.DTOs;
using HealthDesk.Domain.Entities;
using MediatR;

namespace HealthDesk.Application.Features.Doctors.Commands.CreateDoctor
{
    public sealed class CreateDoctorHandler : IRequestHandler<CreateDoctorCommand, DoctorDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUser;

        public CreateDoctorHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUser)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUser = currentUser;
        }

        public async Task<DoctorDto> Handle(CreateDoctorCommand request, CancellationToken cancellationToken)
        {
            if (_currentUser.Role != UserRoles.Admin)
                throw new UnauthorizedAccessException("Only admins can create doctors.");

            var doctor = new Doctor
            (
                request.FullName,
                request.ClinicId,
                request.UserId,
                request.Specialty
            );

            await _unitOfWork.Doctors.AddAsync(doctor);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<DoctorDto>(doctor);
        }
    }
}