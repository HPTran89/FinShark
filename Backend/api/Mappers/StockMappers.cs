using api.DTOs.Stock;
using api.Models;

namespace api.Mappers
{   
    /// <summary>
    /// Our own mapper.
    /// </summary>
    public static class StockMappers
    {
        // this is an extension method so it available through out the code
        // TO-DO: learn more about extension method in C#
        public static StockDto ToStockDto(this Stock model) 
        {
            return new StockDto
            {
                Id = model.Id,
                Symbol = model.Symbol,
                CompanyName = model.CompanyName,
                Purchase = model.Purchase,
                LastDiv = model.LastDiv,
                Industry = model.Industry,
                MarketCap = model.MarketCap,
                Comments = model.Comments.Select(s => s.ToCommentDto()).ToList(),
            };
        }

        public static Stock ToStockFromCreateDto(this CreateStockRequestDto createStockRequestDto) 
        {
            return new Stock
            {

                Symbol = createStockRequestDto.Symbol,
                CompanyName = createStockRequestDto.CompanyName,
                Purchase = createStockRequestDto.Purchase,
                LastDiv = createStockRequestDto.LastDiv,
                Industry = createStockRequestDto.Industry,
                MarketCap = createStockRequestDto.MarketCap,
            };
        }


    }
}
