using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FimwiApi.Core.Models.DTOs;
using FimwiApi.Core.Entities;
using FimwiApi.Core.Exceptions;
using FimwiApi.Core.Interfaces.Repositories;
using FimwiApi.Core.Interfaces.Services;
using FimwiApi.Core.Interfaces;
using FimwiApi.Core.Models.Responses;

namespace FimwiApi.Application.Services
{
    public class SupplierService : ISupplierService
    {
        private readonly ISupplierRepository _supplierRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SupplierService(
            ISupplierRepository supplierRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _supplierRepository = supplierRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<SupplierDto> GetByIdAsync(Guid id)
        {
            var supplier = await _supplierRepository.GetByIdAsync(id);
            if (supplier == null)
            {
                throw new NotFoundException($"Supplier with ID {id} not found");
            }
            return _mapper.Map<SupplierDto>(supplier);
        }

        public async Task<PagedResponse<SupplierDto>> GetAllAsync(int pageNumber, int pageSize)
        {
            var suppliers = await _supplierRepository.GetAllAsync();
            var totalRecords = suppliers.Count();
            var totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
            var pagedData = suppliers.Skip((pageNumber - 1) * pageSize).Take(pageSize);

            return new PagedResponse<SupplierDto>
            {
                Data = _mapper.Map<IEnumerable<SupplierDto>>(pagedData),
                TotalRecords = totalRecords,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalPages = totalPages
            };
        }

        public async Task<IEnumerable<SupplierDto>> SearchAsync(string searchTerm, int page, int pageSize)
        {
            var suppliers = await _supplierRepository.SearchAsync(searchTerm, page, pageSize);
            return _mapper.Map<IEnumerable<SupplierDto>>(suppliers);
        }

        public async Task<SupplierDto> CreateAsync(SupplierDto supplierDto)
        {
            var supplier = _mapper.Map<Supplier>(supplierDto);
            await _supplierRepository.CreateAsync(supplier);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<SupplierDto>(supplier);
        }

        public async Task<SupplierDto> UpdateAsync(SupplierDto supplierDto)
        {
            var existingSupplier = await _supplierRepository.GetByIdAsync(supplierDto.Id);
            if (existingSupplier == null)
            {
                throw new NotFoundException($"Supplier with ID {supplierDto.Id} not found");
            }

            var supplier = _mapper.Map<Supplier>(supplierDto);
            await _supplierRepository.UpdateAsync(supplier);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<SupplierDto>(supplier);
        }

        public async Task DeleteAsync(Guid id)
        {
            var supplier = await _supplierRepository.GetByIdAsync(id);
            if (supplier == null)
            {
                throw new NotFoundException($"Supplier with ID {id} not found");
            }

            await _supplierRepository.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }
    }
} 