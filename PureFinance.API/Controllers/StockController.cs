using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pure.Application.Dtos.Stock;
using Pure.Application.Interfaces;
using Pure.Application.Mappers;
using Pure.Infrastructure.Context;

namespace PureFinance.API.Controllers
{
    [Route("api/stock")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly IStockRepository _stockRepo;

        public StockController(IStockRepository stockRepository)
        {
            _stockRepo = stockRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var stocks = await _stockRepo.GetAllAsync();

            var stockDto = stocks.Select(x => x.ToStockDto());

            return Ok(stocks);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id) 
        { 
            var stock = await _stockRepo.GetByIdAsync(id);

            if(stock == null) return NotFound();

            return Ok(stock.ToStockDto());
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateStockRequestDto sotckDto)
        {
            var stockModel = sotckDto.ToStockFromCreateDTO();
            await _stockRepo.CreateAsync(stockModel);
            return CreatedAtAction(nameof(GetById), new {id = stockModel.Id }, stockModel.ToStockDto());
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStockRequestDto updateDto)
        {
            var stockModel = await _stockRepo.UpdateAsync(id, updateDto);

            if(stockModel == null) return NotFound();

            return Ok(stockModel.ToStockDto());
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var stockModel = await _stockRepo.DeleteAsync(id);

            if(stockModel == null) return NotFound();    

            return NoContent();
        }
    }
}
