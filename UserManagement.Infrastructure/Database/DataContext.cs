using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using UserManagement.Domain.Entities;
using System.Reflection;

namespace UserManagement.Infrastructure.Database
{
    public class DataContext : IdentityDbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) {}

        public DbSet<User> users {get; set;}
        public DbSet<AdminActions> adminActions { get; set; }
        public DbSet<UserMetaData> usersMetaData { get; set; }
        public DbSet<RefreshToken> refreshTokens { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(builder);
        }

    }
}