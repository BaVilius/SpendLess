using ExpensesTracker.Client.Pages;
using ExpensesTracker.Server.Data;
using ExpensesTracker.Server.Repositories.Interfaces;
using ExpensesTracker.Server.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Transactions;
using static ExpensesTracker.Shared.Extensions.Delegates;
using static System.Net.WebRequestMethods;

namespace ExpensesTracker.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StocksController : ControllerBase
    {
        private readonly IStock _stocks;

        public StocksController(IStock _stocks)
        {
            this._stocks = _stocks;
        }

        [HttpGet]
        public async Task<ActionResult<List<Stock>>> GetStocks()
        {
            return Ok(await GetFilteredStocks());
        }

        [HttpGet("currentCount")] 
        public async Task<ActionResult<List<Stock>>> GetOrderedStocks()
        {
            return Ok(await GetFilteredStocks());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Stock>> GetSingleStock(int id)
        {
            var stock = await _stocks.GetSingleStockAsync(id);

            if (stock == null)
            {
                return NotFound("no entry...");
            }
            return Ok(stock.Value);
        }

        [HttpPost("Add")]
        public async Task<ActionResult<List<Stock>>> CreateStock(Stock stock)
        {
            await _stocks.CreateStockAsync(stock);
            return Ok(await GetFilteredStocks());
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<List<Stock>>> DeleteStock(int id)
        {
            var dbStock = await _stocks.GetSingleStockAsync(id);
            if (dbStock == null)
                return NotFound("There is no such stock ");


            if(dbStock.Value == null)
            {
				return Ok(await GetFilteredStocks());
			}
            await _stocks.DeleteStockAsync(dbStock.Value);

            return Ok(await GetFilteredStocks());
        }

        async Task<List<Stock>> GetFilteredStocks()
        {
            return await _stocks.GetStocksAsync();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<List<Stock>>> UpdateStock(Stock stock, int id)
        {
            var dbStock = await _stocks.GetSingleStockAsync(id);
            if (dbStock == null)
                return NotFound("Sorry, but no stock for you");

            await _stocks.UpdateStockAsync(stock, dbStock.Value);

            return Ok(await GetFilteredStocks());
        }
    }
}