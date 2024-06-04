using Microsoft.AspNetCore.Mvc;
using System.Net;
using webapi.DTO;
using webapi.Helpers;
using webapi.models;
using webapi.Repository;
using webapi.Repository.Models;

namespace webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AbonentsController : ControllerBase
    {
        private readonly BillingAbonentDbContext _context;

        public AbonentsController(BillingAbonentDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Составляет историю измений данных абонента за период
        /// </summary>
        /// <param name="model">Модель запроса составления истории</param>
        /// <returns>Список записей с историческими данными абонента за период</returns>
        [HttpPost]
        [Route("get-abonent-hist")]
        [Authorization]
        public List<AbonentHistDTO> SearchAbonent([FromBody] AbonentHistRequest model)
        {
            var abonentHists = _context.AbonentHist.ToList();
            if (!string.IsNullOrEmpty(model.AccountCd))
            {
                string accountCd = model.AccountCd.Split(' ')[0];
                abonentHists = abonentHists.Where(a => a.AccountCd == accountCd).ToList();
            }
            if (model.StartDate != null && model.StartDate > DateTime.MinValue)
            {
                abonentHists = abonentHists.Where(ah => ah.StartDate >= model.StartDate).ToList();
            }
            if (model.EndDate != null && model.EndDate > DateTime.MinValue)
            {
                abonentHists = abonentHists.Where(ah => ah.StartDate <= model.EndDate).ToList();
            }
            abonentHists = abonentHists.OrderByDescending(ah => ah.StartDate).ToList();

            List<AbonentHistDTO> histDTO = new List<AbonentHistDTO>();

            foreach (var hist in abonentHists)
            {
                var street = _context.Streets.FirstOrDefault(s => s.StreetCd == hist.StreetCd);
                AbonentHistDTO dTO = new AbonentHistDTO
                {
                    AccountCd = hist.AccountCd,
                    Fio = hist.Fio,
                    StreetCd = hist.StreetCd,
                    StreetName = street.StreetName,
                    HouseNo = hist.HouseNo,
                    FlatNo = hist.FlatNo,
                    Phone = hist.Phone,
                    Corpus = hist.Corpus,
                    District = hist.District,
                    CountPerson = hist.CountPerson,
                    SizeFlat = hist.SizeFlat,
                    StartDate = hist.StartDate,
                    EndDate = hist.EndDate,
                };
                histDTO.Add(dTO);
            }

            return histDTO;
        }

        /// <summary>
        /// Составляет полную историю измений данных абонента
        /// </summary>
        /// <param name="id">Лицевой счёт абонента</param>
        /// <returns>Список записей с историческими данными абонента</returns>
        [HttpGet]
        [Route("abonent-hist/{id:int}")]
        [Authorization]
        public List<AbonentHistDTO> AbonentHist(int id)
        {
            List<AbonentHist> abonentHists = new List<AbonentHist>();
            var accountCD = id.ToString();
            if (!string.IsNullOrEmpty(accountCD))
            {
                abonentHists = _context.AbonentHist.Where(a => a.AccountCd == accountCD).ToList();
            }
            abonentHists = abonentHists.OrderByDescending(ah => ah.StartDate).ToList();

            List<AbonentHistDTO> histDTO = new List<AbonentHistDTO>();

            foreach (var hist in abonentHists)
            {
                var street = _context.Streets.FirstOrDefault(s => s.StreetCd == hist.StreetCd);
                AbonentHistDTO dTO = new AbonentHistDTO
                {
                    AccountCd = hist.AccountCd,
                    Fio = hist.Fio,
                    StreetCd = hist.StreetCd,
                    StreetName = street.StreetName,
                    HouseNo = hist.HouseNo,
                    FlatNo = hist.FlatNo,
                    Phone = hist.Phone,
                    Corpus = hist.Corpus,
                    District = hist.District,
                    CountPerson = hist.CountPerson,
                    SizeFlat = hist.SizeFlat,
                    StartDate = hist.StartDate,
                    EndDate = hist.EndDate,
                };
                histDTO.Add(dTO);
            }

            return histDTO;
        }

        [HttpPost]
        [Route("search-abonent")]
        [Authorization]
        public List<SearchAbonentResponse> SearchAbonent([FromBody] SearchAbonentModel model)
        {
            var abonents = _context.Abonents.ToList();
            if (!string.IsNullOrEmpty(model.streetName))
            {
                var street = _context.Streets.FirstOrDefault(s => s.StreetName == model.streetName);
                if (street != null)
                    abonents = abonents.Where(a => a.StreetCd == street.StreetCd).ToList();
            }
            if (model.houseNo != null && model.houseNo > 0)
            {
                abonents = abonents.Where(a => a.HouseNo == model.houseNo).ToList();
            }
            if (model.flatNo != null && model.flatNo > 0)
            {
                abonents = abonents.Where(a => a.FlatNo == model.flatNo).ToList();
            }
            if (model.corpus != null && model.corpus > 0)
            {
                abonents = abonents.Where(a => a.Corpus == model.corpus).ToList();
            }
            if (!string.IsNullOrEmpty(model.accountCd))
            {
                string accountCd = model.accountCd.Split(' ')[0];
                abonents = abonents.Where(a => a.AccountCd == accountCd).ToList();
            }

            List<SearchAbonentResponse> response = new List<SearchAbonentResponse>();

            foreach (var abonent in abonents)
            {
                SearchAbonentResponse abonentResp = new SearchAbonentResponse(abonent);
                var street = _context.Streets.FirstOrDefault(s => s.StreetCd == abonent.StreetCd);
                abonentResp.StreetName = street.StreetName;
                response.Add(abonentResp);
            }

            return response;
        }

        /// <summary>
        /// Получает всех абонентов
        /// </summary>
        /// <returns>Возвращает всех абонентов с указанием значений из родительских таблиц</returns>
        [Authorization]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AbonentDTO>>> GetAbonents()
        {
            if (_context.Abonents == null) return NotFound();
            List<AbonentDTO> abonentDTOs = new List<AbonentDTO>();
            AbonentDTO dTO;

            var appInformationToReturn = _context.Abonents
                .Join(_context.Streets,
                a => a.StreetCd,
                s => s.StreetCd,
                (a, s) => new
                {
                    AccountCd = a.AccountCd,
                    Fio = a.Fio,
                    StreetCd = s.StreetCd,
                    StreetName = s.StreetName,
                    HouseNo = a.HouseNo,
                    FlatNo = a.FlatNo,
                    Phone = a.Phone,
                    Сorpus = a.Corpus,
                    District = a.District,
                    CountPerson = a.CountPerson,
                    SizeFlat = a.SizeFlat,
                });

            foreach (var item in appInformationToReturn)
            {
                dTO = new AbonentDTO()
                {
                    AccountCd = item.AccountCd,
                    Fio = item.Fio,
                    StreetCd = item.StreetCd,
                    StreetName = item.StreetName,
                    HouseNo = item.HouseNo,
                    FlatNo = item.FlatNo,
                    Phone = item.Phone,
                    Corpus = item.Сorpus,
                    District = item.District,
                    CountPerson = item.CountPerson,
                    SizeFlat = item.SizeFlat,
                };
                abonentDTOs.Add(dTO);
            }
            List<AbonentDTO> abonentDeleted = new List<AbonentDTO>();
            foreach (var item in abonentDTOs)
            {
                var deletedList = _context.AbonentHist.Where(ah => ah.AccountCd == item.AccountCd && ah.Deleted).ToList();
                if (deletedList.Any())
                    abonentDeleted.Add(item);
            }

            foreach (var item in abonentDeleted)
            {
                abonentDTOs.Remove(item);
            }

            return abonentDTOs;
        }

        /// <summary>
        /// Получает определенного абонента
        /// </summary>
        /// <param name="id">Лицевой счёт абонента</param>
        /// <returns>Объект абонента с указанием значений из родительских таблиц</returns>
        [HttpGet("{id}")]
        [Authorization]
        public async Task<ActionResult<AbonentDTO>> GetAbonent(string id)
        {
            if (_context.Abonents == null) return NotFound();
            var abonent = await _context.Abonents.FindAsync(id);

            if (abonent == null) return NotFound();
            var street = await _context.Streets.FindAsync(abonent.StreetCd);
            if (street == null) return NotFound();

            AbonentDTO dTO = new AbonentDTO()
            {
                AccountCd = abonent.AccountCd,
                Fio = abonent.Fio,
                StreetCd = street.StreetCd,
                StreetName = street.StreetName,
                HouseNo = abonent.HouseNo,
                FlatNo = abonent.FlatNo,
                Phone = abonent.Phone,
                Corpus = abonent.Corpus,
                District = abonent.District,
                CountPerson = abonent.CountPerson,
                SizeFlat = abonent.SizeFlat,
            };
            return dTO;
        }

        /// <summary>
        /// Обновляет значения определенного абонента
        /// </summary>
        /// <param name="dTO">Модель абонента с новыми значениями</param>
        /// <returns>Статус выполнения обновления</returns>
        [HttpPut]
        [Authorization]
        public async Task<IActionResult> PutAbonent(AbonentDTO dTO)
        {
            var abonent = await _context.Abonents.FindAsync(dTO.AccountCd);
            if (abonent == null) return NotFound();

            var street = _context.Streets.Where(s => s.StreetName == dTO.StreetName).FirstOrDefault();
            abonent.AccountCd = dTO.AccountCd;
            abonent.Fio = dTO.Fio;
            abonent.HouseNo = dTO.HouseNo;
            abonent.FlatNo = dTO.FlatNo;
            abonent.Phone = dTO.Phone;
            abonent.Corpus = dTO.Corpus;
            abonent.District = dTO.District;
            abonent.CountPerson = dTO.CountPerson;
            abonent.SizeFlat = dTO.SizeFlat;
            abonent.StreetCd = street.StreetCd;

            _context.Abonents.Update(abonent);
            await _context.SaveChangesAsync();
            return Ok(dTO);
        }

        /// <summary>
        /// Добавляет нового абонента
        /// </summary>
        /// <param name="dTO">Модель абонента с новыми значениями</param>
        /// <returns>Статус выполнения добавления</returns>
        [HttpPost]
        [Authorization]
        public async Task<ActionResult<Abonent>> PostAbonent(AbonentDTO dTO)
        {
            if (_context.Abonents == null)
                return Problem("Entity set 'BillingDbContext.Abonents'  is null.");
            Abonent abonent = new Abonent()
            {
                AccountCd = dTO.AccountCd,
                Fio = dTO.Fio,
                HouseNo = dTO.HouseNo,
                FlatNo = dTO.FlatNo,
                Phone = dTO.Phone,
                Corpus = dTO.Corpus,
                District = dTO.District,
                CountPerson = dTO.CountPerson,
                SizeFlat = dTO.SizeFlat,
            };
            var street = _context.Streets.FirstOrDefault(s => s.StreetName == dTO.StreetName);
            abonent.StreetCd = street.StreetCd;

            _context.Abonents.Add(abonent);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAbonent", new { id = abonent.AccountCd }, abonent);
        }

        /// <summary>
        /// Удаляет определенного абонента
        /// </summary>
        /// <param name="id">Лицевой счёт абонента</param>
        /// <returns>Статус выполнения удаления</returns>
        [HttpDelete("{id}")]
        [Authorization]
        public async Task<IActionResult> DeleteAbonent(string id)
        {
            if (_context.Abonents == null) return NotFound();
            var abonent = await _context.Abonents.FindAsync(id);
            if (abonent == null) return NotFound();

            var abonentModesCheck = !_context.AbonentModes.Any(am => am.AccountCd == abonent.AccountCd);

            var remainsCheck = !_context.Remains.Any(r => r.AccountCd == abonent.AccountCd);
            if (abonentModesCheck && remainsCheck)
            {
                try
                {
                    _context.Abonents.Remove(abonent);
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    
                }
            }
            return Ok();
        }
    }
}
