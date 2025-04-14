using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TaskManager.Models;

namespace TaskManager.Data
{
    public class TaskDbContext : DbContext
    {
        public DbSet<TaskItem> Tasks { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseMySql("server=localhost;database=taskmaster;user=root;password=root",
                new MySqlServerVersion(new Version(8, 0, 30)));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Déclare l'entité Comment comme keyless
            modelBuilder.Entity<Comment>().HasNoKey();

            base.OnModelCreating(modelBuilder);
        }
    }
}
