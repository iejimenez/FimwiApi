using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FimwiApi.Core.Entities;
using FimwiApi.Core.Interfaces.Repositories;
using FimwiApi.Core.Interfaces.Services;
using FimwiApi.Core.Models.DTOs;
using FimwiApi.Core.Exceptions;
using FimwiApi.Core.Interfaces;

namespace FimwiApi.Application.Services
{
    public class BatchService : IBatchService
    {
        private readonly IBatchRepository _batchRepository;
        private readonly FimwiApi.Core.Interfaces.Repositories.IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BatchService(
            IBatchRepository batchRepository,
            FimwiApi.Core.Interfaces.Repositories.IProductRepository productRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _batchRepository = batchRepository;
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<BatchDto>> GetByProductIdAsync(Guid productId)
        {
            var batches = await _batchRepository.GetByProductIdAsync(productId);
            return _mapper.Map<IEnumerable<BatchDto>>(batches);
        }

        public async Task<BatchDto> GetByIdAsync(Guid id)
        {
            var batch = await _batchRepository.GetByIdAsync(id);
            if (batch == null)
            {
                throw new NotFoundException($"Batch with ID {id} not found");
            }
            return _mapper.Map<BatchDto>(batch);
        }

        public async Task<BatchDto> CreateAsync(BatchDto batchDto)
        {
            var product = await _productRepository.GetByIdAsync(batchDto.ProductId);
            if (product == null)
            {
                throw new NotFoundException($"Product with ID {batchDto.ProductId} not found");
            }

            var existingBatch = await _batchRepository.GetByBatchNumberAsync(batchDto.BatchNumber);
            if (existingBatch != null)
            {
                throw new ValidationException("A batch with this batch number already exists");
            }

            var batch = _mapper.Map<Batch>(batchDto);
            await _batchRepository.AddAsync(batch);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<BatchDto>(batch);
        }

        public async Task<BatchDto> UpdateAsync(BatchDto batchDto)
        {
            var existingBatch = await _batchRepository.GetByIdAsync(batchDto.Id);
            if (existingBatch == null)
            {
                throw new NotFoundException($"Batch with ID {batchDto.Id} not found");
            }

            var product = await _productRepository.GetByIdAsync(batchDto.ProductId);
            if (product == null)
            {
                throw new NotFoundException($"Product with ID {batchDto.ProductId} not found");
            }

            var duplicateBatch = await _batchRepository.GetByBatchNumberAsync(batchDto.BatchNumber);
            if (duplicateBatch != null && duplicateBatch.Id != batchDto.Id)
            {
                throw new ValidationException("A batch with this batch number already exists");
            }

            var batch = _mapper.Map<Batch>(batchDto);
            await _batchRepository.UpdateAsync(batch);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<BatchDto>(batch);
        }

        public async Task DeleteAsync(Guid id)
        {
            var batch = await _batchRepository.GetByIdAsync(id);
            if (batch == null)
            {
                throw new NotFoundException($"Batch with ID {id} not found");
            }

            await _batchRepository.DeleteAsync(batch);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<BatchDto>> GetExpiredBatchesAsync()
        {
            var batches = await _batchRepository.GetExpiredBatchesAsync();
            return _mapper.Map<IEnumerable<BatchDto>>(batches);
        }

        public async Task<IEnumerable<BatchDto>> GetActiveBatchesAsync()
        {
            var batches = await _batchRepository.GetActiveBatchesAsync();
            return _mapper.Map<IEnumerable<BatchDto>>(batches);
        }
    }
} 