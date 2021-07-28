using Basket.Domain.Models;
using System.Collections.Generic;

namespace Basket.Domain.Interfaces
{
    public interface IDiscountRepo
    {
        List<Discount> GetProductWithDiscounts(List<int> productCodes);

        List<IDiscountCalculator> GetDiscountCalculators();
    }
}
