using Microsoft.EntityFrameworkCore;
using TaskMaster.Models;

namespace TaskMaster.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Projet> Projets { get; set; }
        public DbSet<TaskItem> Tasks { get; set; }
        public DbSet<Commentaire> Commentaires { get; set; }
        public DbSet<SubTask> SubTasks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuration de User
            modelBuilder.Entity<User>()
                .HasKey(u => u.Id_User);

            // Configuration de Projet
            modelBuilder.Entity<Projet>()
                .HasKey(p => p.Id_Projet);

            // Configuration de TaskItem
            modelBuilder.Entity<TaskItem>()
                .HasKey(t => t.Id_Task);

            modelBuilder.Entity<TaskItem>()
                .Property(t => t.Titre)
                .HasMaxLength(100);

            modelBuilder.Entity<TaskItem>()
                .Property(t => t.Description)
                .HasMaxLength(500);

            // Configuration des enums comme des chaînes
            modelBuilder.Entity<TaskItem>()
                .Property(t => t.Categorie)
                .HasConversion<string>()
                .HasMaxLength(50);

            modelBuilder.Entity<TaskItem>()
                .Property(t => t.Priorite)
                .HasConversion<string>()
                .HasMaxLength(20);

            modelBuilder.Entity<TaskItem>()
                .Property(t => t.Statut)
                .HasConversion<string>()
                .HasMaxLength(20);

            // Configuration des relations
            modelBuilder.Entity<TaskItem>()
                .HasOne(t => t.Auteur)
                .WithMany()
                .HasForeignKey(t => t.Id_Auteur)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TaskItem>()
                .HasOne(t => t.Realisateur)
                .WithMany()
                .HasForeignKey(t => t.Id_Realisateur)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TaskItem>()
                .HasOne(t => t.Projet)
                .WithMany(p => p.Tasks)
                .HasForeignKey(t => t.Id_Projet)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TaskItem>()
                .HasMany(t => t.SousTaches)
                .WithOne(st => st.TaskParent)
                .HasForeignKey(st => st.Id_TaskParent)
                .OnDelete(DeleteBehavior.Cascade);

            // Configuration de SubTask
            modelBuilder.Entity<SubTask>()
                .HasKey(st => st.Id_SubTask);

            modelBuilder.Entity<SubTask>()
                .Property(st => st.Titre)
                .HasMaxLength(100);

            modelBuilder.Entity<SubTask>()
                .Property(st => st.Statut)
                .HasConversion<string>()
                .HasMaxLength(20);

            // Configuration de Commentaire
            modelBuilder.Entity<Commentaire>()
                .HasKey(c => c.Id_Commentaire);

            modelBuilder.Entity<Commentaire>()
                .Property(c => c.Contenu)
                .HasMaxLength(500);

            modelBuilder.Entity<Commentaire>()
                .HasOne(c => c.Auteur)
                .WithMany(u => u.Commentaires)
                .HasForeignKey(c => c.Id_Auteur)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Commentaire>()
                .HasOne(c => c.Task)
                .WithMany(t => t.Commentaires)
                .HasForeignKey(c => c.Id_Task)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
