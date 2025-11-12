
using Application.Catalog.Products.Commands.CreateProduct;
using AutoMapper;
using Domain.Entities;
namespace Application.Common.Mapping;

public class ProductProfile : Profile
{
   public ProductProfile()
   {
        CreateMap<Product, CreateProductCommand>().ReverseMap();

        //CreateMap<AddProductRequest, Product>();

        //CreateMap<UpdateProductRequest, Product>();
    }
}
