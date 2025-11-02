using MediatR;
using FCG.Domain.Entities;
using FCG.Application.Contracts.Repositories;
using FCG.Application.Common.Models;
using FCG.Application.Common.Errors;

namespace FCG.Application.Queries.GetUserById;

public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, Result<User>>
{
    private readonly IUserRepository _userRepository;

    public GetUserByIdQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Result<User>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.Id);

        if (user is null)
            return Result.Failure<User>(DomainErrors.User.NotFound(request.Id));

        return Result.Success(user);
    }
}
