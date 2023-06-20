using Arbetsprov_Bonus.Entities;
using System.Reflection.Metadata;

namespace Arbetsprov_Bonus.Data;

public class ConsultantRepository : IConsultantRepository
{
    private readonly GisysDbContext _gisysDbContext;

    public ConsultantRepository(
        GisysDbContext gisysDbContext
    )
    {
        _gisysDbContext = gisysDbContext;
    }

    /// <inheritdoc/>
    public Consultant Add(Consultant consultant)
    {
        _gisysDbContext.Consultants.Add(consultant);
        _gisysDbContext.SaveChanges();

        return consultant;
    }

    /// <inheritdoc/>
    public IEnumerable<Consultant> Get()
    {
        return _gisysDbContext.Consultants.ToList();
    }

    /// <inheritdoc/>
    public Consultant GetById(int id)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public bool Remove(int id)
    {
        Consultant? consultant = _gisysDbContext.Consultants.FirstOrDefault(x => x.Id == id);
        if (consultant != null)
        {
            _gisysDbContext.Consultants.Remove(consultant);
            _gisysDbContext.SaveChanges();
            return true;

        }
        return false;
        //throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public bool Update(int id, Consultant consultant)
    {
        Consultant? oldConsultant = _gisysDbContext.Consultants.FirstOrDefault(x => x.Id == id);
        if (oldConsultant != null)
        {
            _gisysDbContext.ChangeTracker.Clear();
            _gisysDbContext.Consultants.Update(consultant);
            _gisysDbContext.SaveChanges();
            return true;

        }
        return false;
    }
}
