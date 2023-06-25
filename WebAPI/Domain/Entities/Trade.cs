using System;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.Domain.Entities
{
    public class Trade
    {
        [Key]
        public int Id { get; set; }
        public string Symbol { get; set; }
        public decimal PriceInPound { get; set; }
        public decimal NumberOfShares { get; set; }
        public int BrokerId { get; set; }
        public DateTimeOffset? CreatedDate { get; set; } = DateTimeOffset.UtcNow;

        public Trade() { }
    }
}