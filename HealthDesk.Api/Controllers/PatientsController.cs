using HealthDesk.Application.Features.Patients.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthDesk.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PatientsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public PatientsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{id}")]
        [Authorize(Roles ="Admin, Doctor")]
        public async Task<IActionResult> GetPatientById(int id)
        {
            var result = await _mediator.Send(new GetPatientByIdQuery(id));
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }
    }
}