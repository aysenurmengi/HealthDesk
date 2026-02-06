using HealthDesk.Application.Features.Clinics.Queries;
using HealthDesk.Application.Features.Clinics.Commands;
using HealthDesk.Application.Features.Clinics.Commands.UpdateClinic;
using HealthDesk.Application.Features.Clinics.Commands.DeleteClinic;
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
        public async Task<IActionResult> GetAllClinics([FromQuery] string? city)
        {
            var result = await _mediator.Send(new GetAllClinicsQuery(city));
            return Ok(result);
        }

        [HttpGet("cities")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCities()
        {
            var result = await _mediator.Send(new GetClinicCitiesQuery());
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

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateClinic(int id, [FromBody] UpdateClinicCommand command)
        {
            if (id != command.Id)
                return BadRequest("Id in route and body must match.");

            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteClinic(int id)
        {
            await _mediator.Send(new DeleteClinicCommand(id));
            return NoContent();
        }
    }
}
