using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace NachislService.Repository.Models
{
    [Table("nachislsumma")]
    public partial class NachislSumma
    {
        [Key]
        [Column("nachislfactcd")]
        public int NachislFactCd { get; set; }
        [Column("nachislsum")]
        public decimal NachislSum { get; set; }
        [Column("nachislyear")]
        public short NachislYear { get; set; }
        [Column("nachislmonth")]
        public short NachislMonth { get; set; }
        [Column("nachtype")]
        [Required]
        [StringLength(20)]
        public string NachType { get; set; }
        [Column("abonentmodeСd")]
        public int AbonentModeСd { get; set; }
        [Column("countresources")]
        public decimal CountResources { get; set; }
    }
}
