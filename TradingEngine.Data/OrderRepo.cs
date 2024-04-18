using TradingEngine.Domain.Enums;
using TradingEngine.Domain.Models;

namespace TradingEngine.Data;

public interface IOrderRepo
{
    void AddOrder(Order order);
    IEnumerable<Order> GetOpenBuyOrders();
    IEnumerable<Order> GetOpenSellOrders();
    IEnumerable<Order> GetOrdersByType(OrderType orderType);
    void MarkOrderAsExecuted(string orderId);
}

public class OrderRepo : IOrderRepo
{
    private readonly List<Order> _orders = [];

    public void AddOrder(Order order)
    {
        _orders.Add(order);
    }

    public IEnumerable<Order> GetOpenBuyOrders()
    {
        return _orders.Where(o => o is { Type: OrderType.Bid, Status: OrderStatus.Open });
    }

    public IEnumerable<Order> GetOpenSellOrders()
    {
        return _orders.Where(o => o is { Type: OrderType.Offer, Status: OrderStatus.Open });
    }

    public IEnumerable<Order> GetOrdersByType(OrderType orderType)
    {
        return _orders.Where(o => o.Type == orderType);
    }

    public void MarkOrderAsExecuted(string orderId)
    {
        var order = _orders.FirstOrDefault(o => o.Id.Equals(orderId));
        if (order != null) order.Status = OrderStatus.Executed;
    }
}