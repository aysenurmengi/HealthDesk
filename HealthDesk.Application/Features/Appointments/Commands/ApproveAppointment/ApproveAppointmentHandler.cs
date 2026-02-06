using AutoMapper;
using HealthDesk.Application.Common.Constants;
using HealthDesk.Application.Common.Exceptions;
using HealthDesk.Application.Common.Interfaces;
using HealthDesk.Application.DTOs;
using MediatR;

namespace HealthDesk.Application.Features.Appointments.Commands.ApproveAppointment
{
    public sealed class ApproveAppointmentHandler : IRequestHandler<ApproveAppointmentCommand, AppointmentDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUser;

        public ApproveAppointmentHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUser)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUser = currentUser;
        }

        public async Task<AppointmentDto> Handle(ApproveAppointmentCommand request, CancellationToken cancellationToken)
        {
            if (_currentUser.Role != UserRoles.Doctor)
                throw new ForbiddenAccessException();

            var doctor = await _unitOfWork.Doctors.GetByUserIdAsync(_currentUser.UserId!.Value)
                ?? throw new ForbiddenAccessException();

            var appointment = await _unitOfWork.Appointments.GetByIdAsync(request.AppointmentId)
                ?? throw new NotFoundException(request.AppointmentId);

            if (appointment.DoctorId != doctor.Id)
                throw new ForbiddenAccessException();

            appointment.Approve();

            await _unitOfWork.Appointments.UpdateAsync(appointment);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var updated = await _unitOfWork.Appointments.GetByIdWithDetailsAsync(appointment.Id)
                ?? appointment;

            return _mapper.Map<AppointmentDto>(updated);
        }
    }
}
