using Basket.Domain.Interfaces;
using Basket.Domain.Models;
using Basket.Domain.Utility;
using Basket.Infrastructure.DiscountsCalculator;
using System.Collections.Generic;
using System.Linq;

namespace Basket.Infrastructure.Repo
{
    public class DiscountRepo : IDiscountRepo
    {
        private readonly List<Discount> _discounts;
        private readonly List<IDiscountCalculator> _discountCalculators;
        public DiscountRepo()
        {
            _discounts = new List<Discount> {
                new Discount
                {
                    Id = 1,
                    ProductCode = 1,
                    ProductCount = 2,
                    DependentProductCode = 3,
                    DependentProductCount = 1,
                    DiscountType = DiscountType.BuyNGetNHalf
                },
                new Discount
                {
                    Id = 1,
                    ProductCode = 2,
                    ProductCount = 3,
                    DiscountType = DiscountType.BuyNGetNTHFree
                }
            };

            _discountCalculators = new List<IDiscountCalculator>
            {
                new BuyNGetOneFree(),
                new BuyNGetAnotherProductOneHalfPrice()
            };
        }

        public List<IDiscountCalculator> GetDiscountCalculators()
        {
            return _discountCalculators;
        }

        public List<Discount> GetProductWithDiscounts(List<int> productCodes)
        {
            // This should come from database
            return _discounts.Where(x => productCodes.Contains(x.ProductCode)).ToList();
        }
    }
}
