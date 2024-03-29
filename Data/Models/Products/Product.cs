﻿using StepEbay.Data.Models.Default;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StepEbay.Data.Models.Products
{
    public class Product : IDbServiceEntity<int>
    {
        [Key]
        public int Id { get; set; }
        public DateTime DateCreated { get; set; }
        public string Image { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public bool ByNow { get; set; }
        public int Count { get; set; }

        [ForeignKey("Category")]
        public int CategoryId { get; set; }

        [ForeignKey("ProductState")]
        public int ProductStateId { get; set; }

        public virtual ProductState ProductState { get; set; }
        public virtual Category Category { get; set; }
    }
}