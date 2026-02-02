using AutoMapper;
using Domain.Entities;

namespace Application.Common.Dtos;

public class ImageLookupDto 
{ 
    public Guid Id { get; set; }
    public string? Url { get; set; }
    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Image, ImageLookupDto>()
                .ForMember(d => d.Url, opt => opt.MapFrom(src => src.BaseUrl + "/" + src.FileName));
        }
    }
}