namespace TravelSync.Persistence.Abstractions;

public interface IDbContextProvider
{
    ApplicationDbContext CreateDbContext();
}
