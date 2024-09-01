using Microsoft.AspNetCore.Mvc;
using TaskManager.Api.Extensions;
using TaskManager.Api.Models;
using TaskManager.Application.DTOs.Request;
using TaskManager.Application.DTOs.Response;
using TaskManager.Application.Interfaces;

namespace TaskManager.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    //[Authorize]
    public class ProjectController : Controller
    {
        private readonly IProjectService _projectService;

        public ProjectController(IProjectService projectService)
        {
            _projectService = projectService;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] PaginationParams paginationParams)
        {
            try
            {
                var result = await _projectService.GetAllPagedAsync(paginationParams.PageNumber, paginationParams.PageSize);

                Response.AddPaginationHeader(new PaginationHeader(
                    result.CurrentPage,
                    result.PageSize,
                    result.TotalCount,
                    result.TotalPages
                    ));

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProjectResponseDto>> GetById(Guid id)
        {
            try
            {
                var result = await _projectService.GetByIdAsync(id);
                return Ok(result);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Project with id {id} not found.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProjectRequestDto requestDto)
        {
            try
            {
                var result = await _projectService.CreateAsync(requestDto);
                return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] ProjectRequestDto requestDto)
        {
            try
            {
                requestDto.Id = id;
                var result = await _projectService.UpdateAsync(id, requestDto);
                return Ok(result);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Project with id {id} not found.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var deleted = await _projectService.DeleteAsync(id);
                return Ok(deleted);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Project with id {id} not found.");
            }
        }
    }
}
