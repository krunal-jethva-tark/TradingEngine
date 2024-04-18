using TradingEngine.Data;
using TradingEngine.Domain.Enums;
using TradingEngine.Domain.Models;

namespace TradingEngine.Tests;

public class TradingServiceTests
{
    [Fact]
    public void ExecuteTrades_Scenario_1()
    {
        var orderRepo = new OrderRepo();
        var tradesRepo = new TradeRepo(orderRepo);

        var tradingService = new TradingService(orderRepo, tradesRepo);
        var orderService = new OrderService(orderRepo, tradingService);
        // var tradingEngine = new TradingEngine();

        orderService.PlaceOrder(new Order { Id = "ORDER-1", UserId = "USER-A", StockSymbol = "INFY", Type = OrderType.Bid, Price = 1650, Timestamp = DateTime.Now});
        orderService.PlaceOrder(new Order { Id = "ORDER-2", UserId = "USER-B", StockSymbol = "TCS", Type = OrderType.Offer, Price = 3950, Timestamp = DateTime.Now });
        orderService.PlaceOrder(new Order { Id = "ORDER-3", UserId = "USER-C", StockSymbol = "WIPRO", Type = OrderType.Bid, Price = 550, Timestamp = DateTime.Now });
        orderService.PlaceOrder(new Order { Id = "ORDER-4", UserId = "USER-C", StockSymbol = "TCS", Type = OrderType.Bid, Price = 3920, Timestamp = DateTime.Now });
        orderService.PlaceOrder(new Order { Id = "ORDER-5", UserId = "USER-B", StockSymbol = "INFY", Type = OrderType.Offer, Price = 1675, Timestamp = DateTime.Now });
        orderService.PlaceOrder(new Order { Id = "ORDER-6", UserId = "USER-D", StockSymbol = "TCS", Type = OrderType.Bid, Price = 3980, Timestamp = DateTime.Now });
        orderService.PlaceOrder(new Order { Id = "ORDER-7", UserId = "USER-E", StockSymbol = "INFY", Type = OrderType.Offer, Price = 1640, Timestamp = DateTime.Now });
        
        var trades = tradingService.GetAllTrades();

    }
}