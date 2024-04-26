using Moq;
using TradingEngine.Data;
using TradingEngine.Domain.Enums;
using TradingEngine.Domain.Models;

namespace TradingEngine.Tests;

public class OrderServiceTest
{
    [Fact]
    public void PlaceOrder_AddsOrderAndTriesToExecuteTrades()
    {
        var orderRepo = new Mock<IOrderRepo>();
        var tradingService = new Mock<ITradingService>();
        var orderService = new OrderService(orderRepo.Object, tradingService.Object);

        var order = new Order
        {
            Id = "ORDER-1", UserId = "USER-A", StockSymbol = "INFY", Type = OrderType.Bid, Price = 1650,
            Timestamp = DateTime.Now
        };
        
        orderService.PlaceOrder(order);
        
        orderRepo.Verify(repo => repo.AddOrder(order), Times.Once);
        tradingService.Verify(t => t.TryToExecuteTrades(order), Times.Once);

    }
}