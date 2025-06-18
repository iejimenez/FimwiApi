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
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductService(
            IProductRepository productRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ProductDto> GetByIdAsync(Guid id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
            {
                throw new NotFoundException($"Product with ID {id} not found");
            }
            return _mapper.Map<ProductDto>(product);
        }

        public async Task<PagedResponse<ProductDto>> GetAllAsync(int pageNumber, int pageSize)
        {
            var products = await _productRepository.GetAllAsync();
            var totalRecords = products.Count();
            var totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
            
            var pagedProducts = products
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var productDtos = _mapper.Map<IEnumerable<ProductDto>>(pagedProducts);

            return new PagedResponse<ProductDto>
            {
                Data = productDtos,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalPages = totalPages,
                TotalRecords = totalRecords
            };
        }

        public async Task<IEnumerable<ProductDto>> SearchAsync(string searchTerm, string category, int page, int pageSize)
        {
            var products = await _productRepository.SearchAsync(searchTerm, category, page, pageSize);
            return _mapper.Map<IEnumerable<ProductDto>>(products);
        }

        public async Task<ProductDto> CreateAsync(ProductDto productDto)
        {
            var existingProduct = await _productRepository.GetBySkuAsync(productDto.Sku);
            if (existingProduct != null)
            {
                throw new ValidationException("A product with this SKU already exists");
            }

            var product = _mapper.Map<Product>(productDto);
            await _productRepository.CreateAsync(product);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<ProductDto>(product);
        }

        public async Task<ProductDto> UpdateAsync(ProductDto productDto)
        {
            var existingProduct = await _productRepository.GetByIdAsync(productDto.Id);
            if (existingProduct == null)
            {
                throw new NotFoundException($"Product with ID {productDto.Id} not found");
            }

            var duplicateProduct = await _productRepository.GetBySkuAsync(productDto.Sku);
            if (duplicateProduct != null && duplicateProduct.Id != productDto.Id)
            {
                throw new ValidationException("A product with this SKU already exists");
            }

            var product = _mapper.Map<Product>(productDto);
            await _productRepository.UpdateAsync(product);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<ProductDto>(product);
        }

        public async Task DeleteAsync(Guid id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
            {
                throw new NotFoundException($"Product with ID {id} not found");
            }

            await _productRepository.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<ProductDto>> GetLowStockProductsAsync()
        {
            var products = await _productRepository.GetLowStockProductsAsync();
            return _mapper.Map<IEnumerable<ProductDto>>(products);
        }
    }
} 