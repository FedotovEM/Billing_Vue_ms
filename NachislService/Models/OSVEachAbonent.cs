namespace NachislService.Models
{
    public class OSVEachAbonent
    {
        public string Accountcd { get; set; }
        public string FIO { get; set; }
        public string Address { get; set; }
        public decimal? BeginDebetSum { get; set; }
        public decimal? BeginKreditSum { get; set; }
        public decimal? NachislSum { get; set; }
        public decimal? PaySum { get; set; }
        public decimal? FinishDebetSum { get; set; }
        public decimal? FinishKreditSum { get; set; }
    }
}
