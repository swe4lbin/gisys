using Arbetsprov_Bonus.Entities;
using System.Reflection.Metadata;

namespace Arbetsprov_Bonus.Data;

public class BonusRepository : IBonusRepository
{
    private readonly GisysDbContext _gisysDbContext;

    public BonusRepository(
        GisysDbContext gisysDbContext
    )
    {
        _gisysDbContext = gisysDbContext;
    }

    /// <inheritdoc/>
    public BonusPeriodAndConsultants Add(BonusPeriodAndConsultants periodAndConsultants)
    {

        BonusPeriodAndConsultants calculated = CalculateBonuses(periodAndConsultants);

        calculated.BonusPeriod.Status = "draft";
        _gisysDbContext.BonusPeriods.Add(calculated.BonusPeriod);

        foreach (ConsultantAndCalculation consultant in periodAndConsultants.ConsultantsAndCalculations)
        {
            BonusForConsultant consultantBonus = new BonusForConsultant(consultant.Consultant.Id, calculated.BonusPeriod.Id, consultant.Bonus, consultant.BilledHours, consultant.Points);
            _gisysDbContext.BonusesForConsultants.Add(consultantBonus);
        }

        _gisysDbContext.SaveChanges();

        periodAndConsultants = calculated;

        return periodAndConsultants;
    }

    /// <inheritdoc/>
    public IEnumerable<BonusPeriod> Get()
    {
        return _gisysDbContext.BonusPeriods.ToList();
    }

    /// <inheritdoc/>
    public BonusPeriodAndConsultants GetById(int id)
    {
        List<BonusForConsultant> bonuses = _gisysDbContext.BonusesForConsultants.Where(x => x.BonusPeriodId == id).ToList();
        List<uint> consultantIds = bonuses.Select(x => x.ConsultantId).ToList();
        List<Consultant> consultants = _gisysDbContext.Consultants.Where(x => consultantIds.Any(y => y == x.Id)).ToList();

        List<ConsultantAndCalculation> parsedList = new List<ConsultantAndCalculation>();
        foreach (BonusForConsultant bonus in bonuses)
        {
            ConsultantAndCalculation consultantAndCalc = new ConsultantAndCalculation() { Bonus = bonus.BonusAmount, BilledHours = bonus.BilledHours, Points = bonus.Points, Consultant = consultants.FirstOrDefault(x => x.Id == bonus.ConsultantId) };
            parsedList.Add(consultantAndCalc);
        }
        BonusPeriodAndConsultants periodAndConsultants = new BonusPeriodAndConsultants() { ConsultantsAndCalculations = parsedList, BonusPeriod = _gisysDbContext.BonusPeriods.FirstOrDefault(x => x.Id == id) };
        return periodAndConsultants;
    }

    /// <inheritdoc/>
    public bool Remove(int id)
    {
        BonusPeriod? bonusPeriod = _gisysDbContext.BonusPeriods.FirstOrDefault(x => x.Id == id);
        List<BonusForConsultant> bonuses = _gisysDbContext.BonusesForConsultants.Where(x => x.BonusPeriodId == id).ToList();
        if (bonusPeriod != null)
        {
            _gisysDbContext.BonusPeriods.Remove(bonusPeriod);
            _gisysDbContext.BonusesForConsultants.RemoveRange(bonuses);
            _gisysDbContext.SaveChanges();
            return true;

        }
        return false;
        //throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public BonusPeriodAndConsultants? Update(int id, BonusPeriodAndConsultants periodAndConsultants)
    {
        BonusPeriod? oldPeriod = _gisysDbContext.BonusPeriods.FirstOrDefault(x => x.Id == id);
        if (oldPeriod != null)
        {
            BonusPeriodAndConsultants calculated = CalculateBonuses(periodAndConsultants);
            _gisysDbContext.ChangeTracker.Clear();

            _gisysDbContext.BonusPeriods.Update(calculated.BonusPeriod);

            foreach (ConsultantAndCalculation consultant in periodAndConsultants.ConsultantsAndCalculations)
            {
                BonusForConsultant? oldBonus = _gisysDbContext.BonusesForConsultants.FirstOrDefault(x => x.ConsultantId == consultant.Consultant.Id && x.BonusPeriodId == calculated.BonusPeriod.Id);
                if (oldBonus is BonusForConsultant)
                {
                    oldBonus.BonusAmount = consultant.Bonus;
                    oldBonus.BilledHours = consultant.BilledHours;
                    oldBonus.Points = consultant.Points;
                }
                else
                {
                    BonusForConsultant consultantBonus = new BonusForConsultant(consultant.Consultant.Id, calculated.BonusPeriod.Id, consultant.Bonus, consultant.BilledHours, consultant.Points);
                    _gisysDbContext.BonusesForConsultants.Add(consultantBonus);
                }
            }

            _gisysDbContext.SaveChanges();
            return calculated;

        }
        return null;
    }

    public BonusPeriodAndConsultants CalculateBonuses(BonusPeriodAndConsultants bonusPeriodAndConsultants)
    {
        double totalBonus = bonusPeriodAndConsultants.BonusPeriod.NetProfit * 0.05;
        int totalPoints = 0;
        for (int i = 0; i < bonusPeriodAndConsultants.ConsultantsAndCalculations.Count(); i++)
        {
            double loyaltyFactor = getLoyaltyFactor(bonusPeriodAndConsultants.ConsultantsAndCalculations[i].Consultant);
            bonusPeriodAndConsultants.ConsultantsAndCalculations[i].Points = (int)(bonusPeriodAndConsultants.ConsultantsAndCalculations[i].BilledHours * loyaltyFactor);
            totalPoints = totalPoints + bonusPeriodAndConsultants.ConsultantsAndCalculations[i].Points;
        }

        for (int i = 0; i < bonusPeriodAndConsultants.ConsultantsAndCalculations.Count(); i++)
        {
            double pointPerc = ((double)bonusPeriodAndConsultants.ConsultantsAndCalculations[i].Points / totalPoints);
            if (double.IsNaN(pointPerc))
            {
                pointPerc = 0;
            }
            bonusPeriodAndConsultants.ConsultantsAndCalculations[i].Bonus = Convert.ToInt32((totalBonus * pointPerc));
        }
        return bonusPeriodAndConsultants;
    }

    public double getLoyaltyFactor(Consultant consultant)
    {
        double loyaltyFactor = 1;
        int employedYears = getEmployedYears(consultant.StartDate);
        if (employedYears < 5) {
            loyaltyFactor = loyaltyFactor + (employedYears / 10.0);
        }
        else
        {
            loyaltyFactor = 1.5;
        }
        return loyaltyFactor;
    }

    int getEmployedYears(DateTime employedDate)
    {
        DateTime now = DateTime.Now;

        return (now.Year - employedDate.Year - 1) +
            (((now.Month > employedDate.Month) ||
            ((now.Month == employedDate.Month) && (now.Day >= employedDate.Day))) ? 1 : 0);
    }
}

public class ConsultantAndCalculation
{
    public Consultant? Consultant
    {
        get;
        set;
    }
    public int BilledHours
    {
        get;
        set;
    }
    public int Points
    {
        get;
        set;
    }
    public int Bonus
    {
        get;
        set;
    }
}

public class BonusPeriodAndConsultants
{
    public BonusPeriod BonusPeriod
    { get; set; }
    public List<ConsultantAndCalculation> ConsultantsAndCalculations
    { get; set; }

}