using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NetCore_InMemory_Redis.Models;

namespace NetCore_InMemory_Redis.Data
{
    public class IdentityAppDbContext : IdentityDbContext<User>
    {
        public IdentityAppDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<User> User { get; set; }
        public DbSet<Product> Products { get; set; }
    }
}
