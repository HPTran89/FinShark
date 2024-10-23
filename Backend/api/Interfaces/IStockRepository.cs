using api.DTOs.Stock;
using api.Models;

namespace api.Interfaces
{
    public interface IStockRepository
    {
        Task<List<Stock>> GetAllStocks();
        Task<Stock?> GetStockById(int id);
        Task<Stock> CreateStock(Stock stock);
        Task<Stock?> GetStockBySymbol(string symbol);

        Task<Stock> UpdateStock(Stock forUpdate, UpdateStockRequestDto updates);
        Task DeleteStock(Stock forDelete);
    }

}
