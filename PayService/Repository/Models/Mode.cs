using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace PayService.Repository.Models
{
    [Table("mode")]
    public partial class Mode
    {
        [Key]
        [Column("modecd")]
        public int ModeCd { get; set; }
        [Column("modename")]
        [Required]
        [StringLength(230)]
        public string ModeName { get; set; }
        [Column("norma")]
        public decimal Norma { get; set; }
        [Column("servicecd")]
        public int ServiceCd { get; set; }
    }
}
