using Basket.Domain.Models;

namespace Basket.Domain.Utility
{
    public static class ProductExtension
    {
        public static bool IsValid(this Product product)
        {
            if (string.IsNullOrWhiteSpace(product?.Name) || product?.Code < 1 || product?.Price <= 0.0m)
                return false;

            return true;
        }
    }
}
