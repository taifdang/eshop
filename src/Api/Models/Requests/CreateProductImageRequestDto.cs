using Microsoft.AspNetCore.Http;


namespace Api.Models.Requests;

public record CreateProductImageRequestDto(Guid ProductId, bool IsMain = false, IFormFile? MediaFile = null);
