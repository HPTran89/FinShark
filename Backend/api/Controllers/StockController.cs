

using api.Data;
using api.DTOs.Stock;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [Route("api/stock")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly ApplicationDBContext applicationDBContext;

        public StockController(ApplicationDBContext applicationDBContext)
        {
            this.applicationDBContext = applicationDBContext;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var stockList = applicationDBContext.Stocks.ToList().Select(s => s.ToStockDto());

            return Ok(stockList);
        }

        [HttpGet("GetAllAsync")]
        public async Task<IActionResult> GetAllAsync()
        {
            var stockList = await applicationDBContext.Stocks.ToListAsync();
            var result = stockList.Select((s => s.ToStockDto()));

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] int id)
        {
            Stock? stock = await applicationDBContext.Stocks.FindAsync(id);
            if (stock == null)
            {
                return NotFound();
            }
            return Ok(stock.ToStockDto());
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] CreateStockRequestDto createStockRequestDto)
        {
            var model = createStockRequestDto.ToStockFromCreateDto();
            // symbol must be unique 
            if(await applicationDBContext.Stocks.Where(s => s.Symbol.ToLower() ==  model.Symbol.ToLower()).AnyAsync())
            {
                return BadRequest("Symbol already exist in the DB");
            }
            await applicationDBContext.Stocks.AddAsync(model);
            await applicationDBContext.SaveChangesAsync();
            // calls GetById and pass in the required parameter then it will return the model.ToStockDto() in the response
            return CreatedAtAction(nameof(GetByIdAsync), new { id = model.Id }, model.ToStockDto());
        }

        /// <summary>
        /// Use this to update request but you have to populate the whole UpdateStockRequestDto object
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="updateStockRequestDto"></param>
        /// <returns></returns>
        [HttpPut("{symbol}")]
        public async Task<IActionResult> UpdateAsync([FromRoute] string symbol, [FromBody] UpdateStockRequestDto updateStockRequestDto)
        {

            var modelForUpdate = await applicationDBContext.Stocks.Where(x => x.Symbol.ToLower() == symbol.ToLower()).FirstOrDefaultAsync();
            if (modelForUpdate == null)
            {
                return NotFound();
            }

            modelForUpdate.Symbol = updateStockRequestDto.Symbol;
            modelForUpdate.CompanyName = updateStockRequestDto.CompanyName;
            modelForUpdate.Purchase = updateStockRequestDto.Purchase;
            modelForUpdate.Industry = updateStockRequestDto.Industry;
            modelForUpdate.LastDiv = updateStockRequestDto.LastDiv;
            modelForUpdate.MarketCap = updateStockRequestDto.MarketCap;
            
            await applicationDBContext.SaveChangesAsync();
            return Ok(modelForUpdate.ToStockDto());
        }

        /// <summary>
        /// Use this method to only pass in specfic field(s) for upate
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="updateStockRequestDto"></param>
        /// <returns></returns>
        //[HttpPatch("{symbol}")]
        //public IActionResult Patch([FromRoute] string symbol, [FromBody] UpdateStockRequestDto updateStockRequestDto)
        //{

        //    var modelForUpdate = applicationDBContext.Stocks.Where(x => x.Symbol.ToLower() == symbol.ToLower()).FirstOrDefault();
        //    if (modelForUpdate == null)
        //    {
        //        return NotFound();
        //    }

        //    modelForUpdate.Symbol = updateStockRequestDto.Symbol;
        //    modelForUpdate.CompanyName = updateStockRequestDto.CompanyName;
        //    modelForUpdate.Purchase = updateStockRequestDto.Purchase;
        //    modelForUpdate.Industry = updateStockRequestDto.Industry;
        //    modelForUpdate.LastDiv = updateStockRequestDto.LastDiv;
        //    modelForUpdate.MarketCap = updateStockRequestDto.MarketCap;

        //    applicationDBContext.SaveChanges();
        //    return Ok(modelForUpdate.ToStockDto());
        //}

        [HttpDelete("{symbol}")]
        public async Task<IActionResult> DeleteByIdAsync([FromRoute] string symbol) 
        {
            var modelForDelete = await applicationDBContext.Stocks.Where(s => s.Symbol.ToLower() == symbol.ToLower()).FirstOrDefaultAsync();
            if(modelForDelete == null)
            {
                return BadRequest();
            }

            applicationDBContext.Remove(modelForDelete);
            await applicationDBContext.SaveChangesAsync();

            return Ok();
        
        }


    }
}
