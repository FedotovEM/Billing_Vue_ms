﻿namespace NachislService.Models
{
    public class OSVEachAbonentRequest
    {
        public string AccountCd { get; set; }
        public int StartMonth { get; set; }
        public int EndMonth { get; set; }
        public int StartYear { get; set; }
        public int EndYear { get; set; }
        public int ServiceCd { get; set; }
    }
}
