using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    private readonly AppDbContext _db;
    public BooksController(AppDbContext db) { _db = db; }
    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _db.Books.Include(b => b.Author).ToListAsync());
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var book = await _db.Books.Include(b => b.Author).FirstOrDefaultAsync(b => b.BookId == id);
        if (book == null) return NotFound();
        return Ok(book);
    }
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Book book)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        _db.Books.Add(book);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { id = book.BookId }, book);
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] Book input)
    {
        if (id != input.BookId) return BadRequest();
        if (!ModelState.IsValid) return BadRequest(ModelState);
        _db.Entry(input).State = EntityState.Modified;
        await _db.SaveChangesAsync();
        return NoContent();
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var book = await _db.Books.FindAsync(id);
        if (book == null) return NotFound();
        _db.Books.Remove(book);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}