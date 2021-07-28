using Basket.Domain.Interfaces;
using Basket.Domain.Models;
using Basket.Domain.Utility;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Basket.Infrastructure.DiscountsCalculator
{
    public class BuyNGetOneFree : IDiscountCalculator
    {
        public DiscountType DiscountType => DiscountType.BuyNGetNTHFree;

        public decimal Calculate(List<BasketProducts> products, Discount discount)
        {
            if (products == null || products.Count == 0 || discount?.DiscountType != DiscountType.BuyNGetNTHFree)
            {
                throw new ArgumentException("Invalid argument exception");
            }

            var basketProduct = products.SingleOrDefault(x => x.Product.Code == discount.ProductCode);

            var groupOf = basketProduct.Quantity / (discount.ProductCount + 1);

            return basketProduct.Product.Price * (basketProduct.Quantity - groupOf);
        }
    }
}

