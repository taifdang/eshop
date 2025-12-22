using Microsoft.AspNetCore.Http;

namespace Api.Models.Requests;

public record CreateOptionValueRequestDto(Guid OptionId, string Value, IFormFile? MediaFile = null);
