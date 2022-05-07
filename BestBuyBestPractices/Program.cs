using System;
using System.Data;
using System.IO;
using MySql.Data.MySqlClient;
using Microsoft.Extensions.Configuration;


namespace BestBuyBestPractices
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            string connString = config.GetConnectionString("DefaultConnection");
            IDbConnection conn = new MySqlConnection(connString);

            //Department implementation:
            var deptRepo = new DapperDepartmentRepository(conn);
            var departments = deptRepo.GetAllDepartments();

            Console.WriteLine($"Printing all departments...");
            foreach(var dept in departments)
            {
                Console.WriteLine($"ID:{dept.DepartmentID} Name:{dept.Name}");
            }

            Console.WriteLine($"Type new department name:");
            var newDepartment = Console.ReadLine();
            deptRepo.InsertDepartment(newDepartment);

            Console.WriteLine($"Printing new department list...");
            departments = deptRepo.GetAllDepartments();
            foreach (var dept in departments)
            {
                Console.WriteLine($"ID:{dept.DepartmentID} Name:{dept.Name}");
            }

            //Product implementation:
            var prodRepo = new DapperProductRepository(conn);
            var products = prodRepo.GetAllProducts();

            Console.WriteLine($"Printing all products...");
            foreach(var prod in products)
            {
                Console.WriteLine($"ID:{prod.ProductID} Name:{prod.Name} Price:{prod.Price} CategoryID:{prod.CategoryID}");
            }

            var newProduct = new Product();

            Console.WriteLine($"Type new product name:");
            newProduct.Name = Console.ReadLine();

            Console.WriteLine($"Type price(must be decimal) of {newProduct.Name}:");
            var newProductPrice = Console.ReadLine();
            while(!double.TryParse(newProductPrice, out double result))
            {
                Console.WriteLine($"Failed. Try again");
                newProductPrice = Console.ReadLine();
            }
            newProduct.Price = double.Parse(newProductPrice);

            Console.WriteLine($"Type CategoryID(must be integer) of {newProduct.Name}");
            var newProductCategoryID = Console.ReadLine();
            while(!int.TryParse(newProductCategoryID, out int result))
            {
                Console.WriteLine($"Failed. Try again");
                newProductCategoryID = Console.ReadLine();
            }
            newProduct.CategoryID = int.Parse(newProductCategoryID);

            prodRepo.CreateProduct(newProduct.Name, newProduct.Price, newProduct.CategoryID);

            Console.WriteLine($"Printing new product list...");
            products = prodRepo.GetAllProducts();
            foreach(var prod in products)
            {
                Console.WriteLine($"ID:{prod.ProductID} Name:{prod.Name} Price:{prod.Price} CategoryID:{prod.CategoryID}");
            }
        }
    }
}
