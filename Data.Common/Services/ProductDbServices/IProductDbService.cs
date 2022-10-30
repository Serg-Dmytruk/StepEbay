﻿using StepEbay.Data.Common.Services.Default;
using StepEbay.Data.Models.Products;
using StepEbay.Main.Common.Models.Product;

namespace StepEbay.Data.Common.Services.ProductDbServices
{
    public interface IProductDbService : IDefaultDbService<int, Product> 
    {
        Task<List<Product>> GetProductsByCategory(int categoryId);
        Task<bool> AnyProductsByCategory(int categoryId);
        Task<bool> AnyProductsByState(int stateId);
        Task<bool> AnyProductsByTitle(string productTitle);
        Task<List<Product>> GetProducts();
        Task<int> GetCount();
        Task<List<Product>> GetFilteredProducts(ProductFilterInfo info);
        Task<List<Product>> GetProductForInfo(List<int> products);
        Task<List<int>> GetSearchIds(string product);
        Task<List<Product>> GetProductByIds(SearchIdsDto products);
    }
}
