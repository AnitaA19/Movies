namespace Movie.Infrastucture.Data;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Movie.Domain.Entities;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using static System.Net.Mime.MediaTypeNames;

public class MovieDbContext : DbContext
{
    public DbSet<Movie> Movies { get; set; }
    public DbSet<Actor> Actors { get; set; }
    public DbSet<Studio> Studios { get; set; }
    public DbSet<Country> Countries { get; set; }
    public DbSet<StudioDetails> StudioDetails { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        var _connectionString = configuration.GetConnectionString("DefaultConnection");
        optionsBuilder.UseSqlServer(_connectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Movie>()
            .HasMany(m => m.Actors)
            .WithMany(a => a.Movies)
            ;

        modelBuilder.Entity<Movie>()
            .Property(m => m.Title)
            .IsRequired()
            .HasMaxLength(100);


        modelBuilder.Entity<Studio>()
            .HasOne(s => s.StudioDetails)
            .WithOne(sd => sd.Studio)
            .HasForeignKey<StudioDetails>(sd => sd.StudioId);


        modelBuilder.Entity<Studio>().Property(s => s.Name)
            .IsRequired()
            .HasMaxLength(100);

        modelBuilder.Entity<Studio>()
            .HasMany(s => s.Movies)
            .WithOne(m => m.Studio)
            .HasForeignKey(m => m.StudioId);

        modelBuilder.Entity<Country>()
            .HasMany(c => c.Studios)
            .WithOne(s => s.Country)
            .HasForeignKey(s => s.CountryId);

        modelBuilder.Entity<Actor>()
            .HasMany(a => a.Movies)
            .WithMany(m => m.Actors);

        modelBuilder.Entity<Actor>()
            .Property(a => a.FirstName)
            .IsRequired()
            .HasMaxLength(100);

        // data seeding
        //modelBuilder.Entity<Country>()
        //    .HasData(
        //    new Country { Id = 1, Name = "USA" },
        //    new Country { Id = 2, Name = "UK" },
        //    new Country { Id = 3, Name = "Canada" },
        //    )

    }
}
