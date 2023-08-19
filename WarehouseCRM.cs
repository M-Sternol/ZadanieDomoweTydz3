using System;
using System.Collections.Generic;
using System.IO;

namespace ZadanieDomoweTydz3
{
    public class WarehouseCRM<T> : ICRUD<T> where T : WarehouseProduct
    {
        private readonly string dataFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "file", "Warehouse.txt");
        private List<T> warehouselist = new List<T>();

        public void Run()
        {
            LoadWarehouseFromFile();
            bool isRunning = true;
            while (isRunning)
            {
                Console.WriteLine("==== Zarządzanie magazynem ====" + "\n");
                Console.WriteLine("1. Dodaj Produkt");
                Console.WriteLine("2. Usuń Produkt");
                Console.WriteLine("3. Modyfikacja Produktów");
                Console.WriteLine("4. Lista Produktów");
                Console.WriteLine("0. Exit" + "\n");
                string warehouseManagement = Console.ReadLine();
                switch (warehouseManagement)
                {
                    case "1":
                        Console.Clear();
                        AddProduct();
                        break;

                    case "2":
                        Console.Clear();
                        DeleteProduct();
                        break;

                    case "3":
                        Console.Clear();
                        ModifyProduct();
                        break;

                    case "4":
                        Console.Clear();
                        DisplayProducts();
                        break;
                    case "0":
                        Console.Clear();
                        return;

                    default:
                        Console.Clear();
                        Console.WriteLine("Nieprawidłowa operacja! Spróbuj ponownie!");
                        break;
                }
            }
        }

        public void Create(T product)
        {
            warehouselist.Add(product);
            SaveWarehouseToFile();
        }

        public T Read(string id)
        {
            return warehouselist.Find(p => p.Id.Equals(id, StringComparison.OrdinalIgnoreCase));
        }

        public void Update(string id, T updatedProduct)
        {
            T existingProduct = Read(id);
            if (existingProduct != null)
            {
                existingProduct.ProductName = updatedProduct.ProductName;
                existingProduct.ProductCategory = updatedProduct.ProductCategory;
                existingProduct.ProductPrice = updatedProduct.ProductPrice;
                existingProduct.AmountOfProducts = updatedProduct.AmountOfProducts;
                SaveWarehouseToFile();
            }
        }

        public void Delete(string id)
        {
            T productToDelete = Read(id);
            if (productToDelete != null)
            {
                warehouselist.Remove(productToDelete);
                SaveWarehouseToFile();
            }
        }

        public List<T> GetAll()
        {
            return warehouselist;
        }

        private void AddProduct()
        {
            bool isRunning = true;
            while (isRunning)
            {
                Console.Clear();
                Console.WriteLine("==== Dodaj produkt do magazynu ====" + "\n");
                Console.Write("Nazwa Produktu: ");
                string productName = Console.ReadLine();
                Console.Write("Ilość: ");
                int amountOfProducts = int.Parse(Console.ReadLine());

                Console.Write("Kategoria Produktu: ");
                string productCategory = Console.ReadLine();

                Console.Write("Cena Produktu: ");
                decimal productPrice = decimal.Parse(Console.ReadLine());

                Console.WriteLine($"Produkt: {productName}, Kategoria: {productCategory}, Ilość: {amountOfProducts} szt, Cena za szt: {productPrice} zł" + "\n");
                Console.WriteLine("Produkt został dodany Pomyślnie !" + "\n");

                string productId = Guid.NewGuid().ToString();
                T warehouse = (T)Activator.CreateInstance(typeof(T), productName, productCategory, productPrice, amountOfProducts, productId);

                Create(warehouse);

                Console.WriteLine("\n" + "1. Dodaj kolejny produkt" + "\n" + "2. Lista produktów" + "0. Wyjście ");
                string operation = Console.ReadLine();
                if (operation == "1") { Console.Clear(); continue; }
                else if (operation == "2") { Console.Clear(); DisplayProducts(); }
                else if (operation == "0") { Console.Clear(); isRunning = false; }
            }
        }

