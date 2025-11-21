using Application.Catalog.Categories.Queries.GetListCategory;
using Ardalis.Specification;
using Domain.Entities;

namespace Application.Common.Specifications;

public class CategoryListSpec : Specification<Category, CategoryDto>
{
    public CategoryListSpec()
    {
        //Query
        //    .Select(x => new CategoryDto(x.Id, x.Title, x.Label ?? ""));
    }
}
