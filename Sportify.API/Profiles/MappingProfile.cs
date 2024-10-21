using AutoMapper;
using Core.Entities;
using Sportify.API.DTOs;

namespace Sportify.API.Profiles;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Product, ProductDTO>()
                   .ForMember(dest => dest.ProductBrand, opt => opt.MapFrom(src => src.ProductBrand.Name))
                   .ForMember(dest => dest.ProductType, opt => opt.MapFrom(src => src.ProductType.Name));
    }
}
