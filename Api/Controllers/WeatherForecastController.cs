using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    [HttpGet]
    public IEnumerable<int> Get()
    {
        return Enumerable.Range(1, 5).ToArray();
    }
}
