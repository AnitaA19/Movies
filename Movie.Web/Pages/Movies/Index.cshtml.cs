using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Movie.Domain.DTOs;
using Movie.Service.Interfaces;

namespace Movie.Web.Pages.Movies;

public class IndexModel : PageModel
{
    private readonly IMovieService _movieService;

    public ICollection<MovieDTO> Movies { get; set; } = new List<MovieDTO>();

    [BindProperty(SupportsGet = true)]
    public string? SearchTerm { get; set; }

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

        await LoadMoviesAsync();
    }

    public async Task OnGetResetAsync()
    {
        SuccessMessage = TempData["SuccessMessage"] as string;
        ErrorMessage = TempData["ErrorMessage"] as string;
        SearchTerm = null;

        await LoadMoviesAsync();
    }

    private async Task LoadMoviesAsync()
    {
        try
        {
            Movies = string.IsNullOrWhiteSpace(SearchTerm)
                ? await _movieService.GetAllMoviesAsync()
                : await _movieService.SearchMoviesAsync(SearchTerm);
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
