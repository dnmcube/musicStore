using Application.Dto;

namespace Application.Domine;

public interface IBasketRepo
{
    Task<object> GetBasket(Guid UserId);

    Task AddItem(Guid UserId, Guid ProductId);
    Task DeleteItem(Guid UserId, Guid ProductId);
}