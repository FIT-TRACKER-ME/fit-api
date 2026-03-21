using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FitTracker.Api.Abstractions;
using FitTracker.Application.Services.Anamnesis.Create;
using FitTracker.Application.Services.Anamnesis.GetByPersonal;
using FitTracker.Application.Services.Anamnesis.Deactivate;
using FitTracker.Application.Services.Anamnesis.GetResponsesByStudent;
using FitTracker.Application.Services.Anamnesis.GetFormById;
using FitTracker.Application.Services.Anamnesis.SubmitResponse;
using FitTracker.Domain.Shared;

namespace FitTracker.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class AnamnesisController : ApiController
    {
        public AnamnesisController(ISender sender) : base(sender)
        {
        }

        [HttpPost("form")]
        public async Task<IActionResult> CreateForm([FromBody] CreateAnamnesisFormCommand command, CancellationToken cancellationToken)
        {
            var result = await Sender.Send(command, cancellationToken);
            
            return result.IsSuccess ? Ok(result.Value) : HandleFailure(result);
        }

        [HttpGet("form/{id:guid}")]
        public async Task<IActionResult> GetFormById(Guid id, CancellationToken cancellationToken)
        {
            var query = new GetAnamnesisFormByIdQuery { Id = id };
            var result = await Sender.Send(query, cancellationToken);
            
            return result.IsSuccess ? Ok(result.Value) : HandleFailure(result);
        }

        [HttpGet("forms/personal")]
        public async Task<IActionResult> GetFormsByPersonal(CancellationToken cancellationToken)
        {
            var query = new GetAnamnesisFormsByPersonalQuery();
            var result = await Sender.Send(query, cancellationToken);
            
            return result.IsSuccess ? Ok(result.Value) : HandleFailure(result);
        }

        [HttpPost("response")]
        public async Task<IActionResult> SubmitResponse([FromBody] SubmitAnamnesisResponseCommand command, CancellationToken cancellationToken)
        {
            var result = await Sender.Send(command, cancellationToken);
            
            return result.IsSuccess ? Ok() : HandleFailure(result);
        }

        // Endpoint removed to avoid duplication with responses/student/{studentId} below

        [Authorize(Roles = "Personal")]
        [HttpDelete("form/{id:guid}")]
        public async Task<IActionResult> DeleteForm(Guid id, CancellationToken cancellationToken)
        {
            var command = new DeactivateAnamnesisFormCommand(id);
            var result = await Sender.Send(command, cancellationToken);
            return result.IsSuccess ? Ok() : HandleFailure(result);
        }

        [Authorize(Roles = "Personal")]
        [HttpGet("responses/student/{studentId}")]
        public async Task<IActionResult> GetResponsesByStudent(Guid studentId, CancellationToken cancellationToken)
        {
            var query = new GetAnamnesisResponseByStudentQuery(studentId);
            var result = await Sender.Send(query, cancellationToken);
            return result.IsSuccess ? Ok(result.Value) : HandleFailure(result);
        }
    }
}
