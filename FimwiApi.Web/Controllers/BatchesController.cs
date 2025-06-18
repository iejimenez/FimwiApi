using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using FimwiApi.Core.Models.DTOs;
using FimwiApi.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using FimwiApi.Core.Models.Responses;

namespace FimwiApi.Web.Controllers
{
    [ApiController]
    [Route("api/v1/products/{productId}/batches")]
    [Authorize]
    public class BatchesController : ControllerBase
    {
        private readonly IBatchService _batchService;
        private readonly IMapper _mapper;

        public BatchesController(IBatchService batchService, IMapper mapper)
        {
            _batchService = batchService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(Guid productId, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var batches = await _batchService.GetByProductIdAsync(productId);
            // TODO: Implement pagination logic here
            var pagedResponse = new PagedResponse<BatchDto>
            {
                Data = batches,
                TotalRecords = batches.Count(),
                PageNumber = page,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling(batches.Count() / (double)pageSize)
            };
            return Ok(pagedResponse);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid productId, Guid id)
        {
            var batch = await _batchService.GetByIdAsync(id);
            return Ok(batch);
        }

        [HttpPost]
        public async Task<ActionResult<BatchDto>> Create([FromBody] BatchDto batchDto)
        {
            var createdBatch = await _batchService.CreateAsync(batchDto);
            return CreatedAtAction(nameof(GetById), new { id = createdBatch.Id }, createdBatch);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid productId, Guid id, [FromBody] BatchDto batchDto)
        {
            batchDto.Id = id;
            batchDto.ProductId = productId;
            var batch = await _batchService.UpdateAsync(batchDto);
            return Ok(batch);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid productId, Guid id)
        {
            await _batchService.DeleteAsync(id);
            return NoContent();
        }
    }
} 