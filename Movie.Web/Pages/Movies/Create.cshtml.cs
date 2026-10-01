using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Movie.Domain.DTOs;
using Movie.Service.Interfaces;

namespace Movie.Web.Pages.Movies;

public class CreateModel : PageModel
{
    private readonly IMovieService _movieService;

    public CreateModel(IMovieService movieService)
    {
        _movieService = movieService;
    }

    [BindProperty]
    public CreateMovieDTO Movie { get; set; } = new CreateMovieDTO();

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        try
        {
            await _movieService.AddMovieAsync(Movie);
            TempData["SuccessMessage"] = $"{Movie.Title} was created successfully.";

            return RedirectToPage("./Index");
        }
        catch
        {
            ModelState.AddModelError(string.Empty, "Unable to create movie. Please try again.");
            return Page();
        }
    }
}
