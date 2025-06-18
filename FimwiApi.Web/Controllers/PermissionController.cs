using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using System.Linq;
using FimwiApi.Core.Models.DTOs;
using FimwiApi.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using FimwiApi.Core.Models.Responses;

namespace FimwiApi.Web.Controllers
{
    [ApiController]
    [Route("api/v1/permissions")]
    [Authorize]
    public class PermissionController : ControllerBase
    {
        private readonly IPermissionService _permissionService;

        public PermissionController(IPermissionService permissionService)
        {
            _permissionService = permissionService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var permissions = await _permissionService.GetAllAsync();
            var totalRecords = permissions.Count();
            var totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
            var pagedData = permissions.Skip((pageNumber - 1) * pageSize).Take(pageSize);

            var pagedResponse = new PagedResponse<PermissionDto>
            {
                Data = pagedData,
                TotalRecords = totalRecords,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalPages = totalPages
            };
            return Ok(pagedResponse);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var permission = await _permissionService.GetByIdAsync(id);
            return Ok(permission);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PermissionDto permissionDto)
        {
            var permission = await _permissionService.CreateAsync(permissionDto);
            return CreatedAtAction(nameof(GetById), new { id = permission.Id }, permission);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] PermissionDto permissionDto)
        {
            var permission = await _permissionService.UpdateAsync(id, permissionDto);
            return Ok(permission);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _permissionService.DeleteAsync(id);
            return NoContent();
        }
    }
} 