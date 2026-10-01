using Movie.Domain.Interfaces;
using Movie.Service.Interfaces;
using Movie.Domain.DTOs;

namespace Movie.Service.Implementations;

public class MovieService : IMovieService
{
    private readonly IMovieRepository _movieRepository;
    private readonly IUnitOfWork _unitOfWork;
    public MovieService(IMovieRepository movieRepository, IUnitOfWork unitOfWork)
    {
        _movieRepository = movieRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ICollection<MovieDTO>> GetAllMoviesAsync()
    {
        var movies = await _movieRepository.GetAllMoviesAsync();
        var movieDto = movies.Select(m => new Movie.Domain.DTOs.MovieDTO
        {
            Id = m.Id,
            Title = m.Title,
            ReleaseYear = m.ReleaseYear,
            StudioName = m.Studio?.Name ?? string.Empty
        }).ToList();
        return movieDto;
    }

    public async Task<ICollection<MovieDTO>> SearchMoviesAsync(string? searchTerm)
    {
        var movies = await _movieRepository.SearchMoviesAsync(searchTerm);
        return movies.Select(m => new MovieDTO
        {
            Id = m.Id,
            Title = m.Title,
            ReleaseYear = m.ReleaseYear,
            StudioName = m.Studio?.Name ?? string.Empty
        }).ToList();
    }

    public async Task AddMovieAsync(CreateMovieDTO movieDto)
    {
        if (movieDto == null)
        {
            throw new ArgumentNullException(nameof(movieDto));
        }

        if (string.IsNullOrWhiteSpace(movieDto.Title))
        {
            throw new ArgumentException("Movie title cannot be null or empty.", nameof(movieDto.Title));
        }

        if (movieDto.ReleaseYear < 1888 || movieDto.ReleaseYear > DateTime.Now.Year)
        {
            throw new ArgumentOutOfRangeException(nameof(movieDto.ReleaseYear), "Release year must be between 1888 and the current year.");
        }


        if (movieDto.Title.Length > 200)
        {
            throw new ArgumentException("Movie title cannot exceed 200 characters.", nameof(movieDto.Title));
        }

        var movie = new Movie.Domain.Entities.Movie
        {
            Title = movieDto.Title,
            ReleaseYear = movieDto.ReleaseYear,
            StudioId = movieDto.StudioId
        };
        await _movieRepository.AddMovieAsync(movie);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<MovieDTO> GetMovieByIdAsync(int id)
    {
        var movie = await _movieRepository.GetMovieByIdAsync(id);

        if (movie == null)
        {
            throw new Exception("Movie was not found.");
        }

        return new MovieDTO
        {
            Id = movie.Id,
            Title = movie.Title,
            ReleaseYear = movie.ReleaseYear,
            StudioName = movie.Studio?.Name ?? string.Empty
        };
    }

    public async Task<MovieDTO> DeleteMovieAsync(int id)
    {
        var movie = await _movieRepository.DeleteMovieAsync(id);
    
           
        if (movie == null)
            return null;

        await _unitOfWork.SaveChangesAsync();

        return new MovieDTO
        {
            Id = movie.Id,
            Title = movie.Title,
            ReleaseYear = movie.ReleaseYear,
            StudioName = movie.Studio?.Name ?? string.Empty
        };
    }

    public async Task<MovieDTO> UpdateMovieAsync(int id, UpdateMovieDto movieDto)
    {
        var movie = await _movieRepository.GetMovieByIdAsync(id);

        if (movie == null)
            return null;

        movie.Title = movieDto.Title;
        movie.ReleaseYear = movieDto.ReleaseYear;
        movie.StudioId = movieDto.StudioId;

        await _movieRepository.UpdateMovieAsync(movie);
        await _unitOfWork.SaveChangesAsync();


        return new MovieDTO
        {
            Id = movie.Id,
            Title = movie.Title,
            ReleaseYear = movie.ReleaseYear,
            StudioName = movie.Studio?.Name ?? string.Empty
        };
    }
}
