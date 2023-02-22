using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mokiniu_registro_api.Models;

namespace Mokiniu_registro_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SchoolsController : ControllerBase
    {
        private readonly AppDbContext _dbContext;

        public SchoolsController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        //GET: api/Schools
        [HttpGet]
        public async Task<ActionResult<IEnumerable<School>>> GetSchools()
        {
            if (_dbContext.Schools == null)
            {
                return NotFound();
            }
            return await _dbContext.Schools.ToListAsync();
        }

        //GET: api/Schools/1
        [HttpGet("{id}")]
        public async Task<ActionResult<School>> GetSchool(int id)
        {
            if (_dbContext.Schools == null)
            {
                return NotFound();
            }
            var school = await _dbContext.Schools.FindAsync(id);
            if (school == null)
            {
                return NotFound();
            }

            return school;
        }

        //POST: api/Schools
        [HttpPost]
        public async Task<ActionResult<School>> PostSchool(School school)
        {
            _dbContext.Schools.Add(school);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSchool), new { id = school.Id }, school);
        }

        //PUT: api/Schools/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSchool(int id, School school)
        {
            if (id != school.Id)
            {
                return BadRequest();
            }

            _dbContext.Entry(school).State = EntityState.Modified;

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SchoolExist(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Schools/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSchool(int id)
        {
            if (_dbContext.Schools == null)
            {
                return NotFound();
            }

            var school = await _dbContext.Schools.FindAsync(id);
            if (school == null)
            {
                return NotFound();
            }

            _dbContext.Schools.Remove(school);
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }

        private bool SchoolExist(long id)
        {
            return (_dbContext.Schools?.Any(c => c.Id == id)).GetValueOrDefault();
        }
    }
}
}
