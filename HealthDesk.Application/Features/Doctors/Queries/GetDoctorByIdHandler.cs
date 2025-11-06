using AutoMapper;
using HealthDesk.Application.Common.Constants;
using HealthDesk.Application.Common.Exceptions;
using HealthDesk.Application.Common.Interfaces;
using HealthDesk.Application.DTOs;
using MediatR;

namespace HealthDesk.Application.Features.Doctors.Queries
{
    public sealed class GetDoctorByIdHandler : IRequestHandler<GetDoctorByIdQuery, DoctorDto>
    {
        public readonly IUnitOfWork _unitOfWork;
        public readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUser;

        public GetDoctorByIdHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUser)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUser = currentUser;
        }

        public async Task<DoctorDto> Handle(GetDoctorByIdQuery request, CancellationToken cancellationToken)
        {
            var doctor = await _unitOfWork.Doctors.GetByIdAsync(request.Id) 
                ?? throw new NotFoundException(request.Id);
            if (_currentUser.Role != UserRoles.Admin &&
                !(_currentUser.Role == UserRoles.Doctor && _currentUser.UserId.Value == request.Id))
            {
                throw new ForbiddenAccessException();
            }
            return _mapper.Map<DoctorDto>(doctor);
        }
    }
}