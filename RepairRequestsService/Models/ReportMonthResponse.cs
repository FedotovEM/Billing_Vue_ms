namespace RepairRequestsService.Models
{
    public class ReportMonthResponse
    {
        public int SumCountRequest { get; set; }
        public int SumCountPreviousMonth { get; set; }
        public List<ReportResponse> ReportResponseList { get; set; }
        public ReportMonthResponse()
        {
            ReportResponseList = new List<ReportResponse>();
        }
    }
}
