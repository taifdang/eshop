using Application.Common.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Catalog.Categories.Queries.GetListCategory;

public record GetListCategoryQuery : IRequest<List<CategoryDto>>;

public class GetListCategoryQueryHandler : IRequestHandler<GetListCategoryQuery, List<CategoryDto>>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IMapper _mapper;
    public GetListCategoryQueryHandler(
        IApplicationDbContext dbContext,
        IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<List<CategoryDto>> Handle(GetListCategoryQuery request, CancellationToken cancellationToken)
    {
        var categories = await _dbContext.Categories.ToListAsync();
        return _mapper.Map<List<CategoryDto>>(categories);
    }
}