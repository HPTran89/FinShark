﻿using api.Data;
using api.DTOs.Stock;
using api.Interfaces;
using api.Mappers;
using api.Models;
using api.Utility;
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


        public async Task<List<Stock>> GetAllStocks(QueryObject query)
        {
            var stocks = context.Stocks.Include(c => c.Comments).ThenInclude(a => a.AppUser).AsQueryable();
            //fitlering
            if(!string.IsNullOrWhiteSpace(query.CompanyName))
            {
                stocks = stocks.Where(s => s.CompanyName.Contains(query.CompanyName));
            }
            if (!string.IsNullOrWhiteSpace(query.Symbol))
            {
                stocks = stocks.Where(s => s.Symbol.Contains(query.Symbol));
            }

            //sorting
            if(!string.IsNullOrWhiteSpace(query.SortBy)) 
            {
                if(query.SortBy.Equals("Symbol", StringComparison.OrdinalIgnoreCase))
                {
                    stocks = query.IsDesecending ? stocks.OrderByDescending(s => s.Symbol) : stocks.OrderBy(s => s.Symbol);
                }
            }

            //Pagination
            var skipNumber = (query.PageNumber - 1) * query.PageSize;

            return await stocks.Skip(skipNumber).Take(query.PageSize).ToListAsync();

        }

        public async Task<Stock?> GetStockById(int id)
        {
            var stock = await context.Stocks.Include(c => c.Comments).FirstOrDefaultAsync(i => i.Id == id);

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
