using System.ComponentModel.DataAnnotations;
using System;
using Microsoft.AspNetCore.Identity;
namespace PressStart.Models
{
    public class User
    {
        public int UserID {get; set;}

        [Required, MinLength(1), MaxLength(150)]
        public IdentityUser Name {get; set;}

        [Required]
        public IdentityUser Email {get; set;}

        [Required]
        public IdentityUser Password {get; set;}

        public int RoleId {get; set;}
    }
}