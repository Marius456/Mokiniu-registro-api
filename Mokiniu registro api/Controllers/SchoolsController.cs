using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
    public class SchoolsController : ControllerBase
    {
        private readonly AppDbContext _dbContext;
        private readonly ISchoolService _schoolService;

        public SchoolsController(AppDbContext dbContext, ISchoolService schoolService)
        {
            _dbContext = dbContext;
            _schoolService = schoolService;
        }

        //GET: api/Schools
        [HttpGet]
        public Task<IEnumerable<School>> GetAll()
        {
            return _schoolService.GetAll();
        }

        //GET: api/Schools
        [HttpGet("Names")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<string>>> GetChildrenSchoolsNames(string parentEmail)
        {
            var result = await _schoolService.GetChildrenSchoolsNames(parentEmail);

            if (result == null)
            {
                return NotFound(result);
            }

            return Ok(result);
        }

        //GET: api/Schools/1
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Parent), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDTO), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<School>> GetById(int id)
        {
            var result = await _schoolService.GetById(id);

            if (!result.Success)
            {
                return NotFound(result.Messages);
            }

            return result.School;
        }

        //POST: api/Schools
        [HttpPost]
        public async Task<ActionResult<School>> Create([FromBody] School school)
        {
            var result = await _schoolService.Create(school);
            if (!result.Success)
            {
                return BadRequest(result.Messages);
            }
            return CreatedAtAction("GetAll", result.School);
        }

        //PUT: api/Schools/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] School school)
        {
            try
            {
                var result = await _schoolService.Update(id, school);

                if (!result.Success)
                {
                    return BadRequest(result.Messages);
                }
            }
            catch (KeyNotFoundException)
            {
                Error e = new Error();
                e.Message = "School not found.";
                return NotFound(e);
            }
            return NoContent();
        }

        // DELETE: api/Schools/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _schoolService.Delete(id);

                if (!result.Success)
                {
                    return BadRequest(result.Messages);
                }
            }
            catch (KeyNotFoundException)
            {
                Error e = new Error();
                e.Message = "School not found.";
                return NotFound(e);
            }
            return NoContent();
        }
    }
}
