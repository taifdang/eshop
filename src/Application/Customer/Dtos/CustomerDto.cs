using AutoMapper;

namespace Application.Customer.Dtos;

public class CustomerDto 
{
    public Guid Id { get; init; }
    public string? FullName { get; init; }
    public string? Email { get; init; }
    public string? Phone { get; init; }
    public string? Address { get; init; }

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Domain.Entities.Customer, CustomerDto>()
                .ForMember(x => x.Phone, y => y.MapFrom(src => src.PhoneNumber));          
        }
    }
}

