using Basket.Domain.Interfaces;
using Basket.Domain.Models;
using Basket.Domain.Utility;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Basket.Infrastructure.DiscountsCalculator
{
    public class BuyNGetAnotherProductOneHalfPrice : IDiscountCalculator
    {
        public DiscountType DiscountType => DiscountType.BuyNGetNHalf;

        public decimal Calculate(List<BasketProducts> products, Discount discount)
        {
            if (products == null || products.Count == 0 || discount?.DiscountType != DiscountType.BuyNGetNHalf)
            {
                throw new ArgumentException("Invalid argument exception");
            }

            var basketProduct = products.SingleOrDefault(x => x.Product.Code == discount.ProductCode);
            var productQuantity = basketProduct.Quantity;
            var totalPrice = basketProduct.Product.Price * productQuantity;

            var dependentProduct = products.SingleOrDefault(x => x.Product.Code == discount.DependentProductCode);
            if (dependentProduct != null)
            {
                var groupOf = productQuantity / discount.ProductCount;
                var deductionAmount = groupOf * (dependentProduct.Product.Price / 2);
                var actualAmount = dependentProduct.Product.Price * dependentProduct.Quantity;
                totalPrice += deductionAmount <= actualAmount ? actualAmount - deductionAmount : 0;
            }

            return totalPrice;
        }
    }
}
