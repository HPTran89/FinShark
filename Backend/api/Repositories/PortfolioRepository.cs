using api.Data;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repositories
{
    public class PortfolioRepository : IPortfolioRepository
    {
        private readonly ApplicationDBContext dbContext;

        public PortfolioRepository(ApplicationDBContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Portfolio> CreateAsync(Portfolio portfolio)
        {
            await dbContext.Portfolios.AddAsync(portfolio);
            await dbContext.SaveChangesAsync();
            return portfolio;
        }

        public async Task<Portfolio> DeletePortfolio(AppUser user, string symbol)
        {
            var portfolioModel = await dbContext.Portfolios.FirstOrDefaultAsync(x => x.AppUserId == user.Id && x.Stock.Symbol.ToLower() == symbol.ToLower());

            if (portfolioModel == null) { return null;  }

            dbContext.Portfolios.Remove(portfolioModel);
            await dbContext.SaveChangesAsync();
            return portfolioModel;
        }

        public async Task<List<Stock>> GetUserPorfolio(AppUser user)
        {
            return await dbContext.Portfolios.Where(u => u.AppUserId == user.Id).Select(s => new Stock
            {
                Id = s.StockId,
                Symbol = s.Stock.Symbol,
                CompanyName = s.Stock.CompanyName,
                Purchase = s.Stock.Purchase,
                LastDiv = s.Stock.LastDiv,
                Industry = s.Stock.Industry,
                MarketCap = s.Stock.MarketCap,
            }).ToListAsync();
        }
    }
}
