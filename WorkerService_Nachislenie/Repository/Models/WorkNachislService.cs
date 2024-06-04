using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WorkerService_Nachislenie.Repository.Models
{
    [Table("worknachislservice")]
    public class WorkNachislService
    {
        [Key]
        [Column("id")]
        public int id { get; set; }
        [Column("accountcd")]
        public string accountcd { get; set; }
        [Column("nachislyear")]
        public int nachislyear { get; set; }
        [Column("nachislmonth")]
        public int nachislmonth { get; set; }
        [Column("status")]
        public int status { get; set; }
    }
}
