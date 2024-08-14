using Microsoft.EntityFrameworkCore;
using Pure.Application.Dtos.Stock;
using Pure.Application.Interfaces;
using Pure.Domain.Models;
using Pure.Infrastructure.Context;


namespace Pure.Infrastructure.Repository
{
    public class StockRepository : IStockRepository
    {
        private readonly ApplicationDbContext _context;
        public StockRepository(ApplicationDbContext dbContext)
        {
            _context = dbContext;
        }

        public async Task<Stock> CreateAsync(Stock stockModel)
        {
            await _context.AddAsync(stockModel);
            await _context.SaveChangesAsync();
            return stockModel;
        }

        public async Task<Stock?> DeleteAsync(int id)
        {
            var stockModel = await _context.Stock.FirstOrDefaultAsync(x => x.Id == id);

            if(stockModel == null) return null;

            _context.Stock.Remove(stockModel);
            await _context.SaveChangesAsync();
            return stockModel;
         }

        public async Task<List<Stock>> GetAllAsync()
        {
            return await _context.Stock.Include(c => c.Comments).AsNoTracking().ToListAsync();
        }

        public async Task<Stock?> GetByIdAsync(int id)
        {
            return await _context.Stock.Include(c => c.Comments).FirstOrDefaultAsync(x => x.Id == id);
        }

        public Task<bool> StockExists(int id)
        {
            return _context.Stock.AnyAsync(x => x.Id == id);
        }

        public async Task<Stock?> UpdateAsync(int id, UpdateStockRequestDto stockDto)
        {
            var existingStock = await _context.Stock.FirstOrDefaultAsync(x => x.Id == id);

            if (existingStock == null) return null;

            existingStock.Symbol = stockDto.Symbol;
            existingStock.CompanyName = stockDto.CompanyName;
            existingStock.Purchase = stockDto.Purchase;
            existingStock.LastDiv = stockDto.LastDiv;
            existingStock.Industry = stockDto.Industry;
            existingStock.MarketCap = stockDto.MarketCap;

            await _context.SaveChangesAsync();
            return existingStock;
        }
    }
}
