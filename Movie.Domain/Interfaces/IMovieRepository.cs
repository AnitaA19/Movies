
using System.Collections.Generic;

namespace Movie.Domain.Interfaces;

public interface IMovieRepository
{
    Task<ICollection<Movie.Domain.Entities.Movie>> GetAllMoviesAsync();
    Task AddMovieAsync(Movie.Domain.Entities.Movie movie);
    Task<Movie.Domain.Entities.Movie> GetMovieByIdAsync(int id);
    Task<Movie.Domain.Entities.Movie> DeleteMovieAsync(int id);
    Task UpdateMovieAsync(Movie.Domain.Entities.Movie movie);
}
