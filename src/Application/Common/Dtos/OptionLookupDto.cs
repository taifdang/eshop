using AutoMapper;

namespace Application.Common.Dtos;

public class OptionLookupDto
{ 
    public string Title { get; init; } // option name
    public Guid OptionValueId { get; set; } // option value id
    public string Value { get; init; } // option value name
    public bool IsImage { get; set; }

    private class Mapping : Profile 
    {
        public Mapping()
        {
            CreateMap<OptionLookupDto, OptionValueLookupDto>()
                .ForMember(d => d.OptionValueId , opt => opt.MapFrom(s => s.OptionValueId));
        }
    }
}