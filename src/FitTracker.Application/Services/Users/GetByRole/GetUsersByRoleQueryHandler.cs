using FitTracker.Application.Abstractions.Messaging;
using FitTracker.Application.Services.Users.Login;
using FitTracker.Domain.Enums;
using FitTracker.Domain.Repositories;
using FitTracker.Domain.Shared;

namespace FitTracker.Application.Services.Users.GetByRole
{
    public sealed class GetUsersByRoleQueryHandler : IQueryHandler<GetUsersByRoleQuery, List<UserResponse>>
    {
        private readonly IUserRepository _userRepository;
        private readonly FitTracker.Application.Abstractions.IUserContext _userContext;

        public GetUsersByRoleQueryHandler(IUserRepository userRepository, FitTracker.Application.Abstractions.IUserContext userContext)
        {
            _userRepository = userRepository;
            _userContext = userContext;
        }

        public async Task<Result<List<UserResponse>>> Handle(GetUsersByRoleQuery request, CancellationToken cancellationToken)
        {
            IEnumerable<FitTracker.Domain.Entities.Users.User> users;

            if (request.Role == (int)UserRole.Student && _userContext.Role == UserRole.Personal)
            {
                users = await _userRepository.GetStudentsByPersonalIdAsync(new FitTracker.Domain.Entities.Users.UserId(_userContext.UserId), cancellationToken);
            }
            else
            {
                users = await _userRepository.GetByRoleAsync((UserRole)request.Role, cancellationToken);
            }

            var response = users.Select(u => new UserResponse(
                u.Name,
                u.Id.Value.ToString(),
                u.Document?.Value ?? string.Empty,
                u.Email.Value,
                u.Phone,
                (int)u.Role,
                (int)u.Status
            )).ToList();

            return response;
        }
    }
}
