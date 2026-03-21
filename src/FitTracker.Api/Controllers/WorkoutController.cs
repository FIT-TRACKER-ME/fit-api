using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FitTracker.Api.Abstractions;
using FitTracker.Application.Services.Workouts.Create;
using FitTracker.Application.Services.Workouts.GetByStudent;
using FitTracker.Application.Services.Workouts.GetByPersonal;
using FitTracker.Application.Services.Workouts.Execute;
using FitTracker.Application.Services.Workouts.Update;
using FitTracker.Application.Services.Workouts.Delete;
using FitTracker.Application.Services.Workouts.GetExpirationAlerts;
using FitTracker.Domain.Shared;

namespace FitTracker.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class WorkoutController : ApiController
    {
        public WorkoutController(ISender sender) : base(sender)
        {
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
        {
            var workout = await Sender.Send(new GetWorkoutByIdQuery(id), cancellationToken);

            if (workout.IsFailure)
            {
                return HandleFailure(workout);
            }

            return Ok(workout.Value);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateWorkoutCommand command, CancellationToken cancellationToken)
        {
            Result<Guid> result = await Sender.Send(command, cancellationToken);

            if (result.IsFailure)
            {
                return HandleFailure(result);
            }

            return CreatedAtAction(nameof(GetByStudent), new { studentId = command.StudentId }, result.Value);
        }

        [HttpPost("execute")]
        public async Task<IActionResult> Execute([FromBody] ExecuteWorkoutCommand command, CancellationToken cancellationToken)
        {
            Result result = await Sender.Send(command, cancellationToken);

            if (result.IsFailure)
            {
                return HandleFailure(result);
            }

            return NoContent();
        }

        [HttpGet("student/{studentId:guid}")]
        public async Task<IActionResult> GetByStudent(Guid studentId, CancellationToken cancellationToken)
        {
            var query = new GetStudentWorkoutsQuery(studentId);

            Result<List<WorkoutResponse>> result = await Sender.Send(query, cancellationToken);

            if (result.IsFailure)
            {
                return HandleFailure(result);
            }

            return Ok(result.Value);
        }

        [HttpGet("personal")]
        public async Task<IActionResult> GetByPersonal(CancellationToken cancellationToken)
        {
            var query = new GetPersonalWorkoutsQuery();

            Result<List<WorkoutResponse>> result = await Sender.Send(query, cancellationToken);

            if (result.IsFailure)
            {
                return HandleFailure(result);
            }

            return Ok(result.Value);
        }

        [HttpGet("personal/expiration-alerts")]
        public async Task<IActionResult> GetExpirationAlerts(CancellationToken cancellationToken)
        {
            var query = new GetWorkoutExpirationAlertsQuery();
            var result = await Sender.Send(query, cancellationToken);
            return Ok(result);
        }

        [Authorize(Roles = "Personal")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateWorkoutCommand command, CancellationToken cancellationToken)
        {
            if (id != command.WorkoutId)
            {
                return BadRequest("ID mismatch");
            }

            var result = await Sender.Send(command, cancellationToken);
            return result.IsSuccess ? NoContent() : HandleFailure(result);
        }

        [Authorize(Roles = "Personal")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            var command = new DeleteWorkoutCommand(id);
            var result = await Sender.Send(command, cancellationToken);
            return result.IsSuccess ? NoContent() : HandleFailure(result);
        }
    }
}
