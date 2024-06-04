namespace NachislService.Models
{
    public class OSVEachAbonentResponse
    {
        public List<OSVEachAbonent> OsvEachAbonent { get; set; }
        public decimal? TotalStartDebet { get; set; }
        public decimal? TotalStartKredit { get; set; }
        public decimal? TotalNachisl { get; set; }
        public decimal? TotalPay{ get; set; }
        public decimal? TotalFinishDebet { get; set; }
        public decimal? TotalFinishKredit { get; set; }
        public OSVEachAbonentResponse() {
            OsvEachAbonent = new List<OSVEachAbonent>();
        }
    }
}
