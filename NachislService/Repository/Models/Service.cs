using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace NachislService.Repository.Models
{
    [Table("services")]
    public partial class Service
    {
        [Key]
        [Column("servicecd")]
        public int ServiceCd { get; set; }
        [Required]
        [Column("servicename")]
        [StringLength(50)]
        public string ServiceName { get; set; }
        [Column("unitscd")]
        public int UnitsCd { get; set; }
    }
}
