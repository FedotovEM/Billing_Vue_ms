using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace NachislService.Repository.Models
{
    [Table("unit")]
    public partial class Unit
    {
        [Key]
        [Column("unitcd")]
        public int UnitCd { get; set; }
        [Required]
        [Column("unitsname")]
        [StringLength(15)]
        public string UnitsName { get; set; }
    }
}
