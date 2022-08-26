﻿namespace StepEbay.Main.Common.Models.Product
{
    public class ProductRequestDto
    {
        public DateTime DateCreated { get; set; }
        public string Image { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public bool ByNow { get; set; }
        public int Count { get; set; }
        public int IdCategory { get; set; }
        public int IdProductState { get; set; }
    }
}