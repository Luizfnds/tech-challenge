using MediatR;
using FCG.Domain.Entities;
using FCG.Application.Common.Models;

namespace FCG.Application.Queries.GetUserById;

public record GetUserByIdQuery(Guid Id) : IRequest<Result<User>>;
