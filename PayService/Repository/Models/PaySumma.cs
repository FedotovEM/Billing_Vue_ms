using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace PayService.Repository.Models
{
    [Table("paysumma")]
    public partial class PaySumma
    {
        [Key]
        [Column("payfactcd")]
        public int PayFactCd { get; set; }
        [Column("paysum")]
        public decimal PaySum { get; set; }
        [Column("paydate")]
        public DateTime PayDate { get; set; }
        [Column("paymonth")]
        public short PayMonth { get; set; }
        [Column("payyear")]
        public short PayYear { get; set; }
        [Column("abonentmodeСd")]
        public int AbonentModeСd { get; set; }
        [Column("paytype")]
        [Required]
        [StringLength(30)]
        public string PayType { get; set; }
        [Column("receptionpointcd")]
        public int ReceptionPointCd { get; set; }
    }
}
