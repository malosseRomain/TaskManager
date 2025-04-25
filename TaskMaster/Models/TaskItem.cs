using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskMaster.Models
{
    public class TaskItem
    {
        [Key]
        public int Id_Task { get; set; }

        [Required]
        [StringLength(100)]
        public string Titre { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        public DateTime DateCreation { get; set; }

        public DateTime? Echeance { get; set; }

        [Required]
        public TaskCategory Categorie { get; set; }

        [Required]
        public TaskPriority Priorite { get; set; }

        [Required]
        public TaskStatus Statut { get; set; }

        [NotMapped]
        public string StatutDisplay => StatusDisplay.GetStatusDisplays()
            .FirstOrDefault(s => s.Value == Statut.ToString())?.DisplayText ?? Statut.ToString();

        [StringLength(200)]
        public string? Etiquettes { get; set; }

        [ForeignKey("Projet")]
        public int? Id_Projet { get; set; }
        public Projet Projet { get; set; }

        [ForeignKey("Auteur")]
        public int Id_Auteur { get; set; }
        public User Auteur { get; set; }

        [ForeignKey("Realisateur")]
        public int Id_Realisateur { get; set; }
        public User Realisateur { get; set; }

        public ICollection<SubTask> SousTaches { get; set; }
        public ICollection<Commentaire> Commentaires { get; set; }
    }
} 