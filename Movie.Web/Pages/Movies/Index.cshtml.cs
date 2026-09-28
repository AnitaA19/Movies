using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Movie.Domain.DTOs;
using Movie.Domain.Interfaces;
using Movie.Service;
using Movie.Service.Interfaces;

namespace Movie.Web.Pages.Movies;

public class IndexModel : PageModel
{
    //public List<string> Movies = new List<string>();

    private readonly IMovieService _movieService;
    public ICollection<MovieDTO> Movies { get; set; } = new List<MovieDTO>();

    public IndexModel(IMovieService movieService)
    {
        _movieService = movieService;
    }

    public async Task OnGetAsync()
    {
        Movies = await _movieService.GetAllMoviesAsync();
    }

    //public void OnGet()
    //{
    //    //Movies.Add("Matrix");
    //    //Movies.Add("Interstellar");
    //    //Movies.Add("Home Alone");

    //}
}
