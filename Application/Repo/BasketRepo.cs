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
       return await _context.Basket.Where(x => x.GuestId == UserId).Include(x => x.Products).Select(x => new
       {
           Id = x.Products.Id,
           Description = x.Products.Description,
           Name = x.Products.Name,
           Price = x.Products.Price,
           Type = x.Products.Type,
           Image = "data:image/png;base64,"+ x.Products.Image
       }).ToListAsync();
    }

    public async Task AddItem(Guid UserId, Guid ProductId)
    {
        await _context.Basket.AddAsync(new Basket()
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