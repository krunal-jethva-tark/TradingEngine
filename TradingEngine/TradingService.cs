using TradingEngine.Data;
using TradingEngine.Domain.Enums;
using TradingEngine.Domain.Models;

namespace TradingEngine;

public interface ITradingService
{
    void CheckForTrades(Order order);
    List<Trade> GetAllTrades();
}

public class TradingService(IOrderRepo orderRepo, ITradeRepo tradeRepo) : ITradingService
{
    public void CheckForTrades(Order order)
    {

        var matchingTradeOrders = GetMatchingOrdersForCurrentOrder(order);

        foreach (var matchingTradeOrder in matchingTradeOrders)
        {
            var buyOrder = order.Type == OrderType.Bid ? order : matchingTradeOrder;
            var sellOrder = order.Type == OrderType.Bid ? matchingTradeOrder : order;
        
            var tradeQuantity = Math.Min(buyOrder.Quantity, sellOrder.Quantity);
            if (tradeQuantity <= 0)
                continue;
            
            var trade = new Trade
            {
                BuyOrder = buyOrder,
                SellOrder = sellOrder,
                Price = matchingTradeOrder.Price,
                Quantity = tradeQuantity,
                StockSymbol = matchingTradeOrder.StockSymbol
            };
            
            tradeRepo.AddTrade(trade);

            buyOrder.Quantity -= tradeQuantity;
            sellOrder.Quantity -= tradeQuantity;
            
            if (sellOrder.Quantity == 0 || buyOrder.Quantity == 0) continue;
        }
    }

    private IEnumerable<Order> GetMatchingOrdersForCurrentOrder(Order order)
    {
        var currentOrders = order.Type switch
        {
            OrderType.Bid => orderRepo.GetOpenSellOrders().ToList(),
            OrderType.Offer => orderRepo.GetOpenBuyOrders().ToList(),
            _ => throw new ArgumentOutOfRangeException()
        };
        
        return currentOrders.Where(o =>
            o.StockSymbol == order.StockSymbol &&
            (order.Type == OrderType.Bid ? o.Price <= order.Price : o.Price > order.Price));
    }

    public List<Trade> GetAllTrades()
    {
        return tradeRepo.GetTrades();
    }
}