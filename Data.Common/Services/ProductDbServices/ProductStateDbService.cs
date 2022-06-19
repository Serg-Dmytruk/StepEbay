﻿using Microsoft.EntityFrameworkCore;
using StepEbay.Data.Common.Services.Default;
using StepEbay.Data.Models.Products;

namespace StepEbay.Data.Common.Services.ProductDbServices
{
    public class ProductStateDbService : DefaultDbService<byte, ProductState>, IProductStateDbService
    {
        private readonly ApplicationDbContext _context;
        public ProductStateDbService(ApplicationDbContext context)
            :base(context)
        {
            _context = context;
        }
        public async Task<bool> AnyStateByName(string name)
        {
            return await _context.ProductStates.AnyAsync(p => p.Name == name);
        }
    }
}