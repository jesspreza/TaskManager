using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Api.Extensions;
using TaskManager.Api.Models;
using TaskManager.Application.DTOs.Request;
using TaskManager.Application.DTOs.Response;
using TaskManager.Application.Interfaces;
using TaskManager.Domain.Pagination;

namespace TaskManager.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class ProjectController : Controller
    {
        private readonly IProjectService _projectService;

        public ProjectController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        /// <summary>
        /// Get a list of projects with pagination.
        /// </summary>
        /// <param name="paginationParams">Pagination parameters including page number and page size.</param>
        /// <returns>A paginated list of projects.</returns>
        /// <response code="200">Returns a paginated list of projects.</response>
        /// <response code="400">If the pagination parameters are invalid.</response>
        [HttpGet]
        [ProducesResponseType(typeof(PagedList<ProjectResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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

        /// <summary>
        /// Search for projects based on a search term with pagination.
        /// </summary>
        /// <param name="searchTerm">The term to search for within project names or descriptions.</param>
        /// <param name="paginationParams">Pagination parameters including page number and page size.</param>
        /// <returns>A paginated list of projects that match the search term.</returns>
        /// <response code="200">Returns a paginated list of projects matching the search term.</response>
        /// <response code="400">If the search or pagination parameters are invalid.</response>
        [HttpGet("search")]
        [ProducesResponseType(typeof(PagedList<ProjectResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SearchProjects([FromQuery] string? searchTerm, [FromQuery] PaginationParams paginationParams)
        {
            try
            {
                var result = await _projectService.SearchProjectsAsync(searchTerm, paginationParams.PageNumber, paginationParams.PageSize);

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

        /// <summary>
        /// Get a project by ID.
        /// </summary>
        /// <param name="id">ID of the project.</param>
        /// <returns>The specified project.</returns>
        /// <response code="200">Returns the specified project.</response>
        /// <response code="404">If the project with the specified ID is not found.</response>
        /// <response code="400">If the request is invalid.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ProjectResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Create a new project.
        /// </summary>
        /// <param name="ProjectRequestDto">Details of the project to create.</param>
        /// <returns>The created project.</returns>
        /// <response code="201">Returns the created project.</response>
        /// <response code="400">If the project could not be created due to invalid data.</response>
        [HttpPost]
        [ProducesResponseType(typeof(ProjectResponseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

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


        /// <summary>
        /// Update an existing project.
        /// </summary>
        /// <param name="id">ID of the project to update.</param>
        /// <param name="ProjectRequestDto">Updated details of the project.</param>
        /// <returns>The updated project.</returns>
        /// <response code="200">Returns the updated project.</response>
        /// <response code="404">If the project with the specified ID is not found.</response>
        /// <response code="400">If the project could not be updated due to invalid data.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ProjectResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Delete a project by ID.
        /// </summary>
        /// <param name="id">ID of the project to delete.</param>
        /// <returns>A response indicating the result of the delete operation.</returns>
        /// <response code="200">Returns a success message if the project was deleted.</response>
        /// <response code="404">If the project with the specified ID is not found.</response>
        /// <response code="400">If the deletion failed due to an invalid request.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
