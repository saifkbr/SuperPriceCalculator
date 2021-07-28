using Basket.Domain.Interfaces;
using Basket.Domain.Models;
using Basket.Domain.Utility;
using FluentAssertions;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace Basket.Service.UnitTest
{
    public class BasketServiceUnitTest
    {
        private readonly Mock<IDiscountRepo> _discountRepo;

        private readonly static Product _butter = new Product
        {
            Name = "Butter",
            Code = 1,
            Price = 0.80m
        };
        private readonly static Product _missingName = new Product
        {
            Name = "",
            Code = 1,
            Price = 0.80m
        };
        private readonly static Product _missingCode = new Product
        {
            Name = "",
            Code = 0,
            Price = 0.80m
        };
        private readonly static Product _missingPrice = new Product
        {
            Name = "",
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

        private readonly static List<Product> _productsNull = new List<Product>
        {
            null
        };

        private readonly static List<Product> _productsOne = new List<Product>
        {
            _butter
        };

        private readonly static List<Product> _productsSameTwice = new List<Product>
        {
            _butter, _butter
        };

        private readonly static List<Product> _productsTwo = new List<Product>
        {
            _butter, _milk
        };

        private readonly static List<Product> _productsThree = new List<Product>
        {
            _butter,_milk,_bread
        };

        private readonly static List<Product> _productsMissingName = new List<Product>
        {
            _missingName
        };

        private readonly static List<Product> _productsMissingCode = new List<Product>
        {
            _missingCode
        };

        private readonly static List<Product> _productsMissingPrice = new List<Product>
        {
            _missingPrice
        };
        public static TheoryData<List<Product>, int> Products =>
            new TheoryData<List<Product>, int>
            {
                { _productsNull, 0 },
                { _productsOne, 1 },
                { _productsTwo, 2 },
                { _productsThree, 3 },
                { _productsSameTwice, 1 },
                { _productsMissingName, 0 },
                { _productsMissingCode, 0 },
                { _productsMissingPrice, 0 }
            };

        public BasketServiceUnitTest()
        {
            _discountRepo = new Mock<IDiscountRepo>();
        }

        [Theory]
        [MemberData(nameof(Products))]
        public void AddProduct(List<Product> products, int quantity)
        {
            var basket = new Basket(_discountRepo.Object);
            List<BasketProducts> basketProducts = new List<BasketProducts>();

            foreach (var item in products)
            {
                basketProducts = basket.Add(item);
            }

            basketProducts.Should().HaveCount(quantity);
        }

        [Fact]
        public void TotalPrice_OneNonDiscountProductAdded_ShouldReturnValidTotalPrice()
        {
            var basket = new Basket(_discountRepo.Object);
            var products = basket.Add(_butter);
            var totalPrice = basket.TotalPrice;

            products.Should().HaveCount(1);
            totalPrice.Should().Be(_butter.Price);
        }

        [Fact]
        public void TotalPrice_MultipleNonDiscountProductAdded_ShouldReturnValidTotalPrice()
        {
            var basket = new Basket(_discountRepo.Object);
            basket.Add(_butter);
            basket.Add(_milk);
            basket.Add(_bread);
            var totalPrice = basket.TotalPrice;

            totalPrice.Should().Be(_butter.Price + _milk.Price + _bread.Price);
        }
    }
}
