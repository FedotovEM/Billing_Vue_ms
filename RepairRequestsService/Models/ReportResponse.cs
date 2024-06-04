namespace RepairRequestsService.Models
{
    public class ReportResponse
    {
        public int FailureCd { get; set; }
        public string FailureName { get; set; }
        public int FullCountRequest { get; set; }
        public int FullCountPreviousMonth { get; set; }
        public List<ExecutorToReportMonth> ExecutorsToReportMonths { get; set; }
        public ReportResponse()
        {
            ExecutorsToReportMonths = new List<ExecutorToReportMonth>();
        }
    }
}
