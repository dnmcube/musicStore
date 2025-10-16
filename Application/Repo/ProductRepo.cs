using Application.Domine;
using Application.Dto;
using Domain.Enums;
using Infrastructure.Frameworks.DataBase;
using Infrastructure.Frameworks.DataBase.BaseRepositories;
using Infrastructure.Frameworks.Models;
using Microsoft.EntityFrameworkCore;

namespace Application.Repo;

public class ProductRepo:BaseRepo, IProductRepo
{

    public ProductRepo(Context context) : base(context)
    {
    }
    
    public async Task<List<ProductDto>> GetByFilter(ProductFilterDto dto)
    {
        if (dto.Page <= 0)
            dto.Page = 1;

        int pageSize = 10; // количество товаров на странице

        var query = _context.Products
            .Where(x => string.IsNullOrEmpty(dto.Type) || dto.Type == x.Type);

        // Сортировка
        query = dto.PriceUpDown
            ? query.OrderBy(x => x.Price)
            : query.OrderByDescending(x => x.Price);

        // Пагинация (важно: Skip перед Take)
        query = query
            .Skip((dto.Page - 1) * pageSize)
            .Take(pageSize);

        // Выборка нужных полей
        var res = await query.Select(x => new ProductDto()
        {
            Id = x.Id,
            Description = x.Description,
            Name = x.Name,
            Price = x.Price,
            Type = x.Type,
            Image = x.Image
        }).ToListAsync();

        return res;
    }

    public async Task<object> GetDicType()
    {
        var res =  await _context.DicProductsType.Select(x => x.Name).ToListAsync();

        var t = res.Select(x =>
        {
       
                var enumValue = Enum.Parse<ProductTypeEnum>(x);
                return  enumValue.GetDisplayName();
       
        }).ToList();

        return t;
    }
}