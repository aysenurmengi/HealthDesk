using AutoMapper;
using HealthDesk.Application.Common.Constants;
using HealthDesk.Application.Common.Exceptions;
using HealthDesk.Application.Common.Interfaces;
using HealthDesk.Application.DTOs;
using MediatR;

namespace HealthDesk.Application.Features.Doctors.Commands.UpdateDoctor
{
    public sealed class UpdateDoctorHandler : IRequestHandler<UpdateDoctorCommand, DoctorDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUser;

        public UpdateDoctorHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUser)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUser = currentUser;
        }

        public async Task<DoctorDto> Handle(UpdateDoctorCommand request, CancellationToken cancellationToken)
        {
            if (_currentUser.Role != UserRoles.Admin)
                throw new ForbiddenAccessException();

            var doctor = await _unitOfWork.Doctors.GetByIdWithDetailsAsync(request.Id)
                ?? throw new NotFoundException(request.Id);

            doctor.UpdateDetails(request.FullName, request.ClinicId, request.Specialty);

            await _unitOfWork.Doctors.UpdateAsync(doctor);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return _mapper.Map<DoctorDto>(doctor);
        }
    }
}
