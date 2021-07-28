using Basket.Domain.Interfaces;
using Basket.Domain.Models;
using Basket.Infrastructure.DiscountsCalculator;
using Basket.Infrastructure.Repo;
using FluentAssertions;
using System.Collections.Generic;
using Xunit;

namespace Basket.Service.IntegrationTest
{
    public class BasketServiceIntegrationTest
    {
        private readonly IDiscountRepo _discountRepo;
        private readonly IBasket _basket;

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

        public BasketServiceIntegrationTest()
        {
            var discountCalculators = new List<IDiscountCalculator>
            {
                new BuyNGetOneFree(),
                new BuyNGetAnotherProductOneHalfPrice()
            };

            _discountRepo = new DiscountRepo(discountCalculators);
            _basket = new Basket(_discountRepo);
        }

        [Fact]
        public void OneButterOneBreadOneMilk_ShouldReturnTotal_Successfully()
        {
            _basket.Add(_butter);
            _basket.Add(_bread);
            _basket.Add(_milk);

            _basket.TotalPrice.Should().Be(2.95m);
        }

        [Fact]
        public void TwoButterTwoBread_ShouldReturnTotal_Successfully()
        {
            _basket.Add(_butter);
            _basket.Add(_butter);
            _basket.Add(_bread);
            _basket.Add(_bread);

            _basket.TotalPrice.Should().Be(3.10m);
        }

        [Fact]
        public void FourMilk_ShouldReturnTotal_Successfully()
        {
            for (int i = 0; i < 4; i++)
            {
                _basket.Add(_milk);
            }

            _basket.TotalPrice.Should().Be(3.45m);
        }

        [Fact]
        public void TwoButterOneBreadEightMilk_ShouldReturnTotal_Successfully()
        {
            _basket.Add(_butter);
            _basket.Add(_butter);
            _basket.Add(_bread);

            for (int i = 0; i < 8; i++)
            {
                _basket.Add(_milk);
            }

            _basket.TotalPrice.Should().Be(9.00m);
        }
    }
}
