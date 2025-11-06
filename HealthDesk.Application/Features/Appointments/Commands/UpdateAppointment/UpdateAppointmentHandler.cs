using AutoMapper;
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

            appointment.Reschedule(request.NewStartsAt);

            await _unitOfWork.Appointments.UpdateAsync(appointment);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<AppointmentDto>(appointment);
            
        }
    }
}