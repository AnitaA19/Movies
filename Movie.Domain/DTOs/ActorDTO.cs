namespace Movie.Domain.DTOs;

public class ActorDTO
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public ICollection<string> MoviesTitles { get; set; }
}
