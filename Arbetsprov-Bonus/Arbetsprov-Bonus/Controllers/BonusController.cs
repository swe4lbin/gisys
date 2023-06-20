using Arbetsprov_Bonus.Data;
using Arbetsprov_Bonus.Entities;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Arbetsprov_Bonus.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BonusController : ControllerBase
{
    private readonly IBonusRepository _bonusRepository;

    public BonusController(
        IBonusRepository bonusRepository
    )
    {
        _bonusRepository = bonusRepository;
    }

    [HttpGet]
    public IEnumerable<BonusPeriod> Get()
    {
        return _bonusRepository.Get();
    }

    [HttpGet("{id}")]
    public BonusPeriodAndConsultants Get(int id)
    {
        return _bonusRepository.GetById(id);
    }

    [HttpPost]
    public BonusPeriodAndConsultants Post([FromBody] BonusPeriodAndConsultants period)
    {
       return _bonusRepository.Add(period);
    }

    [HttpPut("{id}")]
    public BonusPeriodAndConsultants Put(int id, [FromBody] BonusPeriodAndConsultants period)
    {
        return _bonusRepository.Update(id, period);
    }

    [HttpDelete("{id}")]
    public bool Delete(int id)
    {
        return _bonusRepository.Remove(id);
    }
}
