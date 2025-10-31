using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniversityApi.Data;
using UniversityApi.Models;

namespace UniversityApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LecturersController : ControllerBase
    {
        private readonly AppDbContext _context;
        public LecturersController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Lecturer>>> GetLecturers()
        {
            return await _context.Lecturers.Include(l => l.Department).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Lecturer>> GetLecturer(int id)
        {
            var lec = await _context.Lecturers.Include(l => l.Department).FirstOrDefaultAsync(l => l.LecturerId == id);
            if (lec == null) return NotFound();
            return lec;
        }

        [HttpPost]
        public async Task<ActionResult<Lecturer>> CreateLecturer(Lecturer lec)
        {
            _context.Lecturers.Add(lec);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetLecturer), new { id = lec.LecturerId }, lec);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLecturer(int id)
        {
            var lec = await _context.Lecturers.FindAsync(id);
            if (lec == null) return NotFound();
            _context.Lecturers.Remove(lec);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
