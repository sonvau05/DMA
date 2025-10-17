using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
[ApiController]
[Route("api/[controller]")]
public class DataController : ControllerBase
{
    [Authorize]
    [HttpGet("protected")]
    public IActionResult Protected(){return Ok(new {msg="Bạn đã vào trang quản trị"});}
}
