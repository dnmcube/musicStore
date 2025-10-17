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
    
    public async Task<(List<ProductDto>, int)> GetByFilter(ProductFilterDto dto)
    {
        if (dto.Page <= 0)
            dto.Page = 1;

        int pageSize = 10; // количество товаров на странице

        var query = _context.Products
            .Where(x => string.IsNullOrEmpty(dto.Type) || dto.Type == x.Type);

        int totalCount = await query.CountAsync();
        
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
            Image = "data:image/png;base64,"+ x.Image
        }).ToListAsync();
 
        
        return (res, totalCount);
    }

    public async Task<object> GetDicType()
    {
        var res =  await _context.DicProductsType.Select(x => x.Name).ToListAsync();

      

        var categoryType = await _context.Products.GroupBy(x => x.Type).Select(x => new
        {
            type = x.Key,
            image = x.First().Image
        }).ToListAsync();
 
        var t = categoryType.Select(x =>
        {
            // Найдём совпадение по имени типа
            var matched = res.FirstOrDefault(r => r == x.type);

            // Если нашли — парсим в enum
            if (matched != null && Enum.TryParse<ProductTypeEnum>(matched, out var enumValue))
            {
                return new
                {
                    type = enumValue.GetDisplayName(),
                    image =  "data:image/png;base64,"+  x.image
                };
            }

            // fallback — если не нашли, просто вернуть исходное имя
            return new
            {
                type = x.type,
                image ="data:image/png;base64,"+ x.image
            };
        }).ToList();
        
        
        return t;
    }
}