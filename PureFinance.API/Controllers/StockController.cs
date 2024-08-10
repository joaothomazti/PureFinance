using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pure.Application.Dtos.Stock;
using Pure.Application.Mappers;
using Pure.Infrastructure.Context;

namespace PureFinance.API.Controllers
{
    [Route("api/stock")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public StockController(ApplicationDbContext dbContext)
        {
            _context = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var stocks = await _context.Stock.ToListAsync();

            var stockDto = stocks.Select(x => x.ToStockDto());

            return Ok(stocks);
        }

        [HttpGet("{id}")]
        public  IActionResult GetById([FromRoute] int id) 
        { 
            var sotck = _context.Stock.Find(id);

            if(sotck == null) return NotFound();

            return Ok(sotck.ToStockDto());
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateStockRequestDto sotckDto)
        {
            var stockModel = sotckDto.ToStockFromCreateDTO();

            await _context.Stock.AddAsync(stockModel);

            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new {id = stockModel.Id }, stockModel.ToStockDto());
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStockRequestDto updateDto)
        {
            var stockModel = await _context.Stock.FirstOrDefaultAsync(x => x.Id == id);

            if(stockModel == null) return NotFound();

            stockModel.Symbol = updateDto.Symbol;
            stockModel.CompanyName = updateDto.CompanyName;
            stockModel.Purchase = updateDto.Purchase;
            stockModel.LastDiv = updateDto.LastDiv;
            stockModel.Industry = updateDto.Industry;
            stockModel.MarketCap = updateDto.MarketCap;

            await _context.SaveChangesAsync();

            return Ok(stockModel.ToStockDto());
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var stockModel = await _context.Stock.FirstOrDefaultAsync(x => x.Id == id);

            if(stockModel == null) return NotFound();    

            _context.Stock.Remove(stockModel);

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
