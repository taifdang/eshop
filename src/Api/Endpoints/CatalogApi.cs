using Api.Filters;
using Api.Models.Requests;
using Api.Models.Responses;
using Application.Catalog.Categories.Commands.CreateCategory;
using Application.Catalog.Categories.Dtos;
using Application.Catalog.Categories.Queries.GetListCategory;
using Application.Catalog.Products.Commands.BulkUpdateVariant;
using Application.Catalog.Products.Commands.CreateOption;
using Application.Catalog.Products.Commands.CreateOptionValue;
using Application.Catalog.Products.Commands.CreateProduct;
using Application.Catalog.Products.Commands.CreateProductImage;
using Application.Catalog.Products.Commands.CreateVariant;
using Application.Catalog.Products.Commands.DeleteOption;
using Application.Catalog.Products.Commands.DeleteProduct;
using Application.Catalog.Products.Commands.DeleteProductImage;
using Application.Catalog.Products.Commands.DeleteVariant;
using Application.Catalog.Products.Commands.GenerateVariant;
using Application.Catalog.Products.Commands.UpdateProduct;
using Application.Catalog.Products.Commands.UpdateVariant;
using Application.Catalog.Products.Queries.GetAvailableProducts;
using Application.Catalog.Products.Queries.GetListProduct;
using Application.Catalog.Products.Queries.GetProductById;
using Application.Catalog.Products.Queries.GetVariantById;
using Application.Catalog.Products.Queries.GetVariantByOption;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Shared.Constants;

namespace Api.Endpoints;

public static class CatalogApi
{
    public static IEndpointRouteBuilder MapCatalogApi(this IEndpointRouteBuilder builder)
    {
        builder.MapGroup("/api/v1/catalog")
            .MapCatalogApi()
            .WithTags("Catalog Api");

        return builder;
    }

    public static RouteGroupBuilder MapCatalogApi(this RouteGroupBuilder group)
    {
        var categoryGroupApi = group.MapGroup("categories").WithTags("Category");
        var productGroupApi = group.MapGroup("products").WithTags("Product");

        // Category
        categoryGroupApi.MapGet("/", GetListCategory)
            .WithSummary("Get list category")
            .WithDescription("Get list category description");
        categoryGroupApi.MapPost("/", CreateCategory);

        // Product
        productGroupApi.MapGet("/get-available", GetAvailableProducts);
        productGroupApi.MapGet("/", GetListProduct)
            .RequireAuthorization(IdentityConstant.Role.Admin);      
        productGroupApi.MapGet($"/{{id}}", GetProductById);
        productGroupApi.MapPost("/", CreateProduct)
            .RequireAuthorization(IdentityConstant.Role.Admin);
        productGroupApi.MapPut("/", UpdateProduct)
            .RequireAuthorization(IdentityConstant.Role.Admin);
        productGroupApi.MapDelete($"/{{id}}", DeleteProduct)
            .RequireAuthorization(IdentityConstant.Role.Admin);

        // Variant
        productGroupApi.MapGet($"/variants/{{id}}", GetVariantById);
        productGroupApi.MapPost("/variants/by-options", GetVariantByOptions); // POST-based Query
        productGroupApi.MapPost("/variants", CreateVariant);
        productGroupApi.MapPost("/variants/generate", GenerateVariant); // include optionValue
        productGroupApi.MapPut("/variants", UpdateVariant);
        productGroupApi.MapPut("/variants/bulk-update", BulkUpdateVariant);
        productGroupApi.MapDelete("/variants", DeteleVariant);

        // ProductOption
        productGroupApi.MapPost("/options", CreateProducOption);
        productGroupApi.MapDelete($"/options", DeleleProducOption);

        // OptionValue
        productGroupApi.MapPost("options/values", CreateOptionValue)
            .AddEndpointFilter<FileValidationFilter>()
            .DisableAntiforgery();
        productGroupApi.MapDelete("options/values", DeleleOptionValue);

        // ProductImage
        productGroupApi.MapPost("images", CreateProductImage)
            .AddEndpointFilter<FileValidationFilter>()
            .DisableAntiforgery();
        productGroupApi.MapDelete("images", DeleteProductImage);

        return group;
    }

