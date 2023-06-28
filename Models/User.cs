using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace AniversaryApp.Models
{
    public class User
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }
        public string? NameA { get; set; }

        public string? UserNameA { get; set; }
        public string? PasswordA { get; set; }
        
        [Column(TypeName = "datetime2")]
        public DateTime Birthday { get; set; }
        
        /*if UserTypeId  = 1 -> Admin , if it is 2 -> normal user*/
        public int UserTypeId { get; set; }
        public string? UserEmailA { get; set; }
        
    }
}
