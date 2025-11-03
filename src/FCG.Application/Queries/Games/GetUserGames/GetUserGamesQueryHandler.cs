using FCG.Application.Common.Errors;
using FCG.Application.Common.Models;
using FCG.Application.Contracts.Repositories;
using FCG.Domain.Entities;
using MediatR;

namespace FCG.Application.Queries.Games.GetUserGames;

public class GetUserGamesQueryHandler : IRequestHandler<GetUserGamesQuery, Result<PagedResult<UserGame>>>
{
    private readonly IGameRepository _gameRepository;
    private readonly IUserRepository _userRepository;

    public GetUserGamesQueryHandler(
        IGameRepository gameRepository,
        IUserRepository userRepository)
    {
        _gameRepository = gameRepository;
        _userRepository = userRepository;
    }

    public async Task<Result<PagedResult<UserGame>>> Handle(GetUserGamesQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);
        if (user is null)
            return Result.Failure<PagedResult<UserGame>>(DomainErrors.User.NotFound(request.UserId));

        var pagedResult = await _gameRepository.GetUserGamesPagedAsync(
            request.UserId,
            request.PageNumber,
            request.PageSize,
            cancellationToken);

        return Result.Success(pagedResult);
    }
}
