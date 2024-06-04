using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace RepairRequestsService.Repository.Models
{
    [Table("executor")]
    public partial class Executor
    {
        [Key]
        [Column("executorcd")]
        public int ExecutorCd { get; set; }
        [Required]
        [Column("fio")]
        [StringLength(50)]
        public string Fio { get; set; }
    }
}
