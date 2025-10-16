using Application.Domine;
using Application.Dto;
using Infrastructure.Frameworks.DataBase;
using Infrastructure.Frameworks.DataBase.BaseRepositories;
using Infrastructure.Frameworks.Models;
using Microsoft.EntityFrameworkCore;

namespace Application.Repo;

public class BasketRepo:BaseRepo, IBasketRepo
{

    public BasketRepo(Context context) : base(context)
    {
    }
    
   

    public async Task<object?> GetBasket(Guid UserId)
    {
       return await _context.Basket.Where(x => x.GuestId == UserId).ToListAsync();
    }

    public async Task AddItem(Guid UserId, Guid ProductId)
    {
        await _context.Basket.AddAsync(new Infrastructure.Frameworks.Models.Basket()
            { GuestId = UserId, ProductsId = ProductId });

       await SaveChangesAsync();
    }

    public async Task DeleteItem(Guid UserId, Guid ProductId)
    {
         _context.Basket.Remove(new Infrastructure.Frameworks.Models.Basket()
            { GuestId = UserId, ProductsId = ProductId });
        await SaveChangesAsync();
    }
}