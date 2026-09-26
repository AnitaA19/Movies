using Microsoft.EntityFrameworkCore;
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
        var services = new ServiceCollection();

        services.AddDbContext<MovieDbContext>();

        services.AddScoped<IMovieRepository, MovieRepository>();
        services.AddScoped<IMovieService, MovieService>();

        services.AddScoped<IActorRepository, ActorRepository>();
        services.AddScoped<IActorService, ActorService>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        var serviceProvider = services.BuildServiceProvider();

        var dbContext = serviceProvider.GetRequiredService<MovieDbContext>();

        var movieService = serviceProvider.GetRequiredService<IMovieService>();
        var actorService = serviceProvider.GetRequiredService<IActorService>();

        var studio = new Studio
        {
            Name = "Warner Bros",
            CountryId = 1
        };

        dbContext.Studios.Add(studio);
        await dbContext.SaveChangesAsync();

        var st = await dbContext.Studios
            .FirstOrDefaultAsync(s => s.Name == "Warner Bros");

        var movieDto = new CreateMovieDTO
        {
            Title = "Home Alone 1",
            ReleaseYear = 1996,
            StudioId = st.Id
        };

        var movie2Dto = new CreateMovieDTO
        {
            Title = "Spider Man",
            ReleaseYear = 2026,
            StudioId = st.Id
        };

        await movieService.AddMovieAsync(movieDto);
        await movieService.AddMovieAsync(movie2Dto);

        var actorDTO = new CreateActorDTO
        {
            FirstName = "Tom",
            LastName = "Holland"
        };

        await actorService.AddActorAsync(actorDTO);

        var actor = await dbContext.Actors
            .FirstAsync(a =>
                a.FirstName == "Tom" &&
                a.LastName == "Holland");

        var film1 = await dbContext.Movies
            .FirstAsync(m => m.Title == "Home Alone 1");

        var film2 = await dbContext.Movies
            .FirstAsync(m => m.Title == "Spider Man");

        var updateActorMovieDTO = new UpdateActorMovieDTO
        {
            MovieIds = new List<int>
            {
                film1.Id,
                film2.Id
            }
        };

        await actorService.UpdateActorMoviesAsync(
            actor.Id,
            updateActorMovieDTO);

        var actorWithMovies = await dbContext.Actors.Include(a => a.Movies).ToListAsync();

        foreach (var actorMovie in actorWithMovies)
        {
            Console.WriteLine($"{actorMovie.FirstName} {actorMovie.LastName}");

            foreach (var movieOfActor in actorMovie.Movies)
            {
                Console.WriteLine($"- {movieOfActor.Title}");
            }

            Console.WriteLine();
        }

    }
}