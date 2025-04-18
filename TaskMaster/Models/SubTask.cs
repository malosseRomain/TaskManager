using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskMaster.Models
{
    public class SubTask
    {
        [Key]
        public int Id_SubTask { get; set; }

        [Required]
        [StringLength(255)]
        public string Titre { get; set; }

        [Required]
        public int Id_Task { get; set; }

        [Required]
        public TaskStatus Statut { get; set; }

        public DateTime? Echeance { get; set; }

        [ForeignKey("Id_Task")]
        public Task Task { get; set; }
    }
}