using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

#nullable disable

namespace NachislService.Repository.Models
{
    [Table("abonent")]
    public partial class Abonent
    {
        [Key]
        [Column("accountcd")]
        [StringLength(6)]
        public string AccountCd { get; set; }
        [Required]
        [Column("fio")]
        [StringLength(70)]
        public string Fio { get; set; }
        [Column("streetcd")]
        public int StreetCd { get; set; }
        [Column("houseno")]
        public short HouseNo { get; set; }
        [Column("flatno")]
        public short? FlatNo { get; set; }
        [Column("phone")]
        [StringLength(16)]
        public string Phone { get; set; }
        [Column("corpus")]
        public int? Corpus { get; set; }
        [Column("district")]
        [StringLength(20)]
        public string District { get; set; }
        [Column("countperson")]
        public int? CountPerson { get; set; }
        [Column("sizeflat")]
        public double? SizeFlat { get; set; }
    }
}
