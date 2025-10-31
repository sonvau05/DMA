using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
[ApiController]
[Route("api/[controller]")]
public class AuthorsController : ControllerBase
{
    private readonly AppDbContext _db;
    public AuthorsController(AppDbContext db) { _db = db; }
    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _db.Authors.Include(a => a.Books).ToListAsync());
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var author = await _db.Authors.Include(a => a.Books).FirstOrDefaultAsync(a => a.AuthorId == id);
        if (author == null) return NotFound();
        return Ok(author);
    }
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Author author)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        _db.Authors.Add(author);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { id = author.AuthorId }, author);
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] Author input)
    {
        if (id != input.AuthorId) return BadRequest();
        if (!ModelState.IsValid) return BadRequest(ModelState);
        _db.Entry(input).State = EntityState.Modified;
        await _db.SaveChangesAsync();
        return NoContent();
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var author = await _db.Authors.FindAsync(id);
        if (author == null) return NotFound();
        _db.Authors.Remove(author);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}