using Application.Abstractions;
using Application.Common.Services;
using Domain.Entities;
using Domain.Repositories;

namespace Application.Catalog.Products.Services;

public class ProductService : CrudService<Product>, IProductService
{
    public ProductService(IRepository<Product, Guid> productRepository, ICurrentUserProvider currentUser)
        : base(productRepository)
    {

    }
}
