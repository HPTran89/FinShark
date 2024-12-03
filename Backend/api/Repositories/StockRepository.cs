using api.Data;
using api.DTOs.Stock;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repositories
{
    public class StockRepository : IStockRepository
    {
        private readonly ApplicationDBContext context;

        public StockRepository(ApplicationDBContext context)
        {
            this.context = context;
        }


        public async Task<List<Stock>> GetAllStocks()
        {
            var stocks = await context.Stocks.ToListAsync();
            return stocks;

        }

        public async Task<Stock?> GetStockById(int id)
        {
            var stock = await context.Stocks.FindAsync(id);

            return stock;
        }

        public async Task<Stock> CreateStock(Stock stock)
        {

            await context.Stocks.AddAsync(stock);
            await context.SaveChangesAsync();
            return stock;

        }

        public async Task<Stock?> GetStockBySymbol(string symbol)
        {
            var stock = await context.Stocks.Where(x => x.Symbol.ToLower() == symbol).FirstOrDefaultAsync();

            return stock;
        }

        public async Task<Stock> UpdateStock(Stock forUpdate, UpdateStockRequestDto updates)
        {


            forUpdate.Symbol = updates.Symbol;
            forUpdate.CompanyName = updates.CompanyName;
            forUpdate.Purchase = updates.Purchase;
            forUpdate.Industry = updates.Industry;
            forUpdate.LastDiv = updates.LastDiv;
            forUpdate.MarketCap = updates.MarketCap;

            await context.SaveChangesAsync();
            return forUpdate;
        }

        public async Task DeleteStock(Stock forDelete)
        {
            context.Remove(forDelete);
            await context.SaveChangesAsync();
        }
    }
}
