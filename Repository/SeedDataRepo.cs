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
    }

    // public async Task Test()
    // {
    //     var res = await _context.Set<Category>().ToListAsync();
    //     foreach (var item in res)
    //     {
    //         item.TechTitle =  item.Title.Replace(" ", "_");
    //     }
    //
    //     _context.UpdateRange(res);
    //    await  _context.SaveChangesAsync();
    // }

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
    

}