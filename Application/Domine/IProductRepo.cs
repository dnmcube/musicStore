using Application.Dto;

namespace Application.Domine;

public interface IProductRepo
{
    Task<List<ProductDto>> GetByFilter(ProductFilterDto dto);
    Task<object> GetDicType();
}