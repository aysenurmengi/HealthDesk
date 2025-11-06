using AutoMapper;
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
            var doctor = await _unitOfWork.Doctors.GetByIdAsync(request.DoctorId);
            var patient = await _unitOfWork.Patients.GetByIdAsync(request.PatientId);
            if (doctor is null)
                throw new KeyNotFoundException($"Doctor with ID {request.DoctorId} not found.");

            if (patient is null)
                throw new KeyNotFoundException($"Patient with ID {request.PatientId} not found.");

            var isAvailable = await _unitOfWork.Appointments
                .IsDoctorAvailableAsync(request.DoctorId, request.StartsAt);
            if (!isAvailable)
                throw new InvalidOperationException("Doctor is not available at the requested time.");

            var appointment = new Appointment(
                request.DoctorId,
                request.PatientId,
                request.StartsAt,
                request.Notes
            );

            await _unitOfWork.Appointments.AddAsync(appointment);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<AppointmentDto>(appointment);
        }
    }
}