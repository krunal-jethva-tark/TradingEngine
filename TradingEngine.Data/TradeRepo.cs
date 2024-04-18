using TradingEngine.Domain.Models;

namespace TradingEngine.Data;

public interface ITradeRepo
{
    void AddTrade(Trade trade);
    List<Trade> GetTrades();
}

public class TradeRepo : ITradeRepo
{
    private readonly List<Trade> _trades = [];
    private readonly IOrderRepo _orderRepo;

    public TradeRepo(IOrderRepo orderRepo)
    {
        _orderRepo = orderRepo;
    }

    public void AddTrade(Trade trade)
    {
        _trades.Add(trade);
        _orderRepo.MarkOrderAsExecuted(trade.BuyOrder.Id);
        _orderRepo.MarkOrderAsExecuted(trade.SellOrder.Id);
    }

    public List<Trade> GetTrades()
    {
        return _trades;
    }

}