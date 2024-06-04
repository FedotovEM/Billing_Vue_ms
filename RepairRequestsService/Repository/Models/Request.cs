using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace RepairRequestsService.Repository.Models
{
    [Table("request")]
    public partial class Request
    {
        [Key]
        [Column("requestcd")]
        public int RequestCd { get; set; }
        [Required]
        [Column("accountcd")]
        [StringLength(6)]
        public string AccountCd { get; set; }
        [Column("failurecd")]
        public int FailureCd { get; set; }
        [Column("executorcd")]
        public int? ExecutorCd { get; set; }
        [Column("incomingdate", TypeName = "timestamp(6)")]
        public DateTime IncomingDate { get; set; } 
        [Column("executiondate", TypeName = "timestamp(6)")]
        public DateTime? ExecutionDate { get; set; }
        [Column("executed")]
        public bool Executed { get; set; }
    }
}
