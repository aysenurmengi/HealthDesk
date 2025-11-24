using HealthDesk.Application.Features.Clinics.Queries;
using HealthDesk.Application.Features.Clinics.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace HealthDesk.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClinicsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ClinicsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet] // GET: api/Clinics
        [AllowAnonymous]
        public async Task<IActionResult> GetAllClinics()
        {
            var result = await _mediator.Send(new GetAllClinicsQuery());
            return Ok(result);
        }
        
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin, Doctor")]
        public async Task<IActionResult> GetClinicsById(int id)
        {
            var result = await _mediator.Send(new GetClinicByIdQuery(id));
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateClinics([FromBody] CreateClinicCommand command)
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetClinicsById), new { id = result.Id }, result);
        }

        //TASK-API: Implement Update and Delete endpoints
    }
}