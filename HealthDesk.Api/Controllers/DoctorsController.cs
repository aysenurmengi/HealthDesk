using HealthDesk.Application.Features.Doctors.Commands.CreateDoctor;
using HealthDesk.Application.Features.Doctors.Commands.UpdateDoctor;
using HealthDesk.Application.Features.Doctors.Commands.DeleteDoctor;
using HealthDesk.Application.Features.Doctors.Commands.AddDoctorAvailability;
using HealthDesk.Application.Features.Doctors.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthDesk.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DoctorsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public DoctorsController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllDoctors()
        {
            var result = await _mediator.Send(new GetAllDoctorsQuery());
            return Ok(result);
        }

        [HttpGet("by-clinic/{clinicId:int}")]
        [Authorize(Roles = "Admin, Doctor, Patient")]
        public async Task<IActionResult> GetDoctorsByClinic(int clinicId)
        {
            var result = await _mediator.Send(new GetDoctorsByClinicQuery(clinicId));
            return Ok(result);
        }

        [HttpGet("{id:int}/availability")]
        [Authorize(Roles = "Admin, Doctor, Patient")]
        public async Task<IActionResult> GetAvailability(int id, [FromQuery] DateTime date)
        {
            var result = await _mediator.Send(new GetDoctorAvailabilityQuery(id, date));
            return Ok(result);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin, Doctor")]
        public async Task<IActionResult> GetDoctorById(int id)
        {
            var result = await _mediator.Send(new GetDoctorByIdQuery(id));
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateDoctor([FromBody] CreateDoctorCommand command)
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetDoctorById), new { id = result.Id }, result);
        }

        [HttpPost("{id:int}/availability")]
        [Authorize(Roles = "Admin, Doctor")]
        public async Task<IActionResult> AddAvailability(int id, [FromBody] AddDoctorAvailabilityCommand command)
        {
            if (id != command.DoctorId)
                return BadRequest("DoctorId in route and body must match.");

            await _mediator.Send(command);
            return NoContent();
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateDoctor(int id, [FromBody] UpdateDoctorCommand command)
        {
            if (id != command.Id)
                return BadRequest("Id in route and body must match.");

            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteDoctor(int id)
        {
            await _mediator.Send(new DeleteDoctorCommand(id));
            return NoContent();
        }
    }
}
