//using DocumentFormat.OpenXml.Bibliography;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Globalization;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ExpensesTracker.Client.Services.StockService
{
    public  class StockService : IStockService
    {
        private readonly HttpClient http;
        private readonly NavigationManager navigationManager;

        public List<Stock> AllStocks { get; set; } = new List<Stock>();
        public Stock stockFilter { get; set; } = new Stock();
        
		public  Stock singleStock { get; set; } = new Stock()
		{
			Price = 100,
		};


		public StockService(HttpClient http, NavigationManager navigationManager)
        {
            this.http = http;
            this.navigationManager = navigationManager;
        }

        public async Task CreateStock(Stock stock)
        {
            var result = await http.PostAsJsonAsync("/api/stocks/Add", stock);
            await SetResults(result);
            navigationManager.NavigateTo("/stocks");
        }

        public async Task DeleteStock(int id)
        {
            var result = await http.DeleteAsync($"api/stocks/{id}");

            await SetResults(result);
        }

        public async Task GetStocks()
        {
            var result = await http.GetFromJsonAsync<List<Stock>>("api/stocks");
            if (result != null)
            {
                AllStocks = result;
            }
        }

        public async Task<Stock> GetSingleStock(int id)
        {
            var result = await http.GetFromJsonAsync<Stock>($"api/stocks/{id}");
            if (result != null)
            {
                return result;
            }

            return null;
        }

        public async Task<Stock> SearchStock(string query)
        {
            using (var client = new HttpClient())
            {
                // polygon key UKGyjYInN20Qt_dPwO9OmLy3e6SG4UWM

                // alphavantage key T1VG084KZLCERV3K

                //client.BaseAddress = new Uri("https://api.polygon.io/v1/open-close/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //GET Method
                HttpResponseMessage response = await client.GetAsync($"https://api.polygon.io/v2/aggs/ticker/{query}/prev?adjusted=true&apiKey=UKGyjYInN20Qt_dPwO9OmLy3e6SG4UWM");
                if (response != null)
                {
					if (response.IsSuccessStatusCode)
					{
						string responseBody = await response.Content.ReadAsStringAsync();
						var data = (JObject)JsonConvert.DeserializeObject(responseBody);

                        var supposedPrice = data.SelectToken("results[0].h");
                        if(supposedPrice != null)
                        {
							var price = supposedPrice.Value<double>();
							return new Stock()
							{
								Id = -1,
								Price = price,
								Title = query,
								Ticker = query,
							};
						}
					}
				}
                var stock = new Stock()
                {
                    Id= -1,
                    Price = -1,
                    Title = "",
                    Ticker = "",
                };
                return stock;
            }
        }

        public async Task UpdateStock(Stock stock)
        {
            var result = await http.PutAsJsonAsync($"api/stocks/{stock.Id}", stock);
            await SetResults(result);
            navigationManager.NavigateTo("stocks");
        }

        private async Task SetResults(HttpResponseMessage result)
        {
            var response = await result.Content.ReadFromJsonAsync<List<Stock>>();
            AllStocks = response;
        }
    }
}
