using AutoMapper;
using HealthDesk.Application.Common.Interfaces;
using HealthDesk.Application.DTOs;
using MediatR;

namespace HealthDesk.Application.Features.Appointments.Queries
{
    public sealed class GetAllAppointmentsHandler : IRequestHandler<GetAllAppointmentsQuery, IEnumerable<AppointmentDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllAppointmentsHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<AppointmentDto>> Handle(GetAllAppointmentsQuery req, CancellationToken ct)
        {
            var appointments = await _unitOfWork.Appointments.GetAllAsync();
            return _mapper.Map<IEnumerable<AppointmentDto>>(appointments);
        }
    }
}