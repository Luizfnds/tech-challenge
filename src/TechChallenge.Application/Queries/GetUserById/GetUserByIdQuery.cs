using MediatR;
using TechChallenge.Domain.Entities;

namespace TechChallenge.Application.Queries.GetUserById;

public record GetUserByIdQuery(Guid Id) : IRequest<User?>;
