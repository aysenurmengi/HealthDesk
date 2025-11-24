using HealthDesk.Application.Features.Appointments.Commands.CancelAppointment;
using HealthDesk.Application.Features.Appointments.Commands.CreateAppointment;
using HealthDesk.Application.Features.Appointments.Commands.UpdateAppointment;
using HealthDesk.Application.Features.Appointments.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthDesk.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AppointmentsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public AppointmentsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllAppointments()
        {
            var result = await _mediator.Send(new GetAllAppointmentsQuery());
            return Ok(result);
        }  

        [HttpPost]
        [Authorize(Roles = "Doctor, Patient")]
        public async Task<IActionResult> CreateAppointment([FromBody] CreateAppointmentCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete("{appointmentId:int}")]  
        [Authorize(Roles = "Doctor, User")]
        public async Task<IActionResult> CancelAppointment(int appointmentId)
        {
            await _mediator.Send(new CancelAppointmentCommand(appointmentId));
            return NoContent();
        }

        [HttpPut("{appointmentId:int}")]
        [Authorize(Roles = "Doctor, Patient")]
        public async Task<IActionResult> UpdateAppointment(int appointmentId, [FromBody] UpdateAppointmentCommand command)
        {
            if (appointmentId != command.AppointmentId)
            {
                return BadRequest("AppointmentId in route and body must match.");
            }

            var result = await _mediator.Send(command);
            return Ok(result);
        }

    }
}