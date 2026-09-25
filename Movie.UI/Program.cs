using Microsoft.Extensions.DependencyInjection;
using Movie.Domain.DTOs;
using Movie.Domain.Entities;
using Movie.Domain.Interfaces;
using Movie.Infrastucture.Data;
using Movie.Infrastucture.Repositories;
using Movie.Service.Implementations;
using Movie.Service.Interfaces;

namespace Movie.UI;

public class Program
{
    static async Task Main(string[] args)
    {
        //var dbContext = new MovieDbContext();

        //var movieRepository = new MovieRepository(dbContext);
        //var movieService = new MovieService(movieRepository);

        //var country = new Country
        //{
        //    Name = "USA"
        //};

        //dbContext.Countries.Add(country);
        //await dbContext.SaveChangesAsync();

        //var studio = new Studio
        //{
        //    Name = "Marvel",
        //    CountryId = country.Id
        //};

        //dbContext.Studios.Add(studio);
        //await dbContext.SaveChangesAsync();

        //var createMovieDto = new CreateMovieDTO
        //{
        //    Title = "Spiderman",
        //    ReleaseYear = 2026,
        //    StudioId = studio.Id
        //};

        //await movieService.AddMovieAsync(createMovieDto);

        //await dbContext.SaveChangesAsync();

        var services = new ServiceCollection();
        services.AddDbContext<MovieDbContext>();
        services.AddScoped<IMovieRepository, MovieRepository>();
        services.AddScoped<IMovieService, MovieService>();

        var serviceProvider = services.BuildServiceProvider();
        var movieService = serviceProvider.GetRequiredService<MovieService>();
    }
}