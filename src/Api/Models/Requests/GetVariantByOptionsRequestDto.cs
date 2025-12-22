namespace Api.Models.Requests;

public record GetVariantByOptionsRequestDto(Guid ProductId, List<Guid> OptionValueMap);
