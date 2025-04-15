using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TaskManager.Models;

namespace TaskManager.Data
{
    public class TaskDbContext : DbContext
    {
        public DbSet<TaskItem> Tasks { get; set; }

        public TaskDbContext(DbContextOptions<TaskDbContext> options) : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            if (!options.IsConfigured)
            {
                options
                    .UseMySql(
                        "server=localhost;database=taskmaster;user=root;password=root",
                        new MySqlServerVersion(new Version(8, 0, 30))
                    )
                    .LogTo(Console.WriteLine, LogLevel.Error); // Ajout des journaux
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Déclare l'entité Comment comme keyless
            modelBuilder.Entity<Comment>().HasNoKey();

            base.OnModelCreating(modelBuilder);
        }
    }
}
