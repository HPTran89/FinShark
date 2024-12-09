

using api.Data;
using api.DTOs.Stock;
using api.Interfaces;
using api.Mappers;
using api.Models;
using api.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [Route("api/stock")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly IStockRepository stockRepository;

        public StockController(IStockRepository stockRepository)
        {
            this.stockRepository = stockRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync([FromQuery] QueryObject query)
        {
            if(!ModelState.IsValid) return BadRequest(ModelState);

            var stockList = await stockRepository.GetAllStocks(query);
            var result = stockList.Select(s => s.ToStockDto()).ToList();
            return Ok(result);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] int id)
        {
            Stock? stock = await stockRepository.GetStockById(id);
            if (stock == null)
            {
                return NotFound();
            } 
            return Ok(stock.ToStockDto());
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] CreateStockRequestDto createStockRequestDto)
        {
            // symbol must be unique 
            if (await stockRepository.GetStockBySymbol(createStockRequestDto.Symbol) != null)
            {
                return BadRequest("Stock already exist in the DB");
            }

            var model = createStockRequestDto.ToStockFromCreateDto();
            await stockRepository.CreateStock(model);

            // calls GetById and pass in the required parameter then it will return the model.ToStockDto() in the response
            return CreatedAtAction(nameof(GetByIdAsync), new { id = model.Id }, model.ToStockDto());
        }

        /// <summary>
        /// Use this to update request but you have to populate the whole UpdateStockRequestDto object
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="updateStockRequestDto"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync([FromRoute] int id, [FromBody] UpdateStockRequestDto updateStockRequestDto)
        {

            var stockForUpdate = await stockRepository.GetStockById(id);
            if (stockForUpdate == null)
            {
                return NotFound();
            }
            await stockRepository.UpdateStock(stockForUpdate, updateStockRequestDto);

            return Ok(stockForUpdate.ToStockDto());
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteByIdAsync([FromRoute] int id) 
        {
            var modelForDelete = await stockRepository.GetStockById(id);
            if(modelForDelete == null)
            {
                return BadRequest();
            }

            await stockRepository.DeleteStock(modelForDelete);
            return Ok() ;
        
        }


    }
}
