using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniversityApi.Data;
using UniversityApi.Models;

namespace UniversityApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentsController : ControllerBase
    {
        private readonly AppDbContext _context;
        public DepartmentsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Department>>> GetDepartments()
        {
            return await _context.Departments.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Department>> GetDepartment(int id)
        {
            var dep = await _context.Departments.FindAsync(id);
            if (dep == null) return NotFound();
            return dep;
        }

        [HttpPost]
        public async Task<ActionResult<Department>> CreateDepartment(Department dep)
        {
            _context.Departments.Add(dep);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetDepartment), new { id = dep.DepartmentId }, dep);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDepartment(int id, Department dep)
        {
            if (id != dep.DepartmentId) return BadRequest();
            _context.Entry(dep).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDepartment(int id)
        {
            var dep = await _context.Departments.FindAsync(id);
            if (dep == null) return NotFound();
            _context.Departments.Remove(dep);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
