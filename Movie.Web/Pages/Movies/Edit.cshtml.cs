using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Movie.Domain.DTOs;
using Movie.Service.Interfaces;

namespace Movie.Web.Pages.Movies
{
    public class EditModel : PageModel
    {
        private readonly IMovieService _movieService;

        public EditModel(IMovieService movieService)
        {
            _movieService = movieService;
        }

        [BindProperty]
        public UpdateMovieDto Movie { get; set; } = new();

        public int Id { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            try
            {
                var movie = await _movieService.GetMovieByIdAsync(id);
                Id = movie.Id;
                Movie = new UpdateMovieDto
                {
                    Title = movie.Title ?? string.Empty,
                    ReleaseYear = movie.ReleaseYear,
                    StudioId = 0
                };

                return Page();
            }
            catch
            {
                TempData["ErrorMessage"] = "Unable to load movie for editing. Please try again.";
                return RedirectToPage("./Index");
            }
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (!ModelState.IsValid)
            {
                Id = id;
                return Page();
            }

            try
            {
                var updatedMovie = await _movieService.UpdateMovieAsync(id, Movie);

                if (updatedMovie is null)
                {
                    TempData["ErrorMessage"] = "Movie not found.";
                    return RedirectToPage("./Index");
                }

                TempData["SuccessMessage"] = "Movie updated successfully.";
                return RedirectToPage("./Index");
            }
            catch
            {
                Id = id;
                ModelState.AddModelError(string.Empty, "Unable to update movie. Please try again.");
                return Page();
            }
        }
    }
}
