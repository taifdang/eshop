namespace Api.Models.Requests;

public record GenerateVariantRequestDto(Guid ProductId, Dictionary<Guid, List<Guid>>? OptionValueFilter);