using System.Collections.Generic;
using System.Linq;
using WebAPI.Domain.Entities;

namespace WebAPI.Infrastructure
{
    public class DataSeeder
    {
        public static List<Stock> StockFeed = new List<Stock>()
                {
                    new Stock("nwg", "NatWest Group plc ", 0M),
                    new Stock("vod", "Vodafone Group PLC", 0M),
                    new Stock("lloy", "Lloyds Banking Group plc", 0M),
                    new Stock("barc", "Barclays plc",0M),
                    new Stock("shel", "Shell", 0M)
                };
    public static void Seed(AppDbContext appDbContext)
        {
            if (!appDbContext.Stocks.Any())
            {
                appDbContext.Stocks.AddRange(StockFeed);
                appDbContext.SaveChanges();
            }
        }
    }
}