
using System.Collections.Generic;

namespace Movie.Domain.Interfaces;

public interface IMovieRepository
{
    Task<ICollection<Movie.Domain.Entities.Movie>> GetAllMoviesAsync();
    Task<ICollection<Movie.Domain.Entities.Movie>> GetLatestMoviesAsync(int count);
    Task<ICollection<Movie.Domain.Entities.Movie>> SearchMoviesAsync(string? searchTerm);
    Task AddMovieAsync(Movie.Domain.Entities.Movie movie);
    Task<Movie.Domain.Entities.Movie> GetMovieByIdAsync(int id);
    Task<Movie.Domain.Entities.Movie> DeleteMovieAsync(int id);
    Task UpdateMovieAsync(Movie.Domain.Entities.Movie movie);
    Task<ICollection<Domain.Entities.Movie>> SearchMoviesByStudioAsync(int year, string studioName, int minimumActorCount);
    Task<ICollection<Domain.Entities.Movie>> SearchMoviesByCountryAsync(
      string countryName,
      int minimumYear,
      int maximumActorCount);
    Task<ICollection<Domain.Entities.Movie>> SearchMoviesAdvancedAsync(
    int fromYear,
    int toYear,
    string countryName,
    string titleText,
    int minimumActorCount);
}
