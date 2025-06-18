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
    [Route("api/v1/roles/{roleId}/permissions")]
    [Authorize]
    public class RolePermissionController : ControllerBase
    {
        private readonly IRolePermissionService _rolePermissionService;

        public RolePermissionController(IRolePermissionService rolePermissionService)
        {
            _rolePermissionService = rolePermissionService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(Guid roleId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var permissions = await _rolePermissionService.GetByRoleIdAsync(roleId);
            var totalRecords = permissions.Count();
            var totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
            var pagedData = permissions.Skip((pageNumber - 1) * pageSize).Take(pageSize);

            var pagedResponse = new PagedResponse<RolePermissionDto>
            {
                Data = pagedData,
                TotalRecords = totalRecords,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalPages = totalPages
            };
            return Ok(pagedResponse);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Guid roleId, [FromBody] RolePermissionDto rolePermissionDto)
        {
            rolePermissionDto.RoleId = roleId;
            var rolePermission = await _rolePermissionService.CreateAsync(rolePermissionDto);
            return CreatedAtAction(nameof(GetAll), new { roleId }, rolePermission);
        }

        [HttpDelete("{permissionId}")]
        public async Task<IActionResult> Delete(Guid roleId, Guid permissionId)
        {
            await _rolePermissionService.DeleteAsync(roleId, permissionId);
            return NoContent();
        }
    }
} 