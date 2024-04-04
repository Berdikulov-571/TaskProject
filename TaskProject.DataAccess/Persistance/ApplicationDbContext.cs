using Microsoft.EntityFrameworkCore;
using TaskProject.Domain.Entities;
using TaskProject.Service.Abstractions.DataAccess;

namespace TaskProject.DataAccess.Persistance
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Product> Products { get ; set ; }
    }
}