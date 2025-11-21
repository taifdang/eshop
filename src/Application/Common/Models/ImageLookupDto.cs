using AutoMapper;
using Domain.Entities;

namespace Application.Common.Models;

public class ImageLookupDto 
{ 
    public Guid Id { get; init; }
    public string? Url { get; init; }
    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<ProductImage, ImageLookupDto>()
                .ForMember(d => d.Url, opt => opt.MapFrom(src => src.ImageUrl));
        }
    }
}