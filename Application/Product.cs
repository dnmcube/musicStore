using Application.Domine;
using Application.Dto;
using Application.Repo;

namespace Application;

public class Product : IProduct
{
    private  IProductRepo _productRepo { get; set; }

    public Product(IProductRepo productRepo)
    {
        _productRepo = productRepo;
    }

    public async Task<object> Get(ProductFilterDto dto)
    {
        var productModel = await _productRepo.GetByFilter(dto);
        return new
        { 
            products = productModel.Item1,
           totalPages = productModel.Item2 / 10
        };
    }
    
    public async Task<object> GetDicType()
    {
        var type = await _productRepo.GetDicType();
        return type;
    }
  
}