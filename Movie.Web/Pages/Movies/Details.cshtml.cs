using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Movie.Domain.DTOs;
using Movie.Service.Interfaces;

namespace Movie.Web.Pages.Movies;

public class DetailsModel : PageModel
{
    private readonly IMovieService _movieService;
    public DetailsModel(IMovieService movieService)
    {
        _movieService = movieService;
    }

    public MovieDTO? Movie { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        try
        {
            Movie = await _movieService.GetMovieByIdAsync(id);

            if (Movie is null)
            {
                TempData["ErrorMessage"] = "Movie not found.";
                return RedirectToPage("./Index");
            }

            return Page();
        }
        catch
        {
            TempData["ErrorMessage"] = "Unable to load movie details. Please try again.";
            return RedirectToPage("./Index");
        }
    }
}
