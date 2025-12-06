using Application.Catalog.Categories.Commands.CreateCategory;
using Application.Catalog.Categories.Queries.GetListCategory;
using Microsoft.AspNetCore.Mvc;
using Shared.Web;

namespace Api.Controllers.Catalog;

[Route(BaseApiPath + "/product/category")]
public class CategoriesController : BaseController
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetListCategory(GetListCategoryQuery command)
    {
        return Ok(await Mediator.Send(command));
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AddCategory(CreateCategoryCommand command)
    {
        return Ok(await Mediator.Send(command));
    }
}
