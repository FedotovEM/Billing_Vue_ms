using PayService.Repository.Models;

namespace PayService.DTO
{
    public class PaySummaDTO: PaySumma
    {
        public string AccountCd { get; set; }
        public string ServiceName { get; set; }
        public string ReceptionName { get; set; }
    }
}
