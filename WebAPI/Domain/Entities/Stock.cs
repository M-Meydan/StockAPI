using System.ComponentModel.DataAnnotations;

namespace WebAPI.Domain.Entities
{
    public class Stock
    {
        public Stock(string symbol, string companyName, decimal currentPrice)
        {
            Symbol = symbol;
            CompanyName = companyName;
            CurrentPrice = currentPrice;
        }

        [Key]
        public int StockId { get; set; }
        public string Symbol { get; set; }
        public string CompanyName { get; set; }
        public decimal CurrentPrice { get; set; }
    }

}
