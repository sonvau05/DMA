using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LoginSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DataController : ControllerBase
    {
        [Authorize]
        [HttpGet("private")]
        public IActionResult GetPrivateData()
        {
            return Ok(new { message = "This is secret data" });
        }
    }
}
