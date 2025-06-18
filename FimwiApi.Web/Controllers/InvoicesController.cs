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
    [Route("api/v1/customers/{customerId}/invoices")]
    [Authorize]
    public class InvoicesController : ControllerBase
    {
        private readonly IInvoiceService _invoiceService;

        public InvoicesController(IInvoiceService invoiceService)
        {
            _invoiceService = invoiceService;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResponse<InvoiceDto>>> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var response = await _invoiceService.GetAllAsync(pageNumber, pageSize);
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid customerId, Guid id)
        {
            var invoice = await _invoiceService.GetByIdAsync(id);
            return Ok(invoice);
        }

        [HttpPost]
        public async Task<ActionResult<InvoiceDto>> Create([FromBody] InvoiceDto invoiceDto)
        {
            var createdInvoice = await _invoiceService.CreateAsync(invoiceDto);
            var createdInvoiceDto = new InvoiceDto { Id = createdInvoice.Id, InvoiceNumber = createdInvoice.InvoiceNumber, Status = createdInvoice.Status, Items = createdInvoice.Items.Select(item => new InvoiceItemDto { Id = item.Id, ProductId = item.ProductId, Quantity = item.Quantity, UnitPrice = item.UnitPrice }).ToList() };
            return CreatedAtAction(nameof(GetById), new { id = createdInvoiceDto.Id }, createdInvoiceDto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<InvoiceDto>> Update(int id, [FromBody] InvoiceDto invoiceDto)
        {
            var updatedInvoice = await _invoiceService.UpdateAsync(invoiceDto);
            var updatedInvoiceDto = new InvoiceDto { Id = updatedInvoice.Id, InvoiceNumber = updatedInvoice.InvoiceNumber, Status = updatedInvoice.Status, Items = updatedInvoice.Items.Select(item => new InvoiceItemDto { Id = item.Id, ProductId = item.ProductId, Quantity = item.Quantity, UnitPrice = item.UnitPrice }).ToList() };
            return Ok(updatedInvoiceDto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid customerId, Guid id)
        {
            await _invoiceService.DeleteAsync(id);
            return NoContent();
        }
    }
} 