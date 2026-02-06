using HealthDesk.Application.Features.Prescriptions.Commands.CreatePrescription;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthDesk.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PrescriptionsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public PrescriptionsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("by-patient")]
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> GetPrescriptionsByPatient()
        {
            var result = await _mediator.Send(new HealthDesk.Application.Features.Prescriptions.Queries.GetPrescriptionByPatientQuery());
            return Ok(result);
        }

        [HttpGet("by-doctor")]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> GetPrescriptionsByDoctor()
        {
            var result = await _mediator.Send(new HealthDesk.Application.Features.Prescriptions.Queries.GetPrescriptionByDoctorQuery());
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> CreatePrescription([FromBody] CreatePrescriptionCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
