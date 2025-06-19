using AutoMapper;
using FimwiApi.Core.Entities;
using FimwiApi.Core.Models.DTOs;

namespace FimwiApi.Core.Mappings
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, UserDto>();
            CreateMap<Product, ProductDto>();
            CreateMap<Batch, BatchDto>();
            CreateMap<Supplier, SupplierDto>();
            CreateMap<SupplierDto, Supplier>();
            CreateMap<PurchaseOrder, PurchaseOrderDto>();
            CreateMap<PurchaseOrderItem, PurchaseOrderItemDto>();
            CreateMap<Customer, CustomerDto>();
            CreateMap<Invoice, InvoiceDto>();
            CreateMap<InvoiceItem, InvoiceItemDto>();
        }
    }
} 