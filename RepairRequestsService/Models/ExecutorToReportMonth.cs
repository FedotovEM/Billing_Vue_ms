namespace RepairRequestsService.Models
{
    public class ExecutorToReportMonth
    {
        public string? ExecutorFIO { get; set; }
        public int CountRequest { get; set; }
        public int CountPreviousMonth { get; set; }
    }
}
