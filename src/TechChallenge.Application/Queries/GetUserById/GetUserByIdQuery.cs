using MediatR;
using TechChallenge.Domain.Entities;

namespace TechChallenge.Application.Queries.GetUserById;

// TODO: Trocar para record
public class GetUserByIdQuery : IRequest<User?>
{
    public Guid Id { get; set; }

    public GetUserByIdQuery(Guid id)
    {
        Id = id;
    }
}
