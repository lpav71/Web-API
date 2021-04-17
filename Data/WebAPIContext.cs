using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebAPI.Models;

namespace WebAPI.Data
{
    public class WebAPIContext : DbContext
    {
        public WebAPIContext (DbContextOptions<WebAPIContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<WebAPI.Models.Stuff> Stuff { get; set; }
        public DbSet<WebAPI.Models.Division> Division { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Stuff>().Property(p => p.FirstName).HasMaxLength(100);
            modelBuilder.Entity<Stuff>().Property(p => p.LastName).HasMaxLength(100);
            modelBuilder.Entity<Division>().Property(p => p.Name).HasMaxLength(50);
        }
    }
}
