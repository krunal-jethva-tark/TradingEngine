namespace TradingEngine.Domain.Models;

public class Trade
{
    public Order BuyOrder { get; set; }
    public Order SellOrder { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
}