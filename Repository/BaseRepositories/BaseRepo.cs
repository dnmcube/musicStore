
using Infrastructure.Frameworks.Models;

namespace Infrastructure.Frameworks.DataBase.BaseRepositories;

public class BaseRepo
{
    protected readonly Context _context;
    public BaseRepo(Context context)
    {
        _context = context;
    }

    protected async Task AddRange<T>(IEnumerable<T> entity) where T : BaseModel
    {
        foreach (var intem in entity)
        {
            intem.CreateAt = DateTime.Now;
            intem.UpdateAt = DateTime.Now;
            intem.IsDeleted = false;
        }
        await _context.Set<T>().AddRangeAsync(entity);
    }

    protected async Task<Guid> AddAsync<T>(T entity) where T : BaseModel
    {
        if(entity.CreateAt == DateTime.MinValue)
          entity.CreateAt = DateTime.Now;
        
        entity.UpdateAt = DateTime.Now;
        entity.IsDeleted = false;
        await _context.Set<T>().AddAsync(entity);
        return entity.Id;
    }
    
    protected  void UpdateRange<T>(IEnumerable<T> entity) where T : BaseModel
    {
        entity = entity.Select(x =>
        {
            x.UpdateAt = DateTime.Now;
            return x;
        });
         _context.Set<T>().UpdateRange(entity);
    }
    
    protected  async Task Update<T>(T entity) where T : BaseModel
    {
        entity.UpdateAt = DateTime.Now;
        _context.Set<T>().Update(entity);
        await _context.SaveChangesAsync();
    }
    
    protected  async Task Delete<T>(T entity) where T : BaseModel?
    {
        entity.UpdateAt = DateTime.Now;
        entity.IsDeleted = true;
        _context.Set<T>().Update(entity);
        await _context.SaveChangesAsync();
    }
    
    public async Task SaveChangesAsync()
    {
       await _context.SaveChangesAsync();
    }
}