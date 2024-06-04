namespace PayService.Models
{
    public class RemainsData
    {
        public int ServiceCd { get; set; }
        public string ServiceName { get; set; }
        public bool Counter { get; set; }
        public int RemainCd { get; set; }
        public int RemainMonth { get; set; }
        public int RemainYear { get; set;}
        public decimal RenSum { get; set; }
    }
}
