using HealthDesk.Application.Common.Interfaces;
using MediatR;

namespace HealthDesk.Application.Features.Clinics.Queries
{
    public sealed class GetClinicCitiesHandler : IRequestHandler<GetClinicCitiesQuery, IEnumerable<string>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetClinicCitiesHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<string>> Handle(GetClinicCitiesQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Clinics.GetCitiesAsync();
        }
    }
}
