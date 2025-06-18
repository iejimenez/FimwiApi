using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FimwiApi.Core.Entities;
using FimwiApi.Core.Exceptions;
using FimwiApi.Core.Interfaces;
using FimwiApi.Core.Interfaces.Services;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using FimwiApi.Core.Models.DTOs;
using FimwiApi.Core.Interfaces.Repositories;
using FimwiApi.Core.Models.Responses;

namespace FimwiApi.Application.Services
{
    public class PurchaseOrderService : IPurchaseOrderService
    {
        private readonly IPurchaseOrderRepository _purchaseOrderRepository;
        private readonly ISupplierRepository _supplierRepository;
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PurchaseOrderService(
            IPurchaseOrderRepository purchaseOrderRepository,
            ISupplierRepository supplierRepository,
            IProductRepository productRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _purchaseOrderRepository = purchaseOrderRepository;
            _supplierRepository = supplierRepository;
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PurchaseOrderDto> GetByIdAsync(Guid id)
        {
            var purchaseOrder = await _purchaseOrderRepository.GetByIdAsync(id);
            if (purchaseOrder == null)
            {
                throw new NotFoundException($"Purchase order with ID {id} not found");
            }
            return _mapper.Map<PurchaseOrderDto>(purchaseOrder);
        }

        public async Task<IEnumerable<PurchaseOrderDto>> GetBySupplierIdAsync(Guid supplierId)
        {
            var purchaseOrders = await _purchaseOrderRepository.GetBySupplierIdAsync(supplierId);
            return _mapper.Map<IEnumerable<PurchaseOrderDto>>(purchaseOrders);
        }

        public async Task<PagedResponse<PurchaseOrderDto>> GetAllAsync(int pageNumber, int pageSize)
        {
            var purchaseOrders = await _purchaseOrderRepository.GetAllAsync();
            var totalRecords = purchaseOrders.Count();
            var totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
            
            var pagedPurchaseOrders = purchaseOrders
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var purchaseOrderDtos = _mapper.Map<IEnumerable<PurchaseOrderDto>>(pagedPurchaseOrders);

            return new PagedResponse<PurchaseOrderDto>
            {
                Data = purchaseOrderDtos,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalPages = totalPages,
                TotalRecords = totalRecords
            };
        }

        public async Task<PurchaseOrderDto> CreateAsync(PurchaseOrderDto orderDto)
        {
            var purchaseOrder = _mapper.Map<PurchaseOrder>(orderDto);
            await _purchaseOrderRepository.AddAsync(purchaseOrder);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<PurchaseOrderDto>(purchaseOrder);
        }

        public async Task<PurchaseOrderDto> UpdateAsync(PurchaseOrderDto orderDto)
        {
            var existingOrder = await _purchaseOrderRepository.GetByIdAsync(orderDto.Id);
            if (existingOrder == null)
            {
                throw new NotFoundException($"Purchase order with ID {orderDto.Id} not found");
            }

            var purchaseOrder = _mapper.Map<PurchaseOrder>(orderDto);
            await _purchaseOrderRepository.UpdateAsync(purchaseOrder);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<PurchaseOrderDto>(purchaseOrder);
        }

        public async Task DeleteAsync(Guid id)
        {
            var purchaseOrder = await _purchaseOrderRepository.GetByIdAsync(id);
            if (purchaseOrder == null)
            {
                throw new NotFoundException($"Purchase order with ID {id} not found");
            }

            await _purchaseOrderRepository.DeleteAsync(purchaseOrder);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<PurchaseOrderDto>> GetByStatusAsync(string status)
        {
            var purchaseOrders = await _purchaseOrderRepository.GetByStatusAsync(status);
            return _mapper.Map<IEnumerable<PurchaseOrderDto>>(purchaseOrders);
        }

        public async Task<IEnumerable<PurchaseOrderDto>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            var purchaseOrders = await _purchaseOrderRepository.GetByDateRangeAsync(startDate, endDate);
            return _mapper.Map<IEnumerable<PurchaseOrderDto>>(purchaseOrders);
        }
    }
} 