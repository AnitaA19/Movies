namespace Movie.Infrastucture.Data;

public class UnitOfWork
{
    private readonly MovieDbContext _context;
    public UnitOfWork(MovieDbContext context)
    {
        _context = context;
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
