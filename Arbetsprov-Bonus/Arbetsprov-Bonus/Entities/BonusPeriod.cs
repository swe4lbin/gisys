using System.Text.Json.Serialization;

namespace Arbetsprov_Bonus.Entities;

public class BonusPeriod
{
    [JsonConstructor]
    public BonusPeriod(int netProfit, int totalPoints, DateTime startDate, DateTime endDate, string status)
    {
        NetProfit = netProfit;
        TotalPoints = totalPoints;
        StartDate = startDate;
        EndDate = endDate;
        Status = status;
    }

    public BonusPeriod(uint id, int netProfit, int totalPoints, DateTime startDate, DateTime endDate, string status)
    {
        Id = id;
        NetProfit = netProfit;
        TotalPoints = totalPoints;
        StartDate = startDate;
        EndDate = endDate;
        Status = status;
    }

    public uint Id { get; set; }
    public int NetProfit { get; set; }
    public int TotalPoints { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Status { get; set; }
}
