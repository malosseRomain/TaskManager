using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace TaskManager.Models
{
    public class TaskItem
    {
        [Key]
        public int Id { get; set; }  // Clé primaire

        [Required]
        [MaxLength(100)]
        public string Title { get; set; }  // Titre court de la tâche

        public string Description { get; set; }  // Détail de la tâche

        public DateTime DateCreation { get; set; } = DateTime.Now;  // Date de création

        public DateTime? Echeance { get; set; }  // Date limite (optionnelle)

        [Required]
        public TaskStatus Statut { get; set; }  // Statut de la tâche

        [Required]
        public TaskPriority Priorite { get; set; }  // Priorité

        [Required]
        public Person Auteur { get; set; }  // Auteur de la tâche

        public Person Realisateur { get; set; }  // Réalisateur (optionnel)

        public string Categorie { get; set; }  // Catégorie (perso, travail, projet, etc.)

        public List<string> Etiquettes { get; set; } = new();  // Liste de mots-clés

        public List<SubTask> SousTaches { get; set; } = new();  // Liste des sous-tâches

        public List<Comment> Commentaires { get; set; } = new();  // Liste des commentaires
    }

    public class Person
    {
        [Required]
        public string Nom { get; set; }

        [Required]
        public string Prenom { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }
    }

    public class SubTask
    {
        [Required]
        public string Title { get; set; }

        public TaskStatus Statut { get; set; }

        public DateTime? Echeance { get; set; }  // Facultatif
    }

    public class Comment
    {
        [Required]
        public Person Auteur { get; set; }

        public DateTime Date { get; set; } = DateTime.Now;

        [Required]
        public string Contenu { get; set; }
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

