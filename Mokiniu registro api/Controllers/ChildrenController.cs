using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mokiniu_registro_api.Models;

namespace Mokiniu_registro_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChildrenController : ControllerBase
    {
        private readonly AppDbContext _dbContext;

        public ChildrenController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        //GET: api/Children
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Child>>> GetChildren()
        {
            if (_dbContext.Children == null)
            {
                return NotFound();
            }
            return await _dbContext.Children.ToListAsync();
        }

        //GET: api/Children/1
        [HttpGet("{id}")]
        public async Task<ActionResult<Child>> GetChild(int id)
        {
            if (_dbContext.Children == null)
            {
                return NotFound();
            }
            var child = await _dbContext.Children.FindAsync(id);
            if (child == null)
            {
                return NotFound();
            }

            return child;
        }

        //POST: api/Children
        [HttpPost]
        public async Task<ActionResult<Child>> PostChild(Child child)
        {
            _dbContext.Children.Add(child);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetChild), new {id = child.Id}, child);
        }

        //PUT: api/Children/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutChild(int id, Child child)
        {
            if (id != child.Id)
            {
                return BadRequest();
            }

            _dbContext.Entry(child).State = EntityState.Modified;

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if(!ChildExist(id))
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

        // DELETE: api/Children/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteChild(int id)
        {
            if (_dbContext.Children == null)
            {
                return NotFound();
            }

            var child = await _dbContext.Children.FindAsync(id);
            if (child == null)
            {
                return NotFound();
            }

            _dbContext.Children.Remove(child);
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }

        private bool ChildExist(long id)
        {
            return (_dbContext.Children?.Any(c => c.Id == id)).GetValueOrDefault();
        }
    }
}
