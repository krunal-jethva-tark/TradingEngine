using Moq;
using TradingEngine.Data;
using TradingEngine.Domain.Enums;
using TradingEngine.Domain.Models;

namespace TradingEngine.Tests;

public class TradingServiceTests
{
    [Fact]
    public void TryToExecuteTrades_ExecuteTrades_Scenario1()
    {
        var order = new Order
        {
            Id = "ORDER-7",
            UserId = "USER-E",
            StockSymbol = "INFY",
            Type = OrderType.Offer,
            Price = 1640,
            Timestamp = DateTime.Now
        };
        var orderRepo = new Mock<IOrderRepo>();
        var tradeRepo = new Mock<ITradeRepo>();
        var mockBuyOrders = new List<Order> {
            new()
            {
                Id = "ORDER-1", UserId = "USER-A", StockSymbol = "INFY", Type = OrderType.Bid, Price = 1650,
                Timestamp = DateTime.Now
            },
            new()
            {
                Id = "ORDER-3", UserId = "USER-C", StockSymbol = "WIPRO", Type = OrderType.Bid, Price = 550,
                Timestamp = DateTime.Now
            },
            new()
            {
                Id = "ORDER-4", UserId = "USER-C", StockSymbol = "TCS", Type = OrderType.Bid, Price = 3920,
                Timestamp = DateTime.Now
            },
            new()
            {
                Id = "ORDER-6", UserId = "USER-D", StockSymbol = "TCS", Type = OrderType.Bid, Price = 3980,
                Timestamp = DateTime.Now
            },
        };

        orderRepo.Setup(repo => repo.GetOpenBuyOrders()).Returns(mockBuyOrders);
        var tradingService = new TradingService(orderRepo.Object, tradeRepo.Object);
        
        tradingService.TryToExecuteTrades(order);

        var expectedTrade = new Trade
        {
            BuyOrder = mockBuyOrders[0],
            SellOrder = order,
            Price = 1650,
            Quantity = 1,
            StockSymbol = "INFY",
        };
        orderRepo.Verify(x => x.GetOpenSellOrders(), Times.Never);
        tradeRepo.Verify(x => x.AddTrade(It.Is<Trade>(t => t.Equals(expectedTrade))), Times.Once);
    }

    [Fact]
    public void TryToExecuteTrades_ExecuteTrades_Scenario2()
    {
        var order = new Order
        {
            Id = "ORDER-6",
            UserId = "USER-D", 
            StockSymbol = "TCS",
            Type = OrderType.Bid,
            Price = 3980,
            Timestamp = DateTime.Now
        };
        var orderRepo = new Mock<IOrderRepo>();
        var tradeRepo = new Mock<ITradeRepo>();
        var mockSellOrders = new List<Order> {
            new()
            {
                Id = "ORDER-2", UserId = "USER-B", StockSymbol = "TCS", Type = OrderType.Offer, Price = 3950,
                Timestamp = DateTime.Now
            },
            new()
            {
                Id = "ORDER-5", UserId = "USER-B", StockSymbol = "INFY", Type = OrderType.Offer, Price = 1675,
                Timestamp = DateTime.Now
            },
            new()
            {
                Id = "ORDER-7", UserId = "USER-E", StockSymbol = "INFY", Type = OrderType.Offer, Price = 1640,
                Timestamp = DateTime.Now
            },
        };

        orderRepo.Setup(repo => repo.GetOpenSellOrders()).Returns(mockSellOrders);
        var tradingService = new TradingService(orderRepo.Object, tradeRepo.Object);
        
        tradingService.TryToExecuteTrades(order);

        var expectedTrade = new Trade
        {
            BuyOrder = order,
            SellOrder = mockSellOrders[0],
            Price = 3950,
            Quantity = 1,
            StockSymbol = "TCS",
        };
        orderRepo.Verify(x => x.GetOpenBuyOrders(), Times.Never);
        tradeRepo.Verify(x => x.AddTrade(It.Is<Trade>(t => t.Equals(expectedTrade))), Times.Once);
    }
}