namespace ExpensesTracker.Client.Services.StockService
{
    public interface IStockService
    {
        Stock singleStock { get; set; }
        List<Stock> AllStocks { get; set; }
        Task GetStocks();
        Task<Stock> GetSingleStock(int id);
        Task CreateStock(Stock stock);
        Task UpdateStock(Stock stock);
        Task DeleteStock(int id);
        Task<Stock> SearchStock(string query);
    }
}

