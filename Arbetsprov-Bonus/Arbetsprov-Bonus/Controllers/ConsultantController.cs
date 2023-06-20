using Arbetsprov_Bonus.Data;
using Arbetsprov_Bonus.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata;

namespace Arbetsprov_Bonus.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ConsultantController : ControllerBase
{
    private readonly IConsultantRepository _consultantRepository;

    public ConsultantController(
        IConsultantRepository consultantRepository    
    )
    {
        _consultantRepository = consultantRepository;
    }

    [HttpGet]
    public IEnumerable<Consultant> Get()
    {
        return _consultantRepository.Get();
    }

    //public Consultant GetById(long id)
    //{
    //    throw new NotImplementedException();
    //}

    [HttpPost]
    public Consultant Add([FromBody]Consultant consultant)
    {
        return _consultantRepository.Add(consultant);
        //throw new NotImplementedException();
    }

    [HttpDelete("{id}")]
    public bool Remove([FromRoute] int id)
    {
        return _consultantRepository.Remove(id);
        //throw new NotImplementedException();
    }

    [HttpPut("{id}")]
    public bool Update([FromRoute] int id, [FromBody]Consultant consultant)
    {
        return _consultantRepository.Update(id, consultant);
    }
}
