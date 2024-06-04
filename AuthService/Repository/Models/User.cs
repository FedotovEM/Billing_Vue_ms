using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable
namespace AuthService.Repository.Models
{
    [Table("user")]
    public class User
    {
        [Key]
        [Column("usercd")]
        public int UserCD { get; set; }

        [Column("username")]
        public string UserName { get; set; }

        [Column("userpassword")]
        public string Password { get; set; }

        [Column("useremail")]
        public string Email { get; set; }

        [Column("userrole")]
        public string UserRole { get; set; }
    }
}
