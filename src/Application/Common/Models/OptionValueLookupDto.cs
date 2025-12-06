namespace Application.Common.Models;

public class OptionValueLookupDto
{   
    public Guid OptionId {  get; set; }
    public Guid OptionValueId { get; set; }
    public string Value { get; set; }
}
