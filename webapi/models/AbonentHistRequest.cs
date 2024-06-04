namespace webapi.models
{
    public class AbonentHistRequest
    {
        public string AccountCd { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
