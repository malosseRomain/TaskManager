﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Windows.System;

namespace TaskMaster.Models
{
    public class Commentaire
    {
        [Key]
        public int Id_Commentaire { get; set; }

        [Required]
        public int Id_Task { get; set; }

        [Required]
        public int Id_User { get; set; }

        [Required]
        public string Contenu { get; set; }

        [Required]
        public DateTime DateCreation { get; set; } = DateTime.Now;

        [ForeignKey("Id_Task")]
        public Task Task { get; set; }

        [ForeignKey("Id_User")]
        public User User { get; set; }
    }
}