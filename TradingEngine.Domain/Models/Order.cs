using TradingEngine.Domain.Enums;

namespace TradingEngine.Domain.Models;

public class Order
{
    public string Id { get; set; }
    public string UserId { get; set; }
    public string StockSymbol { get; set; }
    public int Quantity { get; set; } = 1;
    public decimal Price { get; set; }
    public OrderType Type { get; set; }
    public DateTime Timestamp { get; set; }
    public OrderStatus Status { get; set; }
}