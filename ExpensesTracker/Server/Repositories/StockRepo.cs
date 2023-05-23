using ExpensesTracker.Server.Data;
using Microsoft.AspNetCore.Mvc;

namespace ExpensesTracker.Server.Repositories
{
    public class StockRepo : IStock
    {
        private readonly DataContext context;

        public StockRepo(DataContext context)
        {
            this.context = context;
        }

        public async Task<ActionResult<Stock>> CreateStockAsync(Stock stock)
        {
            context.AllStocks.Add(stock);
            await context.SaveChangesAsync();
            return stock;
        }

        public async Task<ActionResult<Stock>> DeleteStockAsync(Stock stock)
        {
            context.AllStocks.Remove(stock);
            await context.SaveChangesAsync();

            return stock;
        }

        public async Task<List<Stock>> GetStocksAsync()
        {
            return await context.AllStocks.ToListAsync();
        }

        public async Task<ActionResult<Stock?>> GetSingleStockAsync(int id)
        {
            return await context.AllStocks.FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<Stock> UpdateStockAsync(Stock stock, Stock dbStock)
        {
            dbStock.Title = stock.Title;
            dbStock.Price = stock.Price;
            dbStock.Ticker = stock.Ticker;
            dbStock.Id = stock.Id;

            await context.SaveChangesAsync();

            return stock;
        }
    }
}
