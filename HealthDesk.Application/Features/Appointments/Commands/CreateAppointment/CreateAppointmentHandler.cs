using AutoMapper;
using HealthDesk.Application.Common.Helpers;
using HealthDesk.Application.Common.Interfaces;
using HealthDesk.Application.DTOs;
using HealthDesk.Domain.Entities;
using MediatR;

namespace HealthDesk.Application.Features.Appointments.Commands.CreateAppointment
{
    public sealed class CreateAppointmentHandler : IRequestHandler<CreateAppointmentCommand, AppointmentDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateAppointmentHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<AppointmentDto> Handle(CreateAppointmentCommand request, CancellationToken cancellationToken)
        {
            var startsAtUtc = request.StartsAt.Kind switch
            {
                DateTimeKind.Utc => request.StartsAt,
                DateTimeKind.Local => request.StartsAt.ToUniversalTime(),
                _ => DateTime.SpecifyKind(request.StartsAt, DateTimeKind.Utc)
            };

            var doctor = await _unitOfWork.Doctors.GetByIdAsync(request.DoctorId);
            var patient = await _unitOfWork.Patients.GetByIdAsync(request.PatientId);
            if (doctor is null)
                throw new KeyNotFoundException($"Doctor with ID {request.DoctorId} not found.");

            if (patient is null)
                throw new KeyNotFoundException($"Patient with ID {request.PatientId} not found.");

            var isAvailable = await _unitOfWork.Appointments
                .IsDoctorAvailableAsync(request.DoctorId, startsAtUtc);
            if (!isAvailable)
                throw new InvalidOperationException("Doctor is not available at the requested time.");

            var blocks = await _unitOfWork.DoctorAvailabilities
                .GetByDoctorAndDayAsync(request.DoctorId, startsAtUtc.DayOfWeek);
            if (!blocks.Any())
                throw new InvalidOperationException("Doctor has no availability configured for this day.");

            var appointments = await _unitOfWork.Appointments
                .GetByDoctorAndDateAsync(request.DoctorId, startsAtUtc);

            var availableStarts = AvailabilitySlotHelper
                .BuildAvailableSlots(startsAtUtc, blocks, appointments);

            if (!availableStarts.Contains(startsAtUtc))
                throw new InvalidOperationException("Requested time is outside of the doctor's availability.");

            var appointment = new Appointment(
                request.DoctorId,
                request.PatientId,
                doctor.ClinicId,
                startsAtUtc,
                request.Notes
            );

            await _unitOfWork.Appointments.AddAsync(appointment);
            await _unitOfWork.SaveChangesAsync();

            var created = await _unitOfWork.Appointments.GetByIdWithDetailsAsync(appointment.Id)
                ?? appointment;

            return _mapper.Map<AppointmentDto>(created);
        }
    }
}
