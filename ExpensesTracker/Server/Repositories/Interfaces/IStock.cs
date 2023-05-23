using ExpensesTracker.Server.Data;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace ExpensesTracker.Server.Repositories.Interfaces
{
    public interface IStock
    {
        Task<List<Stock>> GetStocksAsync();
        Task<ActionResult<Stock?>> GetSingleStockAsync(int id);
        Task<ActionResult<Stock>> CreateStockAsync(Stock stock);
        Task<ActionResult<Stock>> DeleteStockAsync(Stock stock);
        Task<Stock> UpdateStockAsync(Stock stock, Stock dbStock);
    }
}