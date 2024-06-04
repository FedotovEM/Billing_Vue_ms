using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace WorkerService_Nachislenie.Repository.Models
{
    [Table("price")]
    public partial class Price
    {
        [Key]
        [Column("pricecd")]
        public int PriceCd { get; set; }
        [Column("pricevalue")]
        public decimal PriceValue { get; set; }
        [Column("modecd")]
        public int ModeCd { get; set; }
    }
}
