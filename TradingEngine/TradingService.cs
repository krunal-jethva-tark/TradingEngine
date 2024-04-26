using TradingEngine.Data;
using TradingEngine.Domain.Enums;
using TradingEngine.Domain.Models;

namespace TradingEngine;

public interface ITradingService
{
    void TryToExecuteTrades(Order order);
    List<Trade> GetAllTrades();
}

public class TradingService(IOrderRepo orderRepo, ITradeRepo tradeRepo) : ITradingService
{
    public void TryToExecuteTrades(Order order)
    {

        var matchingTradeOrders = GetMatchingOrdersForCurrentOrder(order);

        foreach (var matchingTradeOrder in matchingTradeOrders)
        {
            ExecuteTrade(order, matchingTradeOrder);
        }
    }
    
    private IEnumerable<Order> GetMatchingOrdersForCurrentOrder(Order order)
    {
        return order.Type switch
        {
            OrderType.Bid => orderRepo.GetOpenSellOrders()
                .Where(o => o.Id != order.Id && o.UserId != order.UserId && o.StockSymbol == order.StockSymbol && o.Price <= order.Price && o.Timestamp < order.Timestamp),
            OrderType.Offer => orderRepo.GetOpenBuyOrders()
                .Where(o => o.Id != order.Id && o.UserId != order.UserId && o.StockSymbol == order.StockSymbol && o.Price >= order.Price && o.Timestamp < order.Timestamp),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private void ExecuteTrade(Order order, Order matchingTradeOrder)
    {
        var (buyOrder, sellOrder) = GetBuyAndSellOrders(order, matchingTradeOrder);
        var tradeQuantity = Math.Min(buyOrder.Quantity, sellOrder.Quantity);
        var tradePrice = matchingTradeOrder.Price;
        
        if (tradeQuantity <= 0)
            return;
            
        var trade = CreateTrade(buyOrder, sellOrder, tradePrice, tradeQuantity);
            
        tradeRepo.AddTrade(trade);

        buyOrder.Quantity -= tradeQuantity;
        sellOrder.Quantity -= tradeQuantity;
    }

    private static (Order buyOrder, Order sellOrder) GetBuyAndSellOrders(Order order, Order matchingTradeOrder)
    {
        return order.Type == OrderType.Bid
            ? (order, matchingTradeOrder)
            : (matchingTradeOrder, order);
    }

    private static Trade CreateTrade(Order buyOrder, Order sellOrder, decimal tradePrice, int tradeQuantity)
    {
        return new Trade
        {
            BuyOrder = buyOrder,
            SellOrder = sellOrder,
            Price = tradePrice,
            Quantity = tradeQuantity,
            StockSymbol = buyOrder.StockSymbol
        };
    }

    public List<Trade> GetAllTrades()
    {
        return tradeRepo.GetTrades();
    }
}