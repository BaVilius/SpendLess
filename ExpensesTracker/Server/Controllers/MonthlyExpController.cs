﻿using ExpensesTracker.Client.Pages;
using ExpensesTracker.Server.Data;
using ExpensesTracker.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static System.Net.WebRequestMethods;

namespace ExpensesTracker.Server.Controllers
{
    [Route("api/[controller]")] // the route will be taken from MonthlyExpController and it will read everything that is before controller
    [ApiController]
    public class MonthlyExpController : ControllerBase
    {
        private readonly DataContext context;
        static int currentCount = 0; // amomunt of times the button Order was pressed

        public MonthlyExpController(DataContext context)
        {
            this.context = context;
        }

        [HttpGet] // for swagger (api controller knows to look for get methods, swagger not so much)
        public async Task<ActionResult<List<MonthlyExp>>> GetMonthlyExps() // specify (more comfortable for swagger)
        {
            var expenses = await context.MonthlyExps.Include(e => e.Category).ToListAsync();
            //var expensesf = expenses.PickCategory(id: 2);

            return Ok(expenses); 
        }

        [HttpGet("currentCount")] // http methods should all be different, otherwise: The request matched multiple endpoints
        public async Task<ActionResult<List<MonthlyExp>>> GetOrderedMonthlyExps()
        {
            var expenses = await context.MonthlyExps.Include(e => e.Category).ToListAsync();

            //for odering
            expenses.Sort(); //ascending
            if (currentCount % 2 == 0) {
                expenses.Reverse(); //descending (have to use sort beforehand for reverse to work)
                }
            currentCount++;

            return Ok(expenses);
        }

        [HttpGet("categories")] 
        public async Task<ActionResult<List<Category>>> GetCategories() 
        {
            var categories = await context.Categories.ToListAsync();
            return Ok(categories); // everything is okay
        }

        [HttpGet("{id}")] //since we are using id as param in method, we have to specify it here as well
        public async Task<ActionResult<List<MonthlyExp>>> GetSingleExp(int id) 
        {
            var expense = await context.MonthlyExps.Include(e => e.Category).FirstOrDefaultAsync(e => e.Id == id); 

            if (expense == null) 
            {
                return NotFound("no entry..."); // error handling?
            }
            return Ok(expense);
        }
    }
}
