using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using NachislService.DTO;
using NachislService.Helpers;
using NachislService.Repository;
using NachislService.Repository.Models;

namespace NachislService.Controllers
{
    [Route("api/nachislenie/[controller]")]
    [ApiController]
    public class PricesController : ControllerBase
    {
        private readonly BillingDbContext _context;

        public PricesController(BillingDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Получает все цены
        /// </summary>
        /// <returns>Возвращает все цены с указанием значений из родительских таблиц</returns>
        [HttpGet]
        [Authorization]
        public async Task<ActionResult<IEnumerable<PriceDTO>>> GetPrices()
        {
            if (_context.Prices == null) return NotFound();
            List<PriceDTO> priceDTOs = new List<PriceDTO>();
            PriceDTO dTO;

            var appInformationToReturn = _context.Prices
                .Join(_context.Modes,
                p => p.ModeCd,
                m => m.ModeCd,
                (p, m) => new
                {
                    PriceCd = p.PriceCd,
                    PriceValue = p.PriceValue,
                    ModeCd = m.ModeCd,
                    ModeName = m.ModeName
                });

            foreach (var item in appInformationToReturn)
            {
                dTO = new PriceDTO()
                {
                    PriceCd = item.PriceCd,
                    PriceValue = item.PriceValue,
                    ModeCd = item.ModeCd,
                    ModeName = item.ModeName
                };
                priceDTOs.Add(dTO);
            }
            return priceDTOs;
        }

        /// <summary>
        /// Получает определенную цену
        /// </summary>
        /// <param name="id">Код цены</param>
        /// <returns>Объект цены с указанием значений из родительских таблиц</returns>
        [HttpGet("{id}")]
        [Authorization]
        public async Task<ActionResult<PriceDTO>> GetPrice(int id)
        {
          if (_context.Prices == null) return NotFound();

            var price = await _context.Prices.FindAsync(id);
            if (price == null) return NotFound();

            var mode = await _context.Modes.FindAsync(price.ModeCd);

            if (mode == null) return NotFound();

            PriceDTO dTO = new PriceDTO()
            {
                PriceCd = price.PriceCd,
                PriceValue = price.PriceValue,
                ModeCd = mode.ModeCd,
                ModeName = mode.ModeName
            };

            return dTO;
        }

        /// <summary>
        /// Обновляет значения определенной цены
        /// </summary>
        /// <param name="dTO">Модель цены с новыми значениями</param>
        /// <returns>Статус выполнения обновления</returns>
        [HttpPut]
        [Authorization]
        public async Task<IActionResult> PutPrice(PriceDTO dTO)
        {
            var price = await _context.Prices.FindAsync(dTO.PriceCd);
            string cleanModeName = Regex.Replace(dTO.ModeName, @"\s*\[.*?\]\s*", "");
            var mode = _context.Modes.Where(m => m.ModeName == cleanModeName).FirstOrDefault();
            if (price == null || mode == null) return NotFound();
            price.PriceValue = dTO.PriceValue;
            price.ModeCd = mode.ModeCd;
            _context.Prices.Update(price);
            await _context.SaveChangesAsync();
            return Ok(dTO);
        }

        /// <summary>
        /// Добавляет новую цену
        /// </summary>
        /// <param name="dTO">Модель цены с новыми значениями</param>
        /// <returns>Статус выполнения добавления</returns>
        [HttpPost]
        [Authorization]
        public async Task<ActionResult<Price>> PostPrice(PriceDTO dTO)
        {
            if (_context.Prices == null)
                return Problem("Entity set 'BillingDbContext.Prices'  is null.");

            Price price = new Price();
            price.PriceValue = dTO.PriceValue;
            string cleanModeName = Regex.Replace(dTO.ModeName, @"\s*\[.*?\]\s*", "");
            var mode = _context.Modes.FirstOrDefault(m => m.ModeName == cleanModeName);
            price.ModeCd = mode.ModeCd;

            _context.Prices.Add(price);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPrice", new { id = price.PriceCd }, price);
        }

        /// <summary>
        /// Удаляет определенную цену
        /// </summary>
        /// <param name="id">Код цены</param>
        /// <returns>Статус выполнения удаления</returns>
        [HttpDelete("{id}")]
        [Authorization]
        public async Task<IActionResult> DeletePrice(int id)
        {
            if (_context.Prices == null) return NotFound();
            
            var price = await _context.Prices.FindAsync(id);
            if (price == null) return NotFound();  

            _context.Prices.Remove(price);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}