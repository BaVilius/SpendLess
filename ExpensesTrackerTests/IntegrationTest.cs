using ExpensesTracker.Server.Data;
using Microsoft.AspNetCore.Mvc.Testing;

namespace ExpensesTrackerTests
{
	[TestClass]
	public class IntegrationTest
	{

		[TestMethod]
		public async Task GetAllTest()
		{
			var webAppFactory = new WebApplicationFactory<Program>();

			var client = webAppFactory.CreateClient();

			var result = await client.GetAsync("api/incomes");

			Assert.IsTrue(result != null);
		}
	}
}
