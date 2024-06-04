using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace PayService.Repository.Models
{
    [Table("receptionpoint")]
    public partial class ReceptionPoint
    {
        [Key]
        [Column("receptionpointcd")]
        public int ReceptionPointCd { get; set; }
        [Column("receptionname")]
        [Required]
        [StringLength(50)]
        public string ReceptionName { get; set; }
    }
}
