using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using API.UserManagement.Domain;

namespace API.UserManagement.Data
{
    public class DataContext : IdentityDbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) {}

        public DbSet<User> users {get; set;}
        public DbSet<AdminActions> adminActions { get; set; }
        public DbSet<UserMetaData> usersMetaData { get; set; }

    }
}