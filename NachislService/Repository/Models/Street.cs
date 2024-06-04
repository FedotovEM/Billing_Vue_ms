using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace NachislService.Repository.Models
{
    [Table("street")]
    public partial class Street
    {
        [Key]
        [Column("streetcd")]
        public int StreetCd { get; set; }
        [Required]
        [Column("streetname")]
        [StringLength(50)]
        public string StreetName { get; set; }
    }
}
