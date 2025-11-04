using MediatR;
using FCG.Domain.Entities;
using FCG.Application.Contracts.Repositories;
using FCG.Application.Common.Models;
using FCG.Application.Common.Errors;

namespace FCG.Application.Queries.Users.GetUserByEmail;

public class GetUserByEmailQueryHandler(IUserRepository userRepository) : IRequestHandler<GetUserByEmailQuery, Result<User>>
{
    private readonly IUserRepository _userRepository = userRepository;

    public async Task<Result<User>> Handle(GetUserByEmailQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);

        if (user is null)
            return Result.Failure<User>(ApplicationErrors.User.NotFound(request.Email));

        return Result.Success(user);
    }
}
