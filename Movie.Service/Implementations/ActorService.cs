using Movie.Domain.DTOs;
using Movie.Domain.Entities;
using Movie.Domain.Interfaces;
using Movie.Service.Interfaces;

namespace Movie.Service.Implementations;

public class ActorService : IActorService
{
    private readonly IActorRepository _actorRepository;
    private readonly IUnitOfWork _unitOfWork;
    public ActorService(IActorRepository actorRepository, IUnitOfWork unitOfWork)
    {
        _actorRepository = actorRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ICollection<ActorDTO>> GetAllActorAsync()
    {
        var actors = await _actorRepository.GetAllActorsAsync();

        var actorDtos = actors.Select(a => new ActorDTO
        {
            Id = a.Id,
            FirstName = a.FirstName,
            LastName = a.LastName,
            MoviesTitles = a.Movies.Select(m => m.Title).ToList()
        }).ToList();

        return actorDtos;
    }

    public async Task<ActorDTO> GetActorByIdAsync(int id)
    {
        if(id <= 0)
        {
            throw new ArgumentException("Invalid Actor Id");
        }

        var actor = await _actorRepository.GetActorByIdAsync(id);

        var actorDto = new ActorDTO
        {
            Id = actor.Id,
            FirstName = actor.FirstName,
            LastName = actor.LastName,

            MoviesTitles = actor.Movies.Select(m => m.Title).ToList()
        };
        return actorDto;
    }

    public async Task AddActorAsync(CreateActorDTO actorDto)
    {
        if(actorDto == null) {
            throw new ArgumentException("Not found");
        }

        var actor = new Actor
        {
            FirstName = actorDto.FirstName,
            LastName = actorDto.LastName
        };

        await _actorRepository.AddActorAsync(actor);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task UpdateActorAsync(int id, UpdateActorDTO actorDTO)
    {
        if(actorDTO == null)
        {
            throw new ArgumentException("Actor was not found");
        }

        var actor = new Actor
        {
            FirstName = actorDTO.FirstName,
            LastName = actorDTO.LastName
        };

        await _actorRepository.UpdateActorAsync(id, actor);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteActorAsync(int id)
    {
        if(id <=0)
        {
            throw new ArgumentException("Invalid id");
        }
        await _actorRepository.DeleteActorAsync(id);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task UpdateActorMoviesAsync(int actorId, UpdateActorMovieDTO updateActorMovieDTO)
    {
        await _actorRepository.UpdateActorMoviesAsync(actorId, updateActorMovieDTO.MovieIds);
        await _unitOfWork.SaveChangesAsync();
    }

}
