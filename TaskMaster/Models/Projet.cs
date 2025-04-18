using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Windows.System;

namespace TaskMaster.Models
{
    public class Projet
    {
        [Key]
        public int Id_Projet { get; set; }

        [Required]
        [StringLength(255)]
        public string Nom { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }

        [Required]
        public DateTime DateCreation { get; set; } = DateTime.Now;

        [Required]
        public int Id_Createur { get; set; }

        [ForeignKey("Id_Createur")]
        public User Createur { get; set; }

        public ICollection<Task> Tasks { get; set; }
    }
}