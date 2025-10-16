using Application.Dto;

namespace Application.Domine;

public interface IBasket
{
    Task<object> GetBasket(Guid UserId);
    Task AddItemBasket(Guid UserId, Guid ProductId);
    Task DeleteItemBasket(Guid UserId, Guid ProductId);
}