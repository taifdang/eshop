using AutoMapper;
using Domain.Entities;

namespace Application.Catalog.Categories.Queries.GetListCategory;

public class CategoryDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string? Lable { get; set; }

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Category, CategoryDto>();
        }
    }
}

