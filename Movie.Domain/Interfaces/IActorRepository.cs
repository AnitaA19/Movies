using Movie.Domain.Entities;

namespace Movie.Domain.Interfaces;

public interface IActorRepository
{
    Task AddActorAsync(Actor actor);
    Task<ICollection<Actor>> GetAllActorsAsync();
    Task<Actor> GetActorAsync(int id);
    Task UpdateActorAsync(int id, Actor actor);
    Task DeleteActorAsync(int id);
    Task UpdateActorMovieAsync(int actorId, ICollection<int> movieIds);
}
