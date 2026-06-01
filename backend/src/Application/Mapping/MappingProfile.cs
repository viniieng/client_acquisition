using AutoMapper;
using ClientAcquisition.Application.Customers.Dtos;
using ClientAcquisition.Application.Orders.Dtos;
using ClientAcquisition.Application.Reports.Dtos;
using ClientAcquisition.Domain.Entities;

namespace ClientAcquisition.Application.Mapping;

public sealed class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Customer, CustomerResponseDto>();
        CreateMap<OrderItem, OrderItemResponseDto>();
        CreateMap<Order, OrderResponseDto>()
            .ForMember(destination => destination.CustomerFullName, option => option.MapFrom(source => source.Customer != null ? source.Customer.FullName : string.Empty));
        CreateMap<(Customer Customer, int OrderCount, decimal TotalSpent), CustomerSpendingDto>()
            .ConvertUsing(source => new CustomerSpendingDto(source.Customer.Id, source.Customer.FullName, source.OrderCount, source.TotalSpent));
    }
}