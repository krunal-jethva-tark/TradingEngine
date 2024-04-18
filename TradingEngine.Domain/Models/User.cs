namespace TradingEngine.Domain.Models;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Balance { get; set; }
    public List<Order> Orders { get; set; } = [];
}