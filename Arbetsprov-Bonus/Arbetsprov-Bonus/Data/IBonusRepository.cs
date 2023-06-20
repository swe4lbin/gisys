using Arbetsprov_Bonus.Entities;

namespace Arbetsprov_Bonus.Data;

public interface IBonusRepository
{
    /// <summary>
    /// Gets all the bonus periods from the InMemory database
    /// </summary>
    IEnumerable<BonusPeriod> Get();

    /// <summary>
    /// Gets a bonus period by id and bonus for all consultants for said period
    /// </summary>
    /// <param name="id">The id of the bonus period to get from the database</param>
    BonusPeriodAndConsultants GetById(int id);

    /// <summary>
    /// Removes a bonus period and all bonuses for consultants that belong to the period
    /// </summary>
    /// <param name="id">The id of the bonus period to remove</param>
    bool Remove(int id);

    /// <summary>
    /// Save a new bonus period for a month
    /// Also saves all consultants and their bonuses for said period
    /// </summary>
    /// <param name="periodAndConsultants">The bonus period and consultants to save to the database</param>
    BonusPeriodAndConsultants Add(BonusPeriodAndConsultants periodAndConsultants);

    /// <summary>
    /// Updates a bonus period
    /// Also updates all consultants and their bonuses for said period
    /// </summary>
    /// <param name="id">The id of the bonus period to update</param>
    /// <param name="periodAndConsultants">The bonus period data and consultants to update</param>
    BonusPeriodAndConsultants Update(int id, BonusPeriodAndConsultants periodAndConsultants);

    /// <summary>
    /// Calculates bonuses for consultants, from the values in the parameter
    /// </summary>
    /// <param name="periodAndConsultants">The bonus period and consultants to calculate</param>
    BonusPeriodAndConsultants CalculateBonuses(BonusPeriodAndConsultants periodAndConsultants);
}
