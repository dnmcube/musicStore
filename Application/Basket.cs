using Application.Domine;
using Application.Dto;
using Application.Repo;

namespace Application;

public class Basket : IBasket
{
    private  IBasketRepo _basketRepo { get; set; }

    public Basket( IBasketRepo basketRepo)
    {
        _basketRepo = basketRepo;
    }
    public async Task<object> GetBasket(Guid UserId)
    {
        var basketList = await _basketRepo.GetBasket(UserId);
        return basketList;
    }
    
    public async Task AddItemBasket(Guid UserId, Guid ProductId)
    {
         await _basketRepo.AddItem(UserId, ProductId);
    }
    
    public async Task DeleteItemBasket(Guid UserId, Guid ProductId)
    {
        await _basketRepo.DeleteItem(UserId, ProductId);
    }
}