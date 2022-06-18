
using StepEbay.Data.Common.Services.Default;
using StepEbay.Data.Models.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StepEbay.Data.Common.Services.ProductDbServices
{
    public interface IProductDbService : IDefaultDbService<int, Product> 
    {
        public Task<List<Product>> GetProductsByCategory(int categoryId);

        public Task<bool> AnyProductsByCategory(int categoryId);

        public Task<bool> AnyProductsByTitle(string productTitle);
    }
}
