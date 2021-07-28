using Basket.Domain.Models;
using Basket.Domain.Utility;
using System.Collections.Generic;

namespace Basket.Domain.Interfaces
{
    public interface IDiscountCalculator
    {
        DiscountType DiscountType { get; }

        decimal Calculate(List<BasketProducts> products, Discount discount);
    }
}
