using Basket.Domain.Models;
using System.Collections.Generic;

namespace Basket.Domain.Interfaces
{
    public interface IBasket
    {
        List<BasketProducts> Add(Product product);

        List<BasketProducts> Add(List<Product> product);

        decimal TotalPrice { get; }
    }
}
