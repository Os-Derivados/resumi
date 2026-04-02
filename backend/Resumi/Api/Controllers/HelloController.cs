using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Resumi.Api.Controllers;

[Route("api/hello")]
[ApiController]
public class HelloController : ControllerBase
{
    [HttpGet]
    [AllowAnonymous]
    public string HelloWorld()
    {
        return "Hello, world!";
    }
}