using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Windows.System;

namespace TaskMaster.Models
{
    public class Task
    {
        [Key]
        public int Id_Task { get; set; }

        [Required]
        [StringLength(255)]
        public string Titre { get; set; }

        public DateTime? Echeance { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        public DateTime DateCreation { get; set; } = DateTime.Now;

        public TaskCategory Categorie { get; set; }

        [Required]
        public TaskPriority Priorite { get; set; }

        [Required]
        public TaskStatus Statut { get; set; }

        [Required]
        public int Id_Auteur { get; set; }

        public int Id_Realisateur { get; set; }

        public int? Id_Projet { get; set; }

        [StringLength(500)]
        public string? Etiquettes { get; set; }

        [ForeignKey("Id_Auteur")]
        public User Auteur { get; set; }

        [ForeignKey("Id_Realisateur")]
        public User Realisateur { get; set; }

        [ForeignKey("Id_Projet")]
        public Projet Projet { get; set; }

        public ICollection<SubTask> SubTasks { get; set; }
        public ICollection<Commentaire> Commentaires { get; set; }
    }
}