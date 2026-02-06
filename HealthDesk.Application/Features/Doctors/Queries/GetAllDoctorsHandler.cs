using AutoMapper;
using HealthDesk.Application.Common.Constants;
using HealthDesk.Application.Common.Exceptions;
using HealthDesk.Application.Common.Interfaces;
using HealthDesk.Application.DTOs;
using MediatR;

namespace HealthDesk.Application.Features.Doctors.Queries
{
    public sealed class GetAllDoctorsHandler : IRequestHandler<GetAllDoctorsQuery, IEnumerable<DoctorDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUser;

        public GetAllDoctorsHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUser)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUser = currentUser;
        }

        public async Task<IEnumerable<DoctorDto>> Handle(GetAllDoctorsQuery request, CancellationToken cancellationToken)
        {
            if (_currentUser.Role != UserRoles.Admin)
                throw new ForbiddenAccessException();

            var doctors = await _unitOfWork.Doctors.GetAllWithDetailsAsync();
            return _mapper.Map<IEnumerable<DoctorDto>>(doctors);
        }
    }
}
