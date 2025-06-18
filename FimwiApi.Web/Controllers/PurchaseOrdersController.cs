using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using FimwiApi.Core.Models.DTOs;
using FimwiApi.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using FimwiApi.Core.Models.Responses;
using FimwiApi.Core.Entities;

namespace FimwiApi.Web.Controllers
{
    [ApiController]
    [Route("api/v1/purchase-orders")]
    [Authorize]
    public class PurchaseOrdersController : ControllerBase
    {
        private readonly IPurchaseOrderService _purchaseOrderService;

        public PurchaseOrdersController(IPurchaseOrderService purchaseOrderService)
        {
            _purchaseOrderService = purchaseOrderService;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResponse<PurchaseOrderDto>>> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var purchaseOrders = await _purchaseOrderService.GetAllAsync(pageNumber, pageSize);
            return Ok(purchaseOrders);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var purchaseOrder = await _purchaseOrderService.GetByIdAsync(id);
            return Ok(purchaseOrder);
        }

        [HttpPost]
        public async Task<ActionResult<PurchaseOrderDto>> Create([FromBody] PurchaseOrderDto purchaseOrderDto)
        {
            var createdPurchaseOrder = await _purchaseOrderService.CreateAsync(purchaseOrderDto);
            return CreatedAtAction(nameof(GetById), new { id = createdPurchaseOrder.Id }, createdPurchaseOrder);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<PurchaseOrderDto>> Update(int id, [FromBody] PurchaseOrderDto purchaseOrderDto)
        {
            var updatedPurchaseOrder = await _purchaseOrderService.UpdateAsync(purchaseOrderDto);
            return Ok(updatedPurchaseOrder);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _purchaseOrderService.DeleteAsync(id);
            return NoContent();
        }
    }
} 