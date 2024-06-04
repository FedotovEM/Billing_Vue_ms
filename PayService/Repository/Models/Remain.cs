using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace PayService.Repository.Models
{
    [Table("remains")]
    public partial class Remain
    {
        [Key]
        [Column("remaincd")]
        public int RemainCd { get; set; }
        [Required]
        [Column("accountcd")]
        [StringLength(6)]
        public string AccountCd { get; set; }
        [Column("servicecd")]
        public int ServiceCd { get; set; }
        [Column("remmonth")]
        public short Remmonth { get; set; }
        [Column("remyear")]
        public short Remyear { get; set; }
        [Column("remainsum")]
        public decimal? Remainsum { get; set; }
    }
}
