using Microsoft.AspNetCore.Components;
using StepEbay.Main.Client.Base.Layout;
using StepEbay.Main.Common.Models.Product;

namespace StepEbay.Main.Client.Base.Pages
{
    [Route("/productList")]
    [Layout(typeof(EmptyLayout))]
    public partial class Product
    {
        private ProductFilters productFilters { get; set; } = new ProductFilters();
        private bool ShowPreloader { get; set; } = true;
    }
}
