using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Movie.Domain.DTOs;
using Movie.Service.Interfaces;

namespace Movie.Web.Pages.Movies;

public class IndexModel : PageModel
{
    private readonly IMovieService _movieService;

    public ICollection<MovieDTO> Movies { get; set; } = new List<MovieDTO>();

    public string? SuccessMessage { get; private set; }
    public string? ErrorMessage { get; private set; }

    public IndexModel(IMovieService movieService)
    {
        _movieService = movieService;
    }

    public async Task OnGetAsync()
    {
        SuccessMessage = TempData["SuccessMessage"] as string;
        ErrorMessage = TempData["ErrorMessage"] as string;

        try
        {
            Movies = await _movieService.GetAllMoviesAsync();
        }
        catch
        {
            ErrorMessage ??= "Unable to load movies. Please try again.";
            Movies = new List<MovieDTO>();
        }
    }

    public async Task<IActionResult> OnPostDeleteAsync(int id)
    {
        try
        {
            await _movieService.DeleteMovieAsync(id);
            TempData["SuccessMessage"] = "Movie deleted successfully.";
        }
        catch
        {
            TempData["ErrorMessage"] = "Unable to delete movie. Please try again.";
        }

        return RedirectToPage();
    }
}
