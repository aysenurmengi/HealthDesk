using AutoMapper;
using HealthDesk.Application.Common.Interfaces;
using HealthDesk.Application.DTOs;
using MediatR;

namespace HealthDesk.Application.Features.Clinics.Queries;
public sealed class GetAllClinicsHandler : IRequestHandler<GetAllClinicsQuery, IEnumerable<ClinicDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllClinicsHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    
    public async Task<IEnumerable<ClinicDto>> Handle(GetAllClinicsQuery req, CancellationToken ct)
    {
        var clinics = string.IsNullOrWhiteSpace(req.City)
            ? await _unitOfWork.Clinics.GetAllAsync()
            : await _unitOfWork.Clinics.GetByCityAsync(req.City);

        return _mapper.Map<IEnumerable<ClinicDto>>(clinics);
    }
}
