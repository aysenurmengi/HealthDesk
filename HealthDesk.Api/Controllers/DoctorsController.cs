using HealthDesk.Application.Features.Doctors.Commands.CreateDoctor;
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

        // TASk-API: Doctor - Implement Update and Delete endpoints
    }
}