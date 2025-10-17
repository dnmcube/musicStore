using Application.Dto;

namespace Application.Domine;

public interface IProduct
{
    Task<object> Get(ProductFilterDto dto);
    Task<object> GetDicType();
}