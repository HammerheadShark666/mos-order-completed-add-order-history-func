using AutoMapper;
using Microservice.Order.History.Function.Domain;
using Microservice.Order.History.Function.MediatR.AddOrderHistory;

namespace Microservice.Order.History.Function.MediatR.AddOrder;

public class AddOrderHistoryMapper : Profile
{
    public AddOrderHistoryMapper()
    {
        base.CreateMap<AddOrderHistoryOrderItemRequest, OrderItem>();   
        base.CreateMap<AddOrderHistoryRequest, OrderHistory>();

        base.CreateMap<AddOrderHistoryRequest, OrderHistory>()
            .ForMember(m => m.AddressLine1, o => o.MapFrom(s => s.Address.AddressLine1))
            .ForMember(m => m.AddressLine2, o => o.MapFrom(s => s.Address.AddressLine2))
            .ForMember(m => m.AddressLine3, o => o.MapFrom(s => s.Address.AddressLine3))
            .ForMember(m => m.TownCity, o => o.MapFrom(s => s.Address.TownCity))
            .ForMember(m => m.County, o => o.MapFrom(s => s.Address.County))
            .ForMember(m => m.Country, o => o.MapFrom(s => s.Address.Country))
            .ForMember(m => m.Postcode, o => o.MapFrom(s => s.Address.Postcode));
    }
}