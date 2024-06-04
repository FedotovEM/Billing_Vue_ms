using NachislService.Repository.Models;

namespace NachislService.DTO
{
    public class PriceDTO: Price
    {
        public string ModeName { get; set; }
    }
}
