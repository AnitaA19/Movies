using Movie.Domain.DTOs;

namespace Movie.Service.Interfaces;

public interface IMovieService
{
    Task<ICollection<MovieDTO>> GetAllMoviesAsync();
    Task AddMovieAsync(CreateMovieDTO movieDto);
    Task<MovieDTO> GetMovieByIdAsync(int id);

}
