using TradingEngine.Data;
using TradingEngine.Domain.Models;

namespace TradingEngine;

public class OrderService(IOrderRepo orderRepo, ITradingService tradingService)
{
    public void PlaceOrder(Order order)
    {
        orderRepo.AddOrder(order);
        tradingService.TryToExecuteTrades(order);
    }
}