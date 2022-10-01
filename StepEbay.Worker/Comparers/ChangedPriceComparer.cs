using StepEbay.Common.Models.ProductInfo;

namespace StepEbay.Worker.Comparers
{
    public class ChangedPriceComparer : IEqualityComparer<ChangedPrice>
    {
        public bool Equals(ChangedPrice first, ChangedPrice second)
        {
            return first.ProductId == second.ProductId && first.Price < second.Price;
        }

        public int GetHashCode(ChangedPrice changedPrice)
        {
            return changedPrice.ProductId.GetHashCode();
        }
    }
}
