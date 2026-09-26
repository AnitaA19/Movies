using Movie.Domain.DTOs;

namespace Movie.Service.Interfaces;

public interface IActorService
{
    Task<ICollection<ActorDTO>> GetAllActorAsync();
    Task<ActorDTO> GetActorByIdAsync(int id);
    Task AddActorAsync(CreateActorDTO actorDto);
    Task UpdateActorAsync(int id, UpdateActorDTO actorDTO);
    Task DeleteActorAsync(int id);
    Task UpdateActorMoviesAsync(int actorId, UpdateActorMovieDTO updateActorMovieDTO);
}
