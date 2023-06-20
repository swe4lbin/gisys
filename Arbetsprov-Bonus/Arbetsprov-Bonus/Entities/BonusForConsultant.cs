using System.Text.Json.Serialization;

namespace Arbetsprov_Bonus.Entities;

public class BonusForConsultant
{
    public BonusForConsultant(uint consultantId, uint bonusPeriodId, int bonusAmount, int billedHours, int points)
    {
        ConsultantId = consultantId;
        BonusPeriodId = bonusPeriodId;
        BonusAmount = bonusAmount;
        BilledHours = billedHours;
        Points = points;
    }

    public BonusForConsultant(uint id, uint consultantId, uint bonusPeriodId, int bonusAmount, int billedHours, int points)
    {
        Id = id;
        ConsultantId = consultantId;
        BonusPeriodId = bonusPeriodId;
        BonusAmount = bonusAmount;
        BilledHours = billedHours;
        Points = points;
    }

    public uint Id { get; set; }
    public uint ConsultantId { get; set; }
    public uint BonusPeriodId { get; set; }
    public int BonusAmount { get; set; }
    public int BilledHours { get; set; }
    public int Points { get; set; }
}
