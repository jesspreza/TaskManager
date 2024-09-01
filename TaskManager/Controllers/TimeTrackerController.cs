using Microsoft.AspNetCore.Authorization;
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
    public class TimeTrackerController : Controller
    {
        private readonly ITimeTrackerService _timeTrackerService;

        public TimeTrackerController(ITimeTrackerService timeTrackerService)
        {
            _timeTrackerService = timeTrackerService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] PaginationParams paginationParams)
        {
            var result = await _timeTrackerService.GetAllPagedAsync(paginationParams.PageNumber, paginationParams.PageSize);

            Response.AddPaginationHeader(new PaginationHeader(
                result.CurrentPage,
                result.PageSize,
                result.TotalCount,
                result.TotalPages
                ));

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TimeTrackerResponseDto>> GetById(Guid id)
        {
            try
            {
                var result = await _timeTrackerService.GetByIdAsync(id);
                return Ok(result);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"TimeTracker with id {id} not found.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TimeTrackerRequestDto requestDto)
        {
            try
            {
                var result = await _timeTrackerService.CreateAsync(requestDto);
                return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] TimeTrackerRequestDto requestDto)
        {
            try
            {
                requestDto.Id = id;
                var result = await _timeTrackerService.UpdateAsync(id, requestDto);
                return Ok(result);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"TimeTracker with id {id} not found.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var deleted = await _timeTrackerService.DeleteAsync(id);
                return Ok(deleted);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"TimeTracker with id {id} not found.");
            }
        }
    }
}
