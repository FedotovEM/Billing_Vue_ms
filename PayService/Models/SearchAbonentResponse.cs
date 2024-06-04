using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using PayService.Repository.Models;

namespace PayService.Models
{
    public class SearchAbonentResponse
    {       
        public string AccountCd { get; set; }        
        public string Fio { get; set; }        
        public string StreetName { get; set; }       
        public short HouseNo { get; set; }       
        public short? FlatNo { get; set; }      
        public string Phone { get; set; }        
        public int? Corpus { get; set; }        
        public string District { get; set; }        
        public int? CountPerson { get; set; }        
        public double? SizeFlat { get; set; }
        public List<RemainsData> AbonentRemains { get; set; }

        public SearchAbonentResponse(Abonent abonent)
        {
            AccountCd = abonent.AccountCd;
            Fio = abonent.Fio;
            HouseNo = abonent.HouseNo;
            FlatNo = abonent.FlatNo;
            Phone = abonent.Phone;
            Corpus = abonent.Corpus;
            District = abonent.District;
            CountPerson = abonent.CountPerson;
            SizeFlat = abonent.SizeFlat;
            AbonentRemains = new List<RemainsData>();
        }
    }
}
