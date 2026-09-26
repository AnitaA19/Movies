using Movie.Domain.Entities;

namespace Movie.Domain.Interfaces;

public interface IActorRepository
{
    Task AddActorAsync(Actor actor);
    Task<ICollection<Actor>> GetAllActorsAsync();
    Task<Actor> GetActorByIdAsync(int id);
    Task UpdateActorAsync(int id, Actor actor);
    Task DeleteActorAsync(int id);
    Task UpdateActorMoviesAsync(int actorId, ICollection<int> movieIds);
}
