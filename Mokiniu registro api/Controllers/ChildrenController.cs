using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mokiniu_registro_api.DTOs;
using Mokiniu_registro_api.DTOs.Errors;
using Mokiniu_registro_api.Models;
using Mokiniu_registro_api.Services.Interfaces;

namespace Mokiniu_registro_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChildrenController : ControllerBase
    {
        private readonly IChildrenService _childrenService;

        public ChildrenController(IChildrenService childrenService)
        {
            _childrenService = childrenService;
        }

        //GET: api/Children
        [HttpGet]
        public Task<IEnumerable<Child>> GetAll()
        {
            return _childrenService.GetAll();
        }

        //GET: api/Children/1
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Child), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDTO), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Child>> GetById(int id)
        {
            var result = await _childrenService.GetById(id);

            if (!result.Success)
            {
                return NotFound(result.Messages);
            }

            return result.Child;
        }

        //POST: api/Children
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Child>> CreateChild([FromBody] Child child)
        {
            var result = await _childrenService.Create(child);
            if (!result.Success)
            {
                return BadRequest(result.Messages);
            }
            return CreatedAtAction("GetAll", result.Child);
        }

        //PUT: api/Children/5
        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult> Update(int id, [FromBody] Child child)
        {
            try
            {
                var result = await _childrenService.Update(id, child, User.Identity.Name);

                if (!result.Autorise)
                {
                    return Unauthorized(result.Messages);
                }

                if (!result.Success)
                {
                    return BadRequest(result.Messages);
                }
            }
            catch (KeyNotFoundException)
            {
                Error e = new Error();
                e.Message = "Child not found.";
                return NotFound(e);
            }
            return NoContent();
        }

        // DELETE: api/Children/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteChild(int id)
        {
            try
            {
                var result = await _childrenService.Delete(id, User.Identity.Name);

                if (!result.Autorise)
                {
                    return Unauthorized(result.Messages);
                }
                if (!result.Success)
                {
                    return BadRequest(result.Messages);
                }
            }
            catch (KeyNotFoundException)
            {
                Error e = new Error();
                e.Message = "Child not found.";
                return NotFound(e);
            }
            return NoContent();
        }
    }
}
