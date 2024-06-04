using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace RepairRequestsService.Repository.Models
{
    [Table("disrepair")]
    public partial class Disrepair
    {
        [Key]
        [Column("failurecd")]
        public int FailureCd { get; set; }
        [Column("failurename")]
        [Required]
        [StringLength(50)]
        public string FailureName { get; set; }
    }
}
