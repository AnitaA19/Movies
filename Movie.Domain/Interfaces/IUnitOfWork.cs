namespace Movie.Domain.Interfaces;

public interface IUnitOfWork
{
    Task SaveChangesAsync();
}