    #region Category
    private static async Task<Results<Ok<List<CategoryDto>>, BadRequest>> GetListCategory(
        IMediator mediator, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetListCategoryQuery(), cancellationToken);

        return TypedResults.Ok(result);
    }

    private static async Task<Results<Ok<Guid>, BadRequest>> CreateCategory(
        IMediator mediator, [AsParameters] CreateCategoryRequestDto request, CancellationToken cancellationToken)
    {
        var command = new CreateCategoryCommand(request.Title, request.UrlSlug);

        var result = await mediator.Send(command, cancellationToken);

        return TypedResults.Ok(result);
    }
    #endregion

    #region Product
    //?
    private static async Task<Results<Ok<PaginatedResult<ProductListDto>>, BadRequest>> GetListProduct(
       IMediator mediator, IMapper mapper, [AsParameters] PaginationRequest request, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetListProductQuery(request.PageIndex, request.PageSize), cancellationToken);

        var response = new PaginatedResult<ProductListDto>(result.PageIndex, result.PageSize, result.Count, result.Items);

        return TypedResults.Ok(response);
    }

    private static async Task<Results<Ok<PaginatedResult<AvailableProductsDto>>, BadRequest>> GetAvailableProducts(
       IMediator mediator, IMapper mapper, [AsParameters] PaginationRequest request, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetAvailableProductsQuery(request.PageIndex, request.PageSize), cancellationToken);

        var response = new PaginatedResult<AvailableProductsDto>(result.PageIndex, result.PageSize, result.Count, result.Items);

        return TypedResults.Ok(response);
    }

    private static async Task<Results<Ok<ProductItemDto>, BadRequest>> GetProductById(
       IMediator mediator, Guid id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetProductByIdQuery(id), cancellationToken);

        return TypedResults.Ok(result);
    }

    private static async Task<Results<Created, BadRequest>> CreateProduct(
       IMediator mediator, [AsParameters] CreateProductRequestDto request, CancellationToken cancellationToken)
    {
        var command = new CreateProductCommand(request.CategoryId, request.Name, request.Description, request.UrlSlug);

        await mediator.Send(command, cancellationToken);

        return TypedResults.Created();
    }

    private static async Task<Results<NoContent, BadRequest>> UpdateProduct(
       IMediator mediator, [AsParameters] UpdateProductRequestDto request, CancellationToken cancellationToken)
    {
        var command = new UpdateProductCommand(request.Id, request.CategoryId, request.Title, request.Description);

        await mediator.Send(command, cancellationToken);

        return TypedResults.NoContent();
    }

    private static async Task<Results<NoContent, BadRequest>> DeleteProduct(
       IMediator mediator, Guid id, CancellationToken cancellationToken)
    {
        await mediator.Send(new DeleteProductCommand(id), cancellationToken);

        return TypedResults.NoContent();
    }
    #endregion
     
    #region Variant
    private static async Task<Results<Ok<VariantDto>, BadRequest>> GetVariantById(
       IMediator mediator, Guid id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetVariantByIdQuery(id), cancellationToken);

        return TypedResults.Ok(result);
    }

    private static async Task<Results<Ok<VariantItemListDto>, BadRequest>> GetVariantByOptions(
       IMediator mediator, [AsParameters] GetVariantByOptionsRequestDto request , CancellationToken cancellationToken)
    {
        var command = new GetVariantByOptionQuery(request.ProductId, request.OptionValueMap);

        var result = await mediator.Send(command, cancellationToken);

        return TypedResults.Ok(result);
    }

    private static async Task<Results<Created, BadRequest>> CreateVariant(
       IMediator mediator, [AsParameters] CreateVariantRequestDto request, CancellationToken cancellationToken)
    {
        var command = new CreateVariantCommand(request.ProductId, request.RegularPrice, request.Quantity);

        var result = await mediator.Send(command, cancellationToken);

        return TypedResults.Created();
    }

    private static async Task<Results<Created, BadRequest>> GenerateVariant(
       IMediator mediator, [AsParameters] GenerateVariantRequestDto request, CancellationToken cancellationToken)
    {
        var command = new GenerateVariantCommand(request.ProductId, request.OptionValueFilter);

        var result = await mediator.Send(command, cancellationToken);

        return TypedResults.Created();
    }

    private static async Task<Results<NoContent, BadRequest>> UpdateVariant(
       IMediator mediator, [AsParameters] UpdateVariantRequestDto request, CancellationToken cancellationToken)
    {
        var command = new UpdateVariantCommand(request.ProductId, request.Id, request.RegularPrice, request.Quantity);

        var result = await mediator.Send(command, cancellationToken);

        return TypedResults.NoContent();
    }

    private static async Task<Results<Ok<Guid>, BadRequest>> BulkUpdateVariant(
       IMediator mediator, [AsParameters] BulkUpdateVariantRequestDto request, CancellationToken cancellationToken)
    {
        var command = new BulkUpdateVariantCommand(request.ProductId, request.Price, request.Quantity, request.Sku, request.IsActive);

        var result = await mediator.Send(command, cancellationToken);

        return TypedResults.Ok(result);
    }

    private static async Task<Results<NoContent, BadRequest>> DeteleVariant(
       IMediator mediator, Guid Id, Guid ProductId, CancellationToken cancellationToken)
    {
        var command = new DeleteVariantCommand(Id, ProductId);

        var result = await mediator.Send(command, cancellationToken);

        return TypedResults.NoContent();
    }

    #endregion

    #region ProducOption
    private static async Task<Results<Created, BadRequest>> CreateProducOption(
       IMediator mediator, [AsParameters] CreateOptionRequestDto request, CancellationToken cancellationToken)
    {
        var command = new CreateOptionCommand(request.ProductId, request.OptionName, request.AllowImage);

        var result = await mediator.Send(command, cancellationToken);

        return TypedResults.Created();
    }

    private static async Task<Results<NoContent, BadRequest>> DeleleProducOption(
       IMediator mediator, Guid OptionId, Guid ProductId, CancellationToken cancellationToken)
    {
        var command = new DeleteOptionCommand(OptionId, ProductId);

        var result = await mediator.Send(command, cancellationToken);

        return TypedResults.NoContent();
    }
    #endregion  

    #region OptionValue
    private static async Task<Results<Created, BadRequest>> CreateOptionValue(
      IMediator mediator, [FromForm] CreateOptionValueRequestDto request, CancellationToken cancellationToken)
    {
        var command = new CreateOptionValueCommand(request.ProductId, request.OptionId, request.Value, request.MediaFile);

        var result = await mediator.Send(command, cancellationToken);

        return TypedResults.Created();
    }

    private static async Task<Results<NoContent, BadRequest>> DeleleOptionValue(
       IMediator mediator, Guid OptionId, Guid ProductId, CancellationToken cancellationToken)
    {
        var command = new DeleteOptionCommand(OptionId, ProductId);

        var result = await mediator.Send(command, cancellationToken);

        return TypedResults.NoContent();
    }
    #endregion

    #region ProductImage
    private static async Task<Results<Created, BadRequest>> CreateProductImage(
        IMediator mediator, [FromForm] CreateProductImageRequestDto request, CancellationToken cancellationToken)
    {
        var command = new CreateProductImageCommand(request.ProductId, request.IsMain, request.MediaFile);

        await mediator.Send(command, cancellationToken);

        return TypedResults.Created();   
    }

    private static async Task<Results<NoContent, BadRequest>> DeleteProductImage(
       IMediator mediator, Guid Id, Guid ProductId, CancellationToken cancellationToken)
    {
        await mediator.Send(new DeleteProductImageCommand(Id, ProductId), cancellationToken);

        return TypedResults.NoContent();
    }
    #endregion
}
