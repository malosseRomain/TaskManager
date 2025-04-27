using System.ComponentModel.DataAnnotations;

namespace TaskMaster.Models
{
    public class User
    {
        [Key]
        public int Id_User { get; set; }

        [Required]
        [StringLength(100)]
        public string Nom { get; set; }

        [Required]
        [StringLength(100)]
        public string Prenom { get; set; }

        [Required]
        [StringLength(255)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(255)]
        public string Password { get; set; }

        public ICollection<Commentaire> Commentaires { get; set; }

        public string DisplayName => $"{Prenom} {Nom}";
    }
}