using Application.Catalog.Products.Commands.CreateVariant;
using Application.Catalog.Products.Commands.DeleteVariant;
using Application.Catalog.Products.Commands.GenerateVariant;
using Application.Catalog.Products.Commands.UpdateVariant;
using Application.Catalog.Products.Queries.GetVariantById;
using Application.Catalog.Products.Queries.GetVariantByOption;
using Microsoft.AspNetCore.Mvc;
using Shared.Web;

namespace Api.Controllers.Catalog;

[Route(BaseApiPath + "/product/variants")]
public class ProductVariantsController : BaseController
{
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetById([FromQuery]GetVariantByIdQuery query)
    {
        return Ok(await Mediator.Send(query));
    }

    [HttpGet("by-option-values")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetByOption([FromQuery] GetVariantByOptionQuery query)
    {
        return Ok(await Mediator.Send(query));
    }

    [HttpPost("generate")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GenerateVariants([FromBody] GenerateVariantCommand command)
    {
        return Ok(await Mediator.Send(command));
    }
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateVariant(CreateVariantCommand command)
    {
        return Ok(await Mediator.Send(command));
    }
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateVariant(UpdateVariantCommand command)
    {
        return Ok(await Mediator.Send(command));
    }
    //[HttpPut("update-many")]
    //[ProducesResponseType(StatusCodes.Status200OK)]
    //[ProducesResponseType(StatusCodes.Status400BadRequest)]
    //public async Task<IActionResult> UpdateManyVariant(UpdateVariantsCommand command)
    //{
    //    return Ok(await Mediator.Send(command));
    //}

    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteVariant(DeleteVariantCommand command)
    {
        return Ok(await Mediator.Send(command));
    }
}
