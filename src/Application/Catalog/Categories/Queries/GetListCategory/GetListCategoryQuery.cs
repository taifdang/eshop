using Application.Common.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Catalog.Categories.Queries.GetListCategory;

public record GetListCategoryQuery : IRequest<List<CategoryDto>>;

public class GetListCategoryQueryHandler : IRequestHandler<GetListCategoryQuery, List<CategoryDto>>
{
    private readonly IRepository<Category> _categoryRepo;
    private readonly IMapper _mapper;
    public GetListCategoryQueryHandler(
        IRepository<Category> categoryRepo,
        IMapper mapper)
    {
        _categoryRepo = categoryRepo;
        _mapper = mapper;
    }
    public async Task<List<CategoryDto>> Handle(GetListCategoryQuery request, CancellationToken cancellationToken)
    {
        var categories = _categoryRepo.ListAsync();

        return _mapper.Map<List<CategoryDto>>(categories);
    }
}
