using Basket.Domain.Utility;

namespace Basket.Domain.Models
{
    public class Discount
    {
        public int Id { get; set; }

        public DiscountType DiscountType { get; set; }

        public int ProductCode { get; set; }

        public int ProductCount { get; set; }

        public decimal Percentage { get; set; }

        public int DependentProductCode { get; set; }

        public int DependentProductCount { get; set; }
    }
}