using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Travel_Express.Models
{
    public class NewAccount
    {
        [RegularExpression(@"^[a-zA-Z][0-9a-zA-Z.]*@[a-zA-Z]+.[a-zA-Z]+$", ErrorMessage = "Veuillez entrer une adresse email valide")]
        [Required(ErrorMessage = "Veuillez entrer une adresse email.")]
        [StringLength(50)]
        [Display(Name = "Adresse Email")]
        public string Mail { get; set; }
        [Display(Name = "Mot de passe")]
        [StringLength(64, MinimumLength = 6, ErrorMessage = "Votre mot de passe doit contenir au moins 6 carractères")]
        [Required(ErrorMessage = "Veuillez entrer un mot de passe.")]
        public string Password { get; set; }
    }
}
