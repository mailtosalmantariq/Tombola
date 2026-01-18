using Microsoft.EntityFrameworkCore;
using MT.Tombola.Api.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MT.Tombola.Api.Data.Data
{
    public class BeansDbContext : DbContext
    {
        public BeansDbContext(DbContextOptions<BeansDbContext> options) : base(options) 
        {
            
        }

        public DbSet<Bean> Beans => Set<Bean>();
        public DbSet<BeanOfTheDay> BeanOfTheDays => Set<BeanOfTheDay>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Bean>()
                .HasIndex(b => b.ExternalId)
                .IsUnique();

            modelBuilder.Entity<BeanOfTheDay>()
                .HasIndex(b => b.Date)
                .IsUnique();

            modelBuilder.Entity<Bean>()
                .Property(b => b.Id)
                .ValueGeneratedOnAdd();
        }
    }
}
