using AutoMapper;
using HealthDesk.Application.Common.Constants;
using HealthDesk.Application.Common.Exceptions;
using HealthDesk.Application.Common.Interfaces;
using HealthDesk.Application.DTOs;
using MediatR;

namespace HealthDesk.Application.Features.Appointments.Queries
{
    public sealed class GetMyAppointmentsHandler : IRequestHandler<GetMyAppointmentsQuery, IEnumerable<AppointmentDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUser;

        public GetMyAppointmentsHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUser)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUser = currentUser;
        }

        public async Task<IEnumerable<AppointmentDto>> Handle(GetMyAppointmentsQuery request, CancellationToken cancellationToken)
        {
            if (_currentUser.UserId is null)
                throw new UnauthorizedAccessException("User not authenticated.");

            if (_currentUser.Role == UserRoles.Doctor)
            {
                var doctor = await _unitOfWork.Doctors.GetByUserIdAsync(_currentUser.UserId.Value)
                    ?? throw new NotFoundException($"UserId={_currentUser.UserId.Value}");
                var appointments = await _unitOfWork.Appointments.GetByDoctorIdWithDetailsAsync(doctor.Id);
                return _mapper.Map<IEnumerable<AppointmentDto>>(appointments);
            }

            if (_currentUser.Role == UserRoles.Patient)
            {
                var patient = await _unitOfWork.Patients.GetByUserIdAsync(_currentUser.UserId.Value)
                    ?? throw new NotFoundException($"UserId={_currentUser.UserId.Value}");
                var appointments = await _unitOfWork.Appointments.GetByPatientIdWithDetailsAsync(patient.Id);
                return _mapper.Map<IEnumerable<AppointmentDto>>(appointments);
            }

            throw new ForbiddenAccessException();
        }
    }
}
