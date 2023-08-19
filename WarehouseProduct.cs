using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZadanieDomoweTydz3
{
    public class WarehouseProduct
    {
        public string ProductName { get; set; }
        public string ProductCategory { get; set; }
        public decimal ProductPrice { get; set; }
        public int AmountOfProducts { get; set; }
        public string Id { get; set; }

        public WarehouseProduct(string productName, string productCategory, decimal productPrice, int amountOfProducts, string id)
        {
            ProductName = productName;
            ProductCategory = productCategory;
            ProductPrice = productPrice;
            AmountOfProducts = amountOfProducts;
            Id = id;
        }
    }
}
