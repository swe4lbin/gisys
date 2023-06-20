using Arbetsprov_Bonus.Entities;

namespace Arbetsprov_Bonus.Data;

public interface IConsultantRepository
{
    /// <summary>
    /// Gets all the Consultants from the InMemory database
    /// </summary>
    IEnumerable<Consultant> Get();

    /// <summary>
    /// Gets a Consultant by id
    /// </summary>
    /// <param name="id">The id of the consultant to get from the database</param>
    Consultant GetById(int id);

    /// <summary>
    /// Removes a Consultant by id
    /// </summary>
    /// <param name="id">The id of the consultant to remove from the database</param>
    bool Remove(int id);

    /// <summary>
    /// Saves a new Consultant to the InMemory database
    /// </summary>
    /// <param name="consultant">The consultant to save to the database</param>
    Consultant Add(Consultant consultant);

    /// <summary>
    /// Updates an existing Consultant by id
    /// </summary>
    /// <param name="id">The id of the consultant to update to the database</param>
    /// <param name="consultant">The consultant data to update</param>
    bool Update(int id, Consultant consultant);
}
