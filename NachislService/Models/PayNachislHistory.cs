namespace NachislService.Models
{
    public class PayNachislHistory
    {
        public int ServiceCd { get; set; }
        public string ServiceName { get; set; }
        public string AccountingMonth { get; set; }
        public decimal? BeginRemainSum { get; set; }
        public decimal? NachislSum { get; set; }
        public decimal CountResources { get; set; }
        public decimal? PaySum { get; set; }
        public decimal? EndRemainSum { get; set; }
        public int Month { get; set; }
    }
}
