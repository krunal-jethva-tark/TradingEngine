using TradingEngine.Data;
using TradingEngine.Domain.Models;

namespace TradingEngine;

public class OrderService
{
    private readonly IOrderRepo _orderRepo;
    private readonly ITradingService _tradingService;

    public OrderService(IOrderRepo orderRepo, ITradingService tradingService)
    {
        _orderRepo = orderRepo;
        _tradingService = tradingService;
    }
    
    public void PlaceOrder(Order order)
    {
        _orderRepo.AddOrder(order);
        _tradingService.TryToExecuteTrades(order);
    }
    
}