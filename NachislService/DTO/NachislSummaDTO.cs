using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using NachislService.Repository.Models;

namespace NachislService.DTO
{
    public class NachislSummaDTO: NachislSumma
    {
        public string AccountCd { get; set; }
        public string ServiceName { get; set; }
        public int RemainCd { get; set; }
    }
}
