using NachislService.Repository.Models;

namespace NachislService.Models
{
    public class SearchAbonentCardResponse
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
        public List<AbonentCard> AbonentCards { get; set; }

        public SearchAbonentCardResponse(Abonent abonent)
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
            AbonentCards = new List<AbonentCard>();
        }
    }
}
