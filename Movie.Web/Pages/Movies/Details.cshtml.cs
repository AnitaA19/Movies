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

    public MovieDTO Movie { get; set; }

    public async Task OnGet(int id)
    {
        Movie = await _movieService.GetMovieByIdAsync(id);
        //return Page();
    }
}
