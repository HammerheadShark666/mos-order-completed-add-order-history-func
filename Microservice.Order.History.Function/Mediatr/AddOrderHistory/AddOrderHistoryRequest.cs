using MediatR;

namespace Microservice.Order.History.Function.MediatR.AddOrderHistory;

public record AddOrderHistoryRequest(Guid Id, Guid CustomerId, string AddressSurname,
                                        string AddressForename, string OrderNumber, string OrderStatus,
                                             decimal Total, DateOnly OrderPlaced,
                                                List<AddOrderHistoryOrderItemRequest> OrderItems,
                                                    AddOrderHistoryAddressRequest Address) : IRequest<AddOrderHistoryResponse>;

public record AddOrderHistoryAddressRequest(string AddressLine1, string AddressLine2, string AddressLine3,
                                                string TownCity, string County, string Postcode,
                                                    string Country) : IRequest<AddOrderHistoryResponse>;

public record AddOrderHistoryOrderItemRequest(Guid ProductId, string Name, string ProductType,
                                                decimal? UnitPrice, int Quantity);