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
        public DbSet<Models.Task> Tasks { get; set; }
        public DbSet<SubTask> SubTasks { get; set; }
        public DbSet<Projet> Projets { get; set; }
        public DbSet<Commentaire> Commentaires { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuration des enums comme des chaînes avec contraintes
            modelBuilder.Entity<Models.Task>()
                .Property(t => t.Categorie)
                .HasConversion<string>()
                .HasMaxLength(20)
                .HasAnnotation("CheckConstraint", "Categorie IN ('Travail', 'Personnel', 'Urgent', 'Important')");

            modelBuilder.Entity<Models.Task>()
                .Property(t => t.Priorite)
                .HasConversion<string>()
                .HasMaxLength(20)
                .HasAnnotation("CheckConstraint", "Priorite IN ('Basse', 'Moyenne', 'Haute', 'Critique')");

            modelBuilder.Entity<Models.Task>()
                .Property(t => t.Statut)
                .HasConversion<string>()
                .HasMaxLength(20)
                .HasAnnotation("CheckConstraint", "Statut IN ('Afaire', 'EnCours', 'Terminee', 'Annulee')");

            // Configuration des relations pour Projet
            modelBuilder.Entity<Projet>()
                .HasOne(p => p.Createur)
                .WithMany()
                .HasForeignKey(p => p.Id_Createur)
                .OnDelete(DeleteBehavior.Cascade);

            // Configuration des relations pour Task
            modelBuilder.Entity<Models.Task>()
                .HasOne(t => t.Projet)
                .WithMany(p => p.Tasks)
                .HasForeignKey(t => t.Id_Projet)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Models.Task>()
                .HasOne(t => t.Auteur)
                .WithMany()
                .HasForeignKey(t => t.Id_Auteur)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Models.Task>()
                .HasOne(t => t.Realisateur)
                .WithMany()
                .HasForeignKey(t => t.Id_Realisateur)
                .OnDelete(DeleteBehavior.Cascade);

            // Configuration des relations pour SubTask
            modelBuilder.Entity<SubTask>()
                .HasOne(st => st.Task)
                .WithMany(t => t.SubTasks)
                .HasForeignKey(st => st.Id_Task)
                .OnDelete(DeleteBehavior.Cascade);

            // Configuration des relations pour Commentaire
            modelBuilder.Entity<Commentaire>()
                .HasOne(c => c.User)
                .WithMany(u => u.Commentaires)
                .HasForeignKey(c => c.Id_User)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Commentaire>()
                .HasOne(c => c.Task)
                .WithMany(t => t.Commentaires)
                .HasForeignKey(c => c.Id_Task)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
