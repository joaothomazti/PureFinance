using System.ComponentModel.DataAnnotations.Schema;

namespace Pure.Domain.Models
{
    public class Stock
    {
        public int Id { get; set; }
        public string Symbol { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;
        [Column(TypeName = "numeric(18,2)")]
        public decimal Purchase { get; set; }
        [Column(TypeName = "numeric(18,2)")]
        public decimal LastDiv { get; set; }
        public string Industry { get; set; } = string.Empty;
        public long MarketCap { get; set; }

        public List<Comment> Comments { get; set; } = new List<Comment>();
    }
}