        private void DeleteProduct()
        {
            bool isRunning = true;
            while (isRunning)
            {
                Console.Clear();
                Console.WriteLine("==== Usuń produkt z magazynu ====" + "\n");
                Console.WriteLine("Wybierz opcję wyszukania produktu:");
                Console.WriteLine("1. Wyszukaj  po nazwie produktu");
                Console.WriteLine("2. Wyszukaj po ID produktu ");
                Console.WriteLine("3. Lista produktów");
                Console.WriteLine("0. Exit");
                Console.WriteLine("Twój wybór:" + "\n");
                string searchProduct = Console.ReadLine();

                T productToDelete = null;

                if (searchProduct == "1")
                {
                    Console.Write("Podaj nazwę produktu: ");
                    string nameProductDel = Console.ReadLine();
                    productToDelete = ReadByName(nameProductDel);
                }
                else if (searchProduct == "2")
                {
                    Console.Write("Podaj ID produktu: ");
                    string idProductDel = Console.ReadLine();
                    productToDelete = Read(idProductDel);
                }
                else if (searchProduct == "3")
                {
                    DisplayProducts();
                }
                else if (searchProduct == "0")
                {
                    isRunning = false;
                }

                if (productToDelete != null)
                {
                    string maxIDproduct = GetMaxProductsID(productToDelete.Id, 8);
                    Console.WriteLine("Znaleziono Produkt!");
                    Console.WriteLine($"ID Produktu: {maxIDproduct}" + "\n" + $"Nazwa Produktu: {productToDelete.ProductName}" + "\n" + $"Kategoria Produktu: {productToDelete.ProductCategory}" + "\n" + $"Ilość Produktu: {productToDelete.AmountOfProducts}" + "\n");
                    Console.WriteLine("Czy na pewno chcesz usunąć ten produkt? (Tak/Nie)");
                    string confirmation = Console.ReadLine();
                    if (confirmation.ToLower() == "tak")
                    {
                        Delete(productToDelete.Id);
                        Console.WriteLine("\n" + "Produkt został wycofany!");
                    }
                    else
                    {
                        Console.WriteLine("\n" + "Anulowano usuwanie produktu!");
                    }
                }
            }
        }
        private void ModifyProduct()
        {
            Console.Clear();
            Console.WriteLine("==== Modyfikacja produktu ====" + "\n");
            Console.Write("Podaj ID produktu: ");
            string idProductToModify = Console.ReadLine();
            T productToModify = Read(idProductToModify);

            if (productToModify != null)
            {
                Console.WriteLine($"Aktualne dane produktu:");
                Console.WriteLine($"Nazwa: {productToModify.ProductName}");
                Console.WriteLine($"Kategoria: {productToModify.ProductCategory}");
                Console.WriteLine($"Ilość: {productToModify.AmountOfProducts}");
                Console.WriteLine($"Cena: {productToModify.ProductPrice}");

                Console.Write("Podaj nową ilość: ");
                int newAmount;
                if (int.TryParse(Console.ReadLine(), out newAmount))
                {
                    productToModify.AmountOfProducts = newAmount;
                }
                else
                {
                    Utils.DisplayError("Nieprawidłowa wartość ilości. Nie dokonano zmian.");
                }

                Console.Write("Podaj nową cenę: ");
                decimal newPrice;
                if (decimal.TryParse(Console.ReadLine(), out newPrice))
                {
                    productToModify.ProductPrice = newPrice;
                }
                else
                {
                    Utils.DisplayError("Nieprawidłowa wartość ceny. Nie dokonano zmian.");
                }

                Update(productToModify.Id, productToModify);
                Console.WriteLine("Produkt został zaktualizowany!" + "\n");
            }
            else
            {
                Utils.DisplayError("Produkt o podanym ID nie został znaleziony.");
            }

            Utils.PressEnterToContinue();
            Console.Clear();
        }

        private void DisplayProducts()
        {
            Console.Clear();
            Console.WriteLine("==== Lista wszystkich produktów ====" + "\n");

            List<T> products = GetAll();

            foreach (T product in products)
            {
                Console.WriteLine("======================" + "\n");
                string productMaxID = GetMaxProductsID(product.Id, 8);
                Console.WriteLine($"ID Produktu: {productMaxID}" + "\n" + $"Nazwa Produktu: {product.ProductName}" + "\n" + $"Kategoria Produktu: {product.ProductCategory}" + "\n" + $"Cena za szt: {product.ProductPrice} zł" + "\n" + $"Ilość sztuk w magazynie: {product.AmountOfProducts}");
                Console.WriteLine("======================" + "\n");
            }

            int totalProductCount = products.Count;
            Console.WriteLine($"Ilość produktów: {totalProductCount} szt");

            Console.WriteLine("1 - Dodaj produkt" + "\n" + "2 - Usuń produkt" + "\n" + "0 - Wyjście");
            string operation = Console.ReadLine();

            switch (operation)
            {
                case "1":
                    AddProduct();
                    break;
                case "2":
                    DeleteProduct();
                    break;
                case "0":
                    Console.Clear();
                    return;
                default:
                    Console.Clear();
                    Console.WriteLine("Nieprawidłowa operacja! Spróbuj ponownie!");
                    break;
            }
        }

        private T ReadByName(string productName)
        {
            return warehouselist.Find(p => p.ProductName == productName);
        }

        private string GetMaxProductsID(string id, int maxLength)
        {
            if (id.Length <= maxLength)
                return id;
            return id.Substring(0, maxLength);
        }

        private void LoadWarehouseFromFile()
        {
            if (File.Exists(dataFilePath))
            {
                string[] lines = File.ReadAllLines(dataFilePath);
                foreach (string line in lines)
                {
                    string[] parts = line.Split('|');
                    if (parts.Length == 6)
                    {
                        string id = parts[0];
                        string productName = parts[1];
                        string productCategory = parts[2];
                        decimal productPrice = decimal.Parse(parts[3]);
                        int amountOfProducts = int.Parse(parts[4]);
                        string productId = parts[5];

                        T warehouse = (T)Activator.CreateInstance(typeof(T), productName, productCategory, productPrice, amountOfProducts, productId);
                        Create(warehouse);
                    }
                }
            }
        }

        private void SaveWarehouseToFile()
        {
            List<string> lines = new List<string>();
            foreach (T product in GetAll())
            {
                string warehouseData = $"{product.Id}|{product.ProductName}|{product.ProductCategory}|{product.ProductPrice}|{product.AmountOfProducts}|{product.Id}";
                lines.Add(warehouseData);
            }
            File.WriteAllLines(dataFilePath, lines);
        }

        public class WarehouseProductComparer : IComparer<WarehouseProduct>
        {
            public int Compare(WarehouseProduct x, WarehouseProduct y)
            {
                return string.Compare(x.ProductName, y.ProductName, StringComparison.OrdinalIgnoreCase);
            }
        }
    }
}
