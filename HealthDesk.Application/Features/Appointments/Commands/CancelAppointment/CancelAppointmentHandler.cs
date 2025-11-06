using AutoMapper;
using HealthDesk.Application.Common.Interfaces;
using HealthDesk.Application.DTOs;
using MediatR;

namespace HealthDesk.Application.Features.Appointments.Commands.CancelAppointment
{
    public class CancelAppointmentHandler : IRequestHandler<CancelAppointmentCommand, AppointmentDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;
        public CancelAppointmentHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }
        
        public async Task<AppointmentDto> Handle(CancelAppointmentCommand request, CancellationToken cancellationToken)
        {
            var appointment = await _unitOfWork.Appointments.GetByIdAsync(request.AppointmentId);
            if (appointment is null)
                throw new KeyNotFoundException($"Appointment with ID {request.AppointmentId} not found.");

            //yetki kontrolü
            if (user.Role == "Patient" && appointment.PatientId != user.Id)
                throw new UnauthorizedAccessException("Patients can only cancel their own appointments.");
            
            if (user.Role == "Doctor" && appointment.DoctorId != user.Id)
                throw new UnauthorizedAccessException("Doctors can only cancel their own appointments.");

            appointment.Cancel();

            await _unitOfWork.Appointments.UpdateAsync(appointment);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<AppointmentDto>(appointment);
        }
    }
}