using Application.Catalog.Products.Commands.CreateOption;
using Application.Catalog.Products.Commands.CreateOptionValue;
//using Application.Catalog.Products.Commands.DeleteFile;
using Application.Catalog.Products.Commands.DeleteOptionValue;
using Microsoft.AspNetCore.Mvc;
using Shared.Web;

namespace Api.Controllers.Catalog;

[Route(BaseApiPath + "/product/options")]
public class ProductOptionsController : BaseController
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddOption(CreateOptionCommand command)
    {
        return Ok(await Mediator.Send(command));
    }

    //[HttpDelete]
    //[ProducesResponseType(StatusCodes.Status200OK)]
    //[ProducesResponseType(StatusCodes.Status400BadRequest)]
    //public async Task<IActionResult> DeleteOption(DeleteFileCommand command)
    //{
    //    return Ok(await Mediator.Send(command));
    //}

    [HttpPost("values")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddOptionValues(CreateOptionValueCommand command)
    {
        return Ok(await Mediator.Send(command));
    }

    [HttpDelete("values")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteOptionValues(DeleteOptionValueCommand command)
    {
        return Ok(await Mediator.Send(command));
    }
}
