using TradingEngine.Data;
using TradingEngine.Domain.Enums;
using TradingEngine.Domain.Models;

namespace TradingEngine;

public interface ITradingService
{
    void CheckForTrades(Order order);
    List<Trade> ExecuteTrades();
    List<Trade> GetAllTrades();
}

public class TradingService : ITradingService
{
    private readonly IOrderRepo _orderRepo;
    private readonly ITradeRepo _tradeRepo;

    public TradingService(IOrderRepo orderRepo, ITradeRepo tradeRepo)
    {
        _orderRepo = orderRepo;
        _tradeRepo = tradeRepo;
    }

    public void CheckForTrades(Order order)
    {
        var currentOrders = order.Type switch
        {
            OrderType.Bid => _orderRepo.GetOpenSellOrders().ToList(),
            OrderType.Offer => _orderRepo.GetOpenBuyOrders().ToList(),
            _ => throw new ArgumentOutOfRangeException()
        };
        
        var matchingTradeOrder =
            currentOrders.FirstOrDefault(o => o.StockSymbol == order.StockSymbol && (order.Type == OrderType.Bid ? o.Price <= order.Price : o.Price > order.Price)); // fix this condition

        if (matchingTradeOrder == null) return;
        
        var buyOrder = order.Type == OrderType.Bid ? order : matchingTradeOrder;
        var sellOrder = order.Type == OrderType.Bid ? matchingTradeOrder : order;
        
        // hanle multiple sell senrios and all
        var tradePrice = order.Type == OrderType.Bid ? order.Price : matchingTradeOrder.Price; // always execute order at buying price
        const int tradeQuantity = 1;
        var trade = new Trade
        {
            BuyOrder = buyOrder,
            SellOrder = sellOrder,
            Price = tradePrice,
            Quantity = tradeQuantity,
        };
        _tradeRepo.AddTrade(trade);
    }
    
    public List<Trade> ExecuteTrades()
    {
        var trades = new List<Trade>();

        // foreach (var buyOrder in _buyOrders.ToList())
        // {
        //     var matchingSellOrder = _sellOrders.FirstOrDefault(o => o.StockSymbol == buyOrder.StockSymbol
        //                                                             && o.Price <= buyOrder.Price);
        //
        //     if (matchingSellOrder != null)
        //     {
        //         var tradePrice = matchingSellOrder.Price;
        //         const int tradeQuantity = 1; // Assuming one share per trade for simplicity
        //         var trade = new Trade
        //         {
        //             BuyOrder = buyOrder,
        //             SellOrder = matchingSellOrder,
        //             Price = tradePrice,
        //             Quantity = tradeQuantity
        //         };
        //
        //         trades.Add(trade);
        //         _buyOrders.Remove(buyOrder);
        //         _sellOrders.Remove(matchingSellOrder);
        //     }
        // }

        return trades;
    }

    public List<Trade> GetAllTrades()
    {
        return _tradeRepo.GetTrades();
    }
}