using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using FimwiApi.Core.Models.DTOs;
using FimwiApi.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using FimwiApi.Core.Models.Responses;

namespace FimwiApi.Web.Controllers
{
    [ApiController]
    [Route("api/v1/suppliers")]
    [Authorize]
    public class SuppliersController : ControllerBase
    {
        private readonly ISupplierService _supplierService;

        public SuppliersController(ISupplierService supplierService)
        {
            _supplierService = supplierService;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResponse<SupplierDto>>> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var response = await _supplierService.GetAllAsync(pageNumber, pageSize);
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var supplier = await _supplierService.GetByIdAsync(id);
            return Ok(supplier);
        }

        [HttpPost]
        public async Task<ActionResult<SupplierDto>> Create([FromBody] SupplierDto supplierDto)
        {
            var createdSupplier = await _supplierService.CreateAsync(supplierDto);
            return CreatedAtAction(nameof(GetById), new { id = createdSupplier.Id }, createdSupplier);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<SupplierDto>> Update(Guid id, [FromBody] SupplierDto supplierDto)
        {
            supplierDto.Id = id;
            var updatedSupplier = await _supplierService.UpdateAsync(supplierDto);
            return Ok(updatedSupplier);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _supplierService.DeleteAsync(id);
            return NoContent();
        }
    }
} 