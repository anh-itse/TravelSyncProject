using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TravelSync.Persistence.Abstractions;

namespace TravelSync.Persistence.Repositories;

public class DbContextProvider(
    ApplicationDbContext dbContext,
    IDbContextFactory<ApplicationDbContext> dbContextFactory
    ) : IDbContextProvider
{
    public ApplicationDbContext CreateDbContext()
    {
        if (dbContext != null) return dbContext;

        // Nếu không có (Background Job), thì tạo mới từ DbContextFactory
        return dbContextFactory.CreateDbContext();
    }
}
