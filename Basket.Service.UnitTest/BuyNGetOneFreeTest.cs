using Basket.Domain.Models;
using Basket.Domain.Utility;
using Basket.Infrastructure.DiscountsCalculator;
using FluentAssertions;
using System;
using System.Collections.Generic;
using Xunit;

namespace Basket.DiscountCalculator.UnitTest
{
    public class BuyNGetOneFreeTest
    {
        private readonly BuyNGetOneFree _buyNGetNFree = new BuyNGetOneFree();

        private readonly static Product _butter = new Product
        {
            Name = "Butter",
            Code = 1,
            Price = 0.80m
        };
        private readonly static Product _milk = new Product
        {
            Name = "Milk",
            Code = 2,
            Price = 1.15m
        };
        private readonly static Product _bread = new Product
        {
            Name = "Bread",
            Code = 3,
            Price = 1.00m
        };

        private readonly static Discount _discount = new Discount
        {
            Id = 1,
            DiscountType = DiscountType.BuyNGetNTHFree,
            ProductCode = 1,
            ProductCount = 3
        };

        private readonly static Discount _invalidDiscount = new Discount
        {
            Id = 1,
            DiscountType = DiscountType.None,
            ProductCode = 1,
            ProductCount = 3
        };

        private readonly static List<BasketProducts> _basketWithOneProductQtyZero = new List<BasketProducts>
        {
            new BasketProducts{ Product = _butter, Quantity = 0 }
        };

        private readonly static List<BasketProducts> _basketWithOneProductQtyOne = new List<BasketProducts>
        {
            new BasketProducts{ Product = _butter, Quantity = 1 }
        };

        private readonly static List<BasketProducts> _basketWithOneProductQtyTwo = new List<BasketProducts>
        {
            new BasketProducts{ Product = _butter, Quantity = 2 }
        };

        private readonly static List<BasketProducts> _basketWithOneProductQtyThree = new List<BasketProducts>
        {
            new BasketProducts{ Product = _butter, Quantity = 3 }
        };

        private readonly static List<BasketProducts> _basketWithOneProductQtyFour = new List<BasketProducts>
        {
            new BasketProducts{ Product = _butter, Quantity = 4 }
        };

        private readonly static List<BasketProducts> _basketWithOneProductQtyTen = new List<BasketProducts>
        {
            new BasketProducts{ Product = _butter, Quantity = 10 }
        };

        private readonly static List<BasketProducts> _basketWithAllProductsWithQuantityOne = new List<BasketProducts>
        {
            new BasketProducts{ Product = _butter, Quantity = 1 },
            new BasketProducts{ Product = _milk, Quantity = 1 },
            new BasketProducts{ Product = _bread, Quantity = 1 }
        };

        public static TheoryData<List<BasketProducts>, Discount, decimal> ValidInput =>
            new TheoryData<List<BasketProducts>, Discount, decimal>
            {
                {_basketWithOneProductQtyZero, _discount, 0.0m },
                { _basketWithOneProductQtyOne, _discount, _butter.Price },
                { _basketWithOneProductQtyTwo, _discount, _butter.Price * 2 },
                { _basketWithOneProductQtyThree, _discount, _butter.Price * 3 },
                { _basketWithOneProductQtyFour, _discount, _butter.Price * 3 },
                { _basketWithOneProductQtyTen, _discount, _butter.Price * 8 },
                { _basketWithAllProductsWithQuantityOne, _discount, _butter.Price }
            };

        public static TheoryData<List<BasketProducts>, Discount> InValidInput =>
            new TheoryData<List<BasketProducts>, Discount>
            {
                {_basketWithOneProductQtyOne, null },
                { null, _discount },
                { null, null },
                { _basketWithOneProductQtyOne, _invalidDiscount }
            };

        [Theory]
        [MemberData(nameof(ValidInput))]
        public void Calculate_ShouldReturnTotalSuccessfully(List<BasketProducts> basketProducts, Discount discount, decimal expectedTotal)
        {
            var totalPrice = _buyNGetNFree.Calculate(basketProducts, discount);

            totalPrice.Should().Be(expectedTotal);
        }

        [Theory]
        [MemberData(nameof(InValidInput))]
        public void Calculate_ShouldThrowException(List<BasketProducts> basketProducts, Discount discount)
        {
            Assert.Throws<ArgumentException>(() => _buyNGetNFree.Calculate(basketProducts, discount));
        }
    }
}
