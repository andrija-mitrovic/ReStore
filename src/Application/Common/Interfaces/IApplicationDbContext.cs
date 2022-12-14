using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<Product> Products { get; }
        DbSet<Basket> Baskets { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
