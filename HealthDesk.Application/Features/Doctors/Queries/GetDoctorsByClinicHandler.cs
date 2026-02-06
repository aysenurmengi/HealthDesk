using AutoMapper;
using HealthDesk.Application.Common.Interfaces;
using HealthDesk.Application.DTOs;
using MediatR;

namespace HealthDesk.Application.Features.Doctors.Queries
{
    public sealed class GetDoctorsByClinicHandler : IRequestHandler<GetDoctorsByClinicQuery, IEnumerable<DoctorDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetDoctorsByClinicHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<DoctorDto>> Handle(GetDoctorsByClinicQuery request, CancellationToken cancellationToken)
        {
            var doctors = await _unitOfWork.Doctors.GetByClinicIdAsync(request.ClinicId);
            return _mapper.Map<IEnumerable<DoctorDto>>(doctors);
        }
    }
}
