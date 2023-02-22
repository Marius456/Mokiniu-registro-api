using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mokiniu_registro_api.Models;

namespace Mokiniu_registro_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParentsController : ControllerBase
    {
        private readonly AppDbContext _dbContext;

        public ParentsController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        //GET: api/Parents
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Parent>>> GetParents()
        {
            if (_dbContext.Parents == null)
            {
                return NotFound();
            }
            return await _dbContext.Parents.ToListAsync();
        }

        //GET: api/Parents/1
        [HttpGet("{id}")]
        public async Task<ActionResult<Parent>> GetParent(int id)
        {
            if (_dbContext.Parents == null)
            {
                return NotFound();
            }
            var parent = await _dbContext.Parents.FindAsync(id);
            if (parent == null)
            {
                return NotFound();
            }

            return parent;
        }

        //POST: api/Parents
        [HttpPost]
        public async Task<ActionResult<Parent>> PostParent(Parent parent)
        {
            _dbContext.Parents.Add(parent);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetParent), new { id = parent.Id }, parent);
        }

        //PUT: api/Parents/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutParent(int id, Parent parent)
        {
            if (id != parent.Id)
            {
                return BadRequest();
            }

            _dbContext.Entry(parent).State = EntityState.Modified;

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ParentExist(id))
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

        // DELETE: api/Parents/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteParent(int id)
        {
            if (_dbContext.Parents == null)
            {
                return NotFound();
            }

            var parent = await _dbContext.Parents.FindAsync(id);
            if (parent == null)
            {
                return NotFound();
            }

            _dbContext.Parents.Remove(parent);
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }

        private bool ParentExist(long id)
        {
            return (_dbContext.Parents?.Any(c => c.Id == id)).GetValueOrDefault();
        }
    }
}
