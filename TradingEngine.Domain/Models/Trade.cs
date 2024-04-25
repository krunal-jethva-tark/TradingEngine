namespace TradingEngine.Domain.Models;

public class Trade
{
    protected bool Equals(Trade other)
    {
        return Price == other.Price && Quantity == other.Quantity && StockSymbol == other.StockSymbol
            && BuyOrder.Id == other.BuyOrder.Id && SellOrder.Id == other.SellOrder.Id;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((Trade)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Price, Quantity, StockSymbol, BuyOrder.Id, SellOrder.Id);
    }

    public Order BuyOrder { get; set; }
    public Order SellOrder { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public string StockSymbol { get; set; }
}