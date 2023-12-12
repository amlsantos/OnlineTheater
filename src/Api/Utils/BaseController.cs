using Logic.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Api.Utils;

public abstract class BaseController : ControllerBase
{
    protected IUnitOfWork UnitOfWork;

    public BaseController(IUnitOfWork unitOfWork) => UnitOfWork = unitOfWork;
    
    protected new IActionResult Ok()
    {
        UnitOfWork.SaveChanges();
        return base.Ok(Envelope.Ok());
    }

    protected IActionResult Ok<T>(T result)
    {
        UnitOfWork.SaveChanges();
        return base.Ok(Envelope.Ok(result));
    }

    protected IActionResult Error(string errorMessage)
    {
        return BadRequest(Envelope.Error(errorMessage));
    }
}