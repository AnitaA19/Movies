using Microsoft.AspNetCore.Mvc;
using Movie.Service.Interfaces;

namespace Movie.Web.ViewComponents;

public class LatestMoviesViewComponent : ViewComponent
{
    private readonly IMovieService _movieService;

    public LatestMoviesViewComponent(IMovieService movieService)
    {
        _movieService = movieService;
    }

    public async Task<IViewComponentResult> InvokeAsync(int count)
    {
        try
        {
            var latestMovies = await _movieService.GetLatestMoviesAsync(count);
            return View(latestMovies);
        }
        catch
        {
            return View(Array.Empty<Movie.Domain.DTOs.MovieDTO>());
        }
    }
}
