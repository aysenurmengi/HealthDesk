using AutoMapper;
using HealthDesk.Application.Common.Constants;
using HealthDesk.Application.Common.Exceptions;
using HealthDesk.Application.Common.Interfaces;
using HealthDesk.Application.DTOs;
using HealthDesk.Domain.Entities;
using MediatR;

namespace HealthDesk.Application.Features.Prescriptions.Commands.CreatePrescription
{
    public sealed class CreatePrescriptionHandler : IRequestHandler<CreatePrescriptionCommand, PrescriptionDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUser;

        public CreatePrescriptionHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUser)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUser = currentUser;
        }

        public async Task<PrescriptionDto> Handle(CreatePrescriptionCommand request, CancellationToken cancellationToken)
        {
            if (_currentUser.UserId is null)
                throw new UnauthorizedAccessException("User not authenticated.");

            if (_currentUser.Role != UserRoles.Doctor)
                throw new ForbiddenAccessException();

            var doctor = await _unitOfWork.Doctors.GetByUserIdAsync(_currentUser.UserId.Value)
                ?? throw new ForbiddenAccessException();

            var appointment = await _unitOfWork.Appointments.GetByIdAsync(request.AppointmentId)
                ?? throw new NotFoundException(request.AppointmentId);

            if (appointment.DoctorId != doctor.Id)
                throw new ForbiddenAccessException();

            var existing = await _unitOfWork.Prescriptions.GetByAppointmentIdAsync(request.AppointmentId);
            if (existing is not null)
                throw new InvalidOperationException("Prescription already exists for this appointment.");

            var prescription = new Prescription(
                appointmentId: appointment.Id,
                doctorId: doctor.Id,
                patientId: appointment.PatientId,
                content: request.Content
            );

            await _unitOfWork.Prescriptions.AddAsync(prescription);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var created = await _unitOfWork.Prescriptions.GetByAppointmentIdAsync(appointment.Id)
                ?? throw new NotFoundException(appointment.Id);

            return _mapper.Map<PrescriptionDto>(created);
        }
    }
}
