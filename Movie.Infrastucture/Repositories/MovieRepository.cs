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

    public async Task<ICollection<Movie.Domain.Entities.Movie>> GetLatestMoviesAsync(int count)
    {
        if (count <= 0)
        {
            return new List<Movie.Domain.Entities.Movie>();
        }

        return await _context.Movies
            .AsNoTracking()
            .Include(m => m.Studio)
            .OrderByDescending(m => m.ReleaseYear)
            .ThenByDescending(m => m.Id)
            .Take(count)
            .ToListAsync();
    }

    public async Task<ICollection<Movie.Domain.Entities.Movie>> SearchMoviesAsync(string? searchTerm)
    {
        var query = _context.Movies.Include(m => m.Studio).AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            var term = searchTerm.Trim();
            query = query.Where(m =>
                m.Title.Contains(term) ||
                m.Studio.Name.Contains(term));
        }

        return await query
            .OrderBy(m => m.Title)
            .ToListAsync();
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
        var movie = await _context.Movies
            .Include(m => m.Studio)
            .FirstOrDefaultAsync(m => m.Id == id);

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

    public async Task<ICollection<Domain.Entities.Movie>> SearchMoviesByStudioAsync(
      int year,
      string studioName,
      int minimumActorCount)
    {
        return await _context.Movies
            .Include(m => m.Studio)
                .ThenInclude(s => s.Country)
            .Include(m => m.Actors)
            .Where(m =>
                m.ReleaseYear >= year &&
                m.Studio.Name == studioName &&
                m.Actors.Count >= minimumActorCount)
            .OrderByDescending(m => m.ReleaseYear)
            .ThenBy(m => m.Title)
            .ToListAsync();
    }

    public async Task<ICollection<Domain.Entities.Movie>> SearchMoviesByCountryAsync(
        string countryName,
        int minimumYear,
        int maximumActorCount)
    {
        return await _context.Movies
            .Include(m => m.Studio)
                .ThenInclude(s => s.Country)
            .Include(m => m.Actors)
            .Where(m =>
                m.Studio.Country.Name == countryName &&
                m.ReleaseYear >= minimumYear &&
                m.Actors.Count <= maximumActorCount)
            .OrderBy(m => m.Actors.Count)
            .ThenByDescending(m => m.ReleaseYear)
            .ThenBy(m => m.Title)
            .ToListAsync();
    }

    public async Task<ICollection<Domain.Entities.Movie>> SearchMoviesAdvancedAsync(
        int fromYear,
        int toYear,
        string countryName,
        string titleText,
        int minimumActorCount)
    {
        return await _context.Movies
            .Include(m => m.Studio)
                .ThenInclude(s => s.Country)
            .Include(m => m.Actors)
            .Where(m =>
                m.ReleaseYear >= fromYear &&
                m.ReleaseYear <= toYear &&
                m.Studio.Country.Name == countryName &&
                m.Title.Contains(titleText) &&
                m.Actors.Count >= minimumActorCount)
            .OrderByDescending(m => m.Actors.Count)
            .ThenByDescending(m => m.ReleaseYear)
            .ThenBy(m => m.Studio.Name)
            .ThenBy(m => m.Title)
            .ToListAsync();
    }

}
