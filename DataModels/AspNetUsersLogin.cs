using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HalloDoc.DataModels
{
    public class AspNetUsersLogin
    {
        [Key]
        [StringLength(128)]
        public string Id { get; set; } = null!;

        [Required(ErrorMessage = "UserName is required")]
        [StringLength(256)] 
        public string UserName { get; set; } = null!;

        [Required(ErrorMessage = "Password is required")]
        [Column(TypeName = "character varying")]
        public string ?  PasswordHash { get; set; } = null!;


    }
}
