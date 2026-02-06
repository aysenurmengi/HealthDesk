using AutoMapper;
using HealthDesk.Application.Common.Helpers;
using HealthDesk.Application.Common.Interfaces;
using HealthDesk.Application.DTOs;
using MediatR;

namespace HealthDesk.Application.Features.Appointments.Commands.UpdateAppointment
{
    public class UpdateAppointmentHandler : IRequestHandler<UpdateAppointmentCommand, AppointmentDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateAppointmentHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<AppointmentDto> Handle(UpdateAppointmentCommand request, CancellationToken cancellationToken)
        {
            var appointment = await _unitOfWork.Appointments.GetByIdAsync(request.AppointmentId);
            if (appointment is null)
                throw new KeyNotFoundException($"Appointment with ID {request.AppointmentId} not found.");

            var newStartsAtUtc = request.NewStartsAt.Kind switch
            {
                DateTimeKind.Utc => request.NewStartsAt,
                DateTimeKind.Local => request.NewStartsAt.ToUniversalTime(),
                _ => DateTime.SpecifyKind(request.NewStartsAt, DateTimeKind.Utc)
            };

            var blocks = await _unitOfWork.DoctorAvailabilities
                .GetByDoctorAndDayAsync(appointment.DoctorId, newStartsAtUtc.DayOfWeek);
            if (!blocks.Any())
                throw new InvalidOperationException("Doctor has no availability configured for this day.");

            var appointments = await _unitOfWork.Appointments
                .GetByDoctorAndDateAsync(appointment.DoctorId, newStartsAtUtc);
            appointments = appointments.Where(a => a.Id != appointment.Id);

            var availableStarts = AvailabilitySlotHelper
                .BuildAvailableSlots(newStartsAtUtc, blocks, appointments);

            if (!availableStarts.Contains(newStartsAtUtc))
                throw new InvalidOperationException("Requested time is outside of the doctor's availability.");

            appointment.Reschedule(newStartsAtUtc);

            await _unitOfWork.Appointments.UpdateAsync(appointment);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<AppointmentDto>(appointment);
            
        }
    }
}
