using Application.Dto;

namespace Application.Domine;

public interface IProduct
{
    Task<List<ProductDto>> Get(ProductFilterDto dto);
    Task<object> GetDicType();
}