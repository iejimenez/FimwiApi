using AutoMapper;
using FimwiApi.Core.Models.DTOs;
using FimwiApi.Core.Entities;

namespace FimwiApi.Infrastructure.Mappings
{
    public class CustomerMappingProfile : Profile
    {
        public CustomerMappingProfile()
        {
            CreateMap<Customer, CustomerDto>().ReverseMap();
            
            CreateMap<CreateCustomerDto, Customer>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true));
            
            CreateMap<UpdateCustomerDto, Customer>()
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));
        }
    }
} 