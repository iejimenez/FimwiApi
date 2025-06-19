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
using FimwiApi.Core.Models.Responses;

namespace FimwiApi.Application.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CustomerService(
            ICustomerRepository customerRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _customerRepository = customerRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<CustomerDto> GetByIdAsync(Guid id)
        {
            var customer = await _customerRepository.GetByIdAsync(id);
            if (customer == null)
            {
                throw new NotFoundException($"Customer with ID {id} not found");
            }
            return _mapper.Map<CustomerDto>(customer);
        }

        public async Task<PagedResponse<CustomerDto>> GetAllAsync(int pageNumber, int pageSize)
        {
            var customers = await _customerRepository.GetAllAsync();
            var totalRecords = customers.Count();
            var totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
            
            var pagedCustomers = customers
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var customerDtos = _mapper.Map<IEnumerable<CustomerDto>>(pagedCustomers);

            return new PagedResponse<CustomerDto>
            {
                Data = customerDtos,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalPages = totalPages,
                TotalRecords = totalRecords
            };
        }

        public async Task<IEnumerable<CustomerDto>> SearchAsync(string searchTerm, int page, int pageSize)
        {
            var customers = await _customerRepository.SearchAsync(searchTerm, page, pageSize);
            return _mapper.Map<IEnumerable<CustomerDto>>(customers);
        }

        public async Task<CustomerDto> CreateAsync(CustomerDto customerDto)
        {
            var existingCustomer = await _customerRepository.GetByDocumentTypeAndDocumentNumberAsync(customerDto.DocumentType, customerDto.DocumentNumber);
            if (existingCustomer != null)
            {
                throw new ValidationException("A customer with this tax ID already exists");
            }

            var customer = _mapper.Map<Customer>(customerDto);
            customer.CreatedAt = DateTime.UtcNow;
            customer.UpdatedAt = DateTime.UtcNow;

            await _customerRepository.AddAsync(customer);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<CustomerDto>(customer);
        }

        public async Task<CustomerDto> UpdateAsync(CustomerDto customerDto)
        {
            var existingCustomer = await _customerRepository.GetByIdAsync(customerDto.Id);
            if (existingCustomer == null)
            {
                throw new NotFoundException($"Customer with ID {customerDto.Id} not found");
            }

            var duplicateCustomer = await _customerRepository.GetByDocumentTypeAndDocumentNumberAsync(customerDto.DocumentType, customerDto.DocumentNumber);
            if (duplicateCustomer != null && duplicateCustomer.Id != customerDto.Id)
            {
                throw new ValidationException("A customer with this tax ID already exists");
            }

            existingCustomer.Name = customerDto.Name;
            existingCustomer.DocumentType = customerDto.DocumentType;
            existingCustomer.DocumentNumber = customerDto.DocumentNumber;
            existingCustomer.Email = customerDto.Email;
            existingCustomer.Phone = customerDto.Phone;
            existingCustomer.Address = customerDto.Address;
            existingCustomer.UpdatedAt = DateTime.UtcNow;

            await _customerRepository.UpdateAsync(existingCustomer);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<CustomerDto>(existingCustomer);
        }

        public async Task DeleteAsync(Guid id)
        {
            var customer = await _customerRepository.GetByIdAsync(id);
            if (customer == null)
            {
                throw new NotFoundException($"Customer with ID {id} not found");
            }

            await _customerRepository.DeleteAsync(customer);
            await _unitOfWork.SaveChangesAsync();
        }
    }
} 