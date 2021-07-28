using Basket.Domain.Interfaces;
using Basket.Domain.Models;
using Basket.Domain.Utility;
using System.Collections.Generic;
using System.Linq;

namespace Basket.Service
{
    public class Basket : IBasket
    {
        private readonly List<BasketProducts> _products;
        private readonly IDiscountRepo _discountRepo;

        public Basket(IDiscountRepo discountRepo)
        {
            _products = new List<BasketProducts>();
            _discountRepo = discountRepo;
        }

        public List<BasketProducts> Add(Product product)
        {
            if (product.IsValid())
            {
                if (_products.Any(x => x.Product.Code == product.Code))
                {
                    _products.SingleOrDefault(x => x.Product.Code == product.Code).Quantity += 1;
                }
                else
                    _products.Add(new BasketProducts { Product = product, Quantity = 1 });
            }

            return _products;
        }

        public decimal TotalPrice
        {
            get
            {
                decimal totalPrice = 0.0m;

                if (_products.Count > 0)
                {
                    var listOfProductCodesInBasket = _products.Select(x => x.Product.Code).ToList<int>();
                    var listOfProductsWithDiscount = _discountRepo.GetProductWithDiscounts(listOfProductCodesInBasket);

                    if (listOfProductsWithDiscount != null)
                    {
                        var discountedProductCodes = listOfProductsWithDiscount?.Select(x => x.ProductCode).ToList();
                        var listOfDependentProductsCode = listOfProductsWithDiscount.Select(x => x.DependentProductCode).ToList();

                        var listOfNonDiscountAndNonDependentProducts = listOfProductCodesInBasket
                                .Where(x => !discountedProductCodes.Contains(x) && !listOfDependentProductsCode.Contains(x))
                                .ToList();

                        var calculators = _discountRepo.GetDiscountCalculators();

                        foreach (var discount in listOfProductsWithDiscount)
                        {
                            var calculator = calculators.SingleOrDefault(x => x.DiscountType == discount.DiscountType);
                            totalPrice += calculator.Calculate(_products, discount);
                        }

                        totalPrice += _products.Where(x => listOfNonDiscountAndNonDependentProducts.Contains(x.Product.Code)).Sum(x => x.Product.Price);
                    }
                    else
                    {
                        totalPrice += _products.Sum(x => x.Product.Price);
                    }
                }
                return totalPrice;
            }
        }
    }
}
