using MediatR;
using TechChallenge.Domain.Entities;
using TechChallenge.Application.Common.Models;

namespace TechChallenge.Application.Queries.GetUserById;

public record GetUserByIdQuery(Guid Id) : IRequest<Result<User>>;
