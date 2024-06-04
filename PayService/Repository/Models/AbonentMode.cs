using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace PayService.Repository.Models
{
    [Table("abonentmode")]
    public partial class AbonentMode
    {
        [Key]
        [Column("abonentmodeСd")]
        public int AbonentModeСd { get; set; }
        [Required]
        [Column("accountcd")]
        [StringLength(6)]
        public string AccountCd { get; set; }
        [Column("modecd")]
        public int ModeCd { get; set; }
        [Column("counterr")]
        public bool Counterr { get; set; }
    }
}
