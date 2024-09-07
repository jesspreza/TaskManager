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
    public class CollaboratorController : Controller
    {
        private readonly ICollaboratorService _collaboratorService;

        public CollaboratorController(ICollaboratorService collaboratorService)
        {
            _collaboratorService = collaboratorService;
        }

        /// <summary>
        /// Get a list of collaborators with pagination.
        /// </summary>
        /// <param name="paginationParams">Pagination parameters including page number and page size.</param>
        /// <returns>A paginated list of collaborators.</returns>
        /// <response code="200">Returns a paginated list of collaborators.</response>
        /// <response code="400">If the pagination parameters are invalid.</response>
        [HttpGet]
        [ProducesResponseType(typeof(PagedList<CollaboratorResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAll([FromQuery] PaginationParams paginationParams)
        {
            try
            {
                var result = await _collaboratorService.GetAllPagedAsync(paginationParams.PageNumber, paginationParams.PageSize);

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
        /// Get a collaborator by ID.
        /// </summary>
        /// <param name="id">ID of the collaborator.</param>
        /// <returns>A specific collaborator.</returns>
        /// <response code="200">Returns a specific collaborator.</response>
        /// <response code="404">If the collaborator with the specified ID is not found.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(CollaboratorResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CollaboratorResponseDto>> GetById(Guid id)
        {
            try
            {
                var result = await _collaboratorService.GetByIdAsync(id);
                return Ok(result);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Collaborator with id {id} not found.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Create a new collaborator.
        /// </summary>
        /// <param name="requestDto">Details of the collaborator to create.</param>
        /// <returns>The created collaborator.</returns>
        /// <response code="201">Returns the created collaborator.</response>
        /// <response code="400">If the collaborator could not be created due to invalid data.</response>
        [HttpPost]
        [ProducesResponseType(typeof(CollaboratorResponseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CollaboratorRequestDto requestDto)
        {
            try
            {
                var result = await _collaboratorService.CreateAsync(requestDto);
                return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Update an existing collaborator.
        /// </summary>
        /// <param name="id">ID of the collaborator to update.</param>
        /// <param name="requestDto">Updated details of the collaborator.</param>
        /// <returns>The updated collaborator.</returns>
        /// <response code="200">Returns the updated collaborator.</response>
        /// <response code="404">If the collaborator with the specified ID is not found.</response>
        /// <response code="400">If the collaborator could not be updated due to invalid data.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(CollaboratorResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(Guid id, [FromBody] CollaboratorRequestDto requestDto)
        {
            try
            {
                requestDto.Id = id;
                var result = await _collaboratorService.UpdateAsync(id, requestDto);
                return Ok(result);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Collaborator with id {id} not found.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Delete a collaborator by ID.
        /// </summary>
        /// <param name="id">ID of the collaborator to delete.</param>
        /// <returns>A response indicating the result of the delete operation.</returns>
        /// <response code="200">Returns a success message if the collaborator was deleted.</response>
        /// <response code="404">If the collaborator with the specified ID is not found.</response>
        /// <response code="400">If the deletion failed due to an invalid request.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var deleted = await _collaboratorService.DeleteAsync(id);
                return Ok(deleted);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Collaborator with id {id} not found.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
