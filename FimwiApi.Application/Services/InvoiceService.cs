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
    public class InvoiceService : IInvoiceService
    {
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public InvoiceService(
            IInvoiceRepository invoiceRepository,
            ICustomerRepository customerRepository,
            IProductRepository productRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _invoiceRepository = invoiceRepository;
            _customerRepository = customerRepository;
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<InvoiceDto> GetByIdAsync(Guid id)
        {
            var invoice = await _invoiceRepository.GetByIdAsync(id);
            if (invoice == null)
            {
                throw new NotFoundException($"Invoice with ID {id} not found");
            }
            return _mapper.Map<InvoiceDto>(invoice);
        }

        public async Task<IEnumerable<InvoiceDto>> GetByCustomerIdAsync(Guid customerId)
        {
            var invoices = await _invoiceRepository.GetByCustomerIdAsync(customerId);
            return _mapper.Map<IEnumerable<InvoiceDto>>(invoices);
        }

        public async Task<PagedResponse<InvoiceDto>> GetAllAsync(int pageNumber, int pageSize)
        {
            var invoices = await _invoiceRepository.GetAllAsync();
            var totalRecords = invoices.Count();
            var totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
            
            var pagedInvoices = invoices
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var invoiceDtos = _mapper.Map<IEnumerable<InvoiceDto>>(pagedInvoices);

            return new PagedResponse<InvoiceDto>
            {
                Data = invoiceDtos,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalPages = totalPages,
                TotalRecords = totalRecords
            };
        }

        public async Task<InvoiceDto> CreateAsync(InvoiceDto invoiceDto)
        {
            var invoice = _mapper.Map<Invoice>(invoiceDto);
            await _invoiceRepository.CreateAsync(invoice);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<InvoiceDto>(invoice);
        }

        public async Task<InvoiceDto> UpdateAsync(InvoiceDto invoiceDto)
        {
            var existingInvoice = await _invoiceRepository.GetByIdAsync(invoiceDto.Id);
            if (existingInvoice == null)
            {
                throw new NotFoundException($"Invoice with ID {invoiceDto.Id} not found");
            }

            var invoice = _mapper.Map<Invoice>(invoiceDto);
            await _invoiceRepository.UpdateAsync(invoice);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<InvoiceDto>(invoice);
        }

        public async Task DeleteAsync(Guid id)
        {
            var invoice = await _invoiceRepository.GetByIdAsync(id);
            if (invoice == null)
            {
                throw new NotFoundException($"Invoice with ID {id} not found");
            }

            // Restore product stock
            foreach (var item in invoice.Items)
            {
                var product = await _productRepository.GetByIdAsync(item.ProductId);
                product.CurrentStock += item.Quantity;
                await _productRepository.UpdateAsync(product);
            }

            await _invoiceRepository.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<InvoiceDto>> GetByStatusAsync(string status)
        {
            var invoices = await _invoiceRepository.GetByStatusAsync(status);
            return _mapper.Map<IEnumerable<InvoiceDto>>(invoices);
        }

        public async Task<IEnumerable<InvoiceDto>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            var invoices = await _invoiceRepository.GetByDateRangeAsync(startDate, endDate);
            return _mapper.Map<IEnumerable<InvoiceDto>>(invoices);
        }
    }
} 