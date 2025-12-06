using Application.Catalog.Products.Commands.CreateProduct;
using Application.Catalog.Products.Commands.DeleteProduct;
using Application.Catalog.Products.Commands.UpdateProduct;
using Application.Catalog.Products.Queries.GetListProduct;
using Application.Catalog.Products.Queries.GetProductById;
using Microsoft.AspNetCore.Mvc;
using Shared.Web;

namespace Api.Controllers.Catalog;

[Route(BaseApiPath + "/product")]
public class ProductsController : BaseController
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetList([FromQuery] GetListProductQuery query, CancellationToken cancellationToken)
    {
        return Ok(await Mediator.Send(query));
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetById([FromQuery] GetProductByIdQuery query)
    {
        return Ok(await Mediator.Send(query));
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Add(CreateProductCommand command, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(UpdateProductCommand command)
    {
        return Ok(await Mediator.Send(command));
    }

    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Delete(DeleteProductCommand command)
    {
        return Ok(await Mediator.Send(command));
    }
}
