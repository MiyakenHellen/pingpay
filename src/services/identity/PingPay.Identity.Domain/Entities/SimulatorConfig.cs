namespace PingPay.Identity.Domain.Entities;

public class SimulatorConfig
{
    public int TransactionsPerMinute { get; set; } = 10;
    public double ErrorRate { get; set; } = 0.05;
    public string NetworkSegment { get; set; } = "default";
}