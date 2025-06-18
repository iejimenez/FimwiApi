using AutoMapper;
using FimwiApi.Core.Entities;
using FimwiApi.Core.Models.DTOs;

namespace FimwiApi.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<BatchDto, Batch>();
            CreateMap<Batch, BatchDto>();
            // Agrega más mapeos según sea necesario
        }
    }
} 