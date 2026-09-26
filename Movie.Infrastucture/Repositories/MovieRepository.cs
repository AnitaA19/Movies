using Movie.Infrastucture.Data;
using Microsoft.EntityFrameworkCore;
using Movie.Domain.Interfaces;
// Avoid importing Movie as a simple identifier to prevent conflict with namespace 'Movie'
// Use fully-qualified type names below to disambiguate


namespace Movie.Infrastucture.Repositories;

public class MovieRepository : IMovieRepository
{
    private readonly MovieDbContext _context;
    public MovieRepository(MovieDbContext context)
    {
        _context = context;
    }

    public async Task<ICollection<Movie.Domain.Entities.Movie>> GetAllMoviesAsync()
    {
        return await _context.Movies.Include(s => s.Studio).ToListAsync();
    }

    public async Task AddMovieAsync(Movie.Domain.Entities.Movie movie)
    {
        await _context.Movies.AddAsync(movie);
        //await _context.SaveChangesAsync();
    }

    public async Task<Movie.Domain.Entities.Movie> GetMovieByIdAsync(int id)
    {
        return await _context.Movies
            .Include(m => m.Studio)
            .FirstOrDefaultAsync(m => m.Id == id);
    }

    public async Task<Movie.Domain.Entities.Movie> DeleteMovieAsync(int id)
    {
        var movie = await _context.Movies.FindAsync(id);

        if (movie == null)
            return null;

        _context.Movies.Remove(movie);
        //await _context.SaveChangesAsync();

        return movie;
    }

    public async Task UpdateMovieAsync(Movie.Domain.Entities.Movie movie)
    {
        _context.Movies.Update(movie);
        //await _context.SaveChangesAsync();
    }

    public async Task<ICollection<Domain.Entities.Movie>> SearchMovieAsync(int releaseYear, string countryName, int actorCount)
    {
        return await _context.Movies
    .Where(m => m.ReleaseYear > releaseYear)
    .Where(m => m.Studio.Country.Name == countryName)
    .Where(m => m.Actors.Count == actorCount)
    .OrderBy(m => m.Actors.Count)
    .ToListAsync();
    }
}
