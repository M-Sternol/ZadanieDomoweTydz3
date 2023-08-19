namespace ZadanieDomoweTydz3
{
    internal class Program
    {
        static void Main(string[] args)
        {
            WarehouseCRM<WarehouseProduct> warehouseCRM = new WarehouseCRM<WarehouseProduct>();
            warehouseCRM.Run();

        }
    }
}