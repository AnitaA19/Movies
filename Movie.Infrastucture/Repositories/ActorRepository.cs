using Microsoft.EntityFrameworkCore;
using Movie.Domain.Entities;
using Movie.Domain.Interfaces;
using Movie.Infrastucture.Data;

namespace Movie.Infrastucture.Repositories;

public class ActorRepository : IActorRepository
{
    private readonly MovieDbContext _context;
    public ActorRepository(MovieDbContext context)
    {
        _context = context;
    }

    public async Task AddActorAsync(Actor actor)
    {
        await _context.Actors.AddAsync(actor);
    }

    public async Task<ICollection<Actor>> GetAllActorsAsync()
    {
        return await _context.Actors
            .Include(m => m.Movies)
            .ToListAsync();
    }

    public async Task<Actor> GetActorAsync(int id)
    {
        return await _context.Actors
            .Include(a => a.Movies)
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task UpdateActorAsync(int id, Actor actor)
    {
        var actorExists = await _context.Actors.FirstOrDefaultAsync(a => a.Id == id);
        if(actorExists == null)
        {
            throw new Exception("Actor not found");
        }

        actorExists.FirstName = actor.FirstName;
        actorExists.LastName = actor.LastName;
    }

    public async Task DeleteActorAsync(int id)
    {
        var actorExists = await _context.Actors.FirstOrDefaultAsync(a=>a.Id == id);
        if(actorExists == null)
        {
            throw new ArgumentException("Actor not found");
        }
        _context.Actors.Remove(actorExists);
    }

    public async Task UpdateActorMovieAsync(int actorId, ICollection<int> movieIds)
    {
        var actor = await _context.Actors.Include(a => a.Movies).FirstOrDefaultAsync(a => a.Id == actorId);
        if(actor == null)
        {
            throw new ArgumentException("Actor not found");
        }

        var movies = await _context.Movies.Where(m => movieIds.Contains(m.Id)).ToListAsync();

        actor.Movies.Clear();
        foreach(var movie in movies)
        {
            actor.Movies.Add(movie);
        }

    }
}
