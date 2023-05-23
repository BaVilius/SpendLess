using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpensesTracker.Shared.Models
{
    public class Stock
    {
        public int Id { get; set; }

        public string Ticker { get; set; }

        public string Title { get; set; }

        public double Price { get; set; }
    }
}
