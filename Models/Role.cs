using System.ComponentModel.DataAnnotations;
namespace PressStart.Models
{
    public class Role
    {
        public int RoleId {get; set;}

        [Required]
        public bool Subscription {get; set;}
    }
}