using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManager.Models
{
    public class TaskItem
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public DateTime DateCreation { get; set; } = DateTime.Now;
        public DateTime? Echeance { get; set; }
        public required TaskStatus Statut { get; set; }
        public required TaskPriority Priorite { get; set; }
        public required Person Auteur { get; set; }
        public required Person Realisateur { get; set; }
        public required string Categorie { get; set; }
        public List<string> Etiquettes { get; set; } = new();
        public List<SubTask> SousTaches { get; set; } = new();
        [NotMapped]
        public List<Comment> Commentaires { get; set; } = new();
    }


    [NotMapped]
    public class Person
    {
        public required string Nom { get; set; }

        public required string Prenom { get; set; }

        public required string Email { get; set; }
    }

    [NotMapped]
    public class SubTask
    {
        public required string Title { get; set; }

        public TaskStatus Statut { get; set; }

        public DateTime? Echeance { get; set; }
    }

    public class Comment
    {
        public required Person Auteur { get; set; }

        public DateTime Date { get; set; } = DateTime.Now;

        public required string Contenu { get; set; }
    }

    public enum TaskStatus
    {
        Afaire,
        EnCours,
        Terminee,
        Annulee
    }

    public enum TaskPriority
    {
        Basse,
        Moyenne,
        Haute,
        Critique
    }
}

