using System.Runtime.CompilerServices;
using Controller.Seeds.Interfaces;
using Domain.Enums;
using Infrastructure.Frameworks.DataBase;
using Infrastructure.Frameworks.Models;
using Microsoft.EntityFrameworkCore;

namespace Inrastructure.Seeds.Repositories;

public class SeedDataRepo:ISeedDataRepo
{
    private readonly Context _context;
    
    public SeedDataRepo(Context context)
    {
        _context = context;
    }

    public async Task Execute()
    {
        await Roles();
        await ProductType();
    }



    public async Task Roles()
    {
        // 1. Берем все роли из БД
        var existingRoles = await _context.Set<Role>().ToListAsync();

        // 2. Получаем все значения enum как строки
        var enumRoleNames = Enum.GetNames(typeof(RolesEnum)).ToList();

        // 3. Находим те, которых нет в БД
        var rolesToAdd = enumRoleNames
            .Where(enumName => !existingRoles.Any(dbRole => dbRole.Name == enumName))
            .Select(enumName => new Role { Name = enumName, CreateAt = DateTime.Now, UpdateAt = DateTime.Now})
            .ToList();
        
        
        if (rolesToAdd.Any())
        {
            await _context.AddRangeAsync(rolesToAdd);
            await _context.SaveChangesAsync();
        }
    }
    
    public async Task ProductType()
    {
        // 1. Берем все роли из БД
        var existingProductType = await _context.Set<DicProductsType>().ToListAsync();

        // 2. Получаем все значения enum как строки
        var enumProductType = Enum.GetNames(typeof(ProductTypeEnum)).ToList();

        // 3. Находим те, которых нет в БД
        var TypeToAdd = enumProductType
            .Where(enumName => !existingProductType.Any(x => x.Name == enumName))
            .Select(enumName => new DicProductsType() { Name = enumName, CreateAt = DateTime.Now, UpdateAt = DateTime.Now})
            .ToList();
        
        
        if (TypeToAdd.Any())
        {
            await _context.AddRangeAsync(TypeToAdd);
            await _context.SaveChangesAsync();
        }
    }
    

}