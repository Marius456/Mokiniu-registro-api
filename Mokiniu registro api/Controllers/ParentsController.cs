using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mokiniu_registro_api.DTOs;
using Mokiniu_registro_api.DTOs.Errors;
using Mokiniu_registro_api.Models;
using Mokiniu_registro_api.Services;
using Mokiniu_registro_api.Services.Interfaces;

namespace Mokiniu_registro_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParentsController : ControllerBase
    {
        private readonly IParentService _parentService;

        public ParentsController(IParentService parentService)
        {
            _parentService = parentService;
        }


        [HttpPost]
        [Route("login")]
        [ProducesResponseType(typeof(AuthenticatedUserDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
        {
            var response = await _parentService.Login(loginDTO);
            if (response.Success == false)
            {
                return BadRequest(response.Message);
            }
            return Ok(response.Data);
        }

        //GET: api/Parents
        [HttpGet]
        public Task<IEnumerable<Parent>> GetAll()
        {
            return _parentService.GetAll();
        }

        //GET: api/Parents/1
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Parent), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDTO), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Parent>> GetById(int id)
        {
            var result = await _parentService.GetById(id);

            if (!result.Success)
            {
                return NotFound(result.Messages);
            }

            return result.Parent;
        }

        //POST: api/Parents
        [HttpPost]
        public async Task<ActionResult<Parent>> Create([FromBody] Parent parent)
        {
            var result = await _parentService.Create(parent);
            if (!result.Success)
            {
                return BadRequest(result.Messages);
            }
            return CreatedAtAction("GetAll", result.Parent);
        }

        //PUT: api/Parents/5
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Update(int id, Parent parent)
        {
            try
            {
                var result = await _parentService.Update(id, parent, User.Identity.Name);

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
                e.Message = "Parent not found.";
                return NotFound(e);
            }
            return NoContent();
        }

        // DELETE: api/Parents/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteParent(int id)
        {
            try
            {
                var result = await _parentService.Delete(id, User.Identity.Name);

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
                e.Message = "Parent not found.";
                return NotFound(e);
            }
            return NoContent();
        }
    }
}
