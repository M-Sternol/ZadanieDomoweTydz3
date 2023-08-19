using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZadanieDomoweTydz3
{
    public class WarehouseProductComparer : IComparer<WarehouseProduct>
    {
        public int Compare(WarehouseProduct x, WarehouseProduct y)
        {
            return string.Compare(x.ProductName, y.ProductName, StringComparison.OrdinalIgnoreCase);
        }
    }
}
