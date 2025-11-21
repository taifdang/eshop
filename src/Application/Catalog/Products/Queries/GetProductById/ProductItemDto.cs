using Application.Common.Models;
using AutoMapper;
using Domain.Entities;

namespace Application.Catalog.Products.Queries.GetProductById;

public class ProductItemDto
{
    public Guid Id { get; init; }
    public string Title { get; init; }
    public decimal MinPrice { get; init; }
    public decimal MaxPrice { get; init; }
    public string Description { get; init; }
    public string Category { get; init; }
    public List<ImageLookupDto> Images { get; init; }
    public List<ProductOptionDto> Options { get; init; }

    private class Mapping : Profile
    {
        public Mapping()
        {         
            CreateMap<OptionValue, ProductOptionValueDto>()
                .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Value))
                .ForMember(dest => dest.Image, opt => opt.MapFrom(src =>
                    src.ProductImages != null && src.ProductImages.Any()
                        ? src.ProductImages.First()
                        : null));

            CreateMap<ProductOption, ProductOptionDto>()
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.OptionName))
                .ForMember(dest => dest.Values, opt => opt.MapFrom(src => src.OptionValues));

            CreateMap<Product, ProductItemDto>()
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category.Title))
                .ForMember(dest => dest.MinPrice, opt => opt.MapFrom(src =>
                    src.ProductVariants.Any()
                        ? src.ProductVariants.Min(v => v.Price)
                        : 0m))
                .ForMember(dest => dest.MaxPrice, opt => opt.MapFrom(src =>
                    src.ProductVariants.Any()
                        ? src.ProductVariants.Max(v => v.Price)
                        : 0m))
                .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.ProductImages))
                .ForMember(dest => dest.Options, opt => opt.MapFrom(src => src.ProductOptions));
        }
    }
}
