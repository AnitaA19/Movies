namespace Movie.Domain.DTOs;

public class UpdateMovieDto
{
    public string Title { get; set; }
    public int ReleaseYear { get; set; }
    public int StudioId { get; set; }
}