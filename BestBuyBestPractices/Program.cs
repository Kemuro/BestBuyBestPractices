using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

// above are must haves using directives!!

namespace BestBuyBestPractices
{
    class Program
    {
        static void Main(string[] args)
        {
            #region
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            string connString = config.GetConnectionString("DefaultConnection");
            //Console.WriteLine(connString);
            #endregion

            IDbConnection conn = new MySqlConnection(connString);
            DapperDepartmentRepository departmentRepo = new DapperDepartmentRepository(conn);
            DapperProductRepository productRepo = new DapperProductRepository(conn);

            Console.WriteLine("Hello user, would you like to view the departments or products?");
            string userPick = Console.ReadLine();


            if (userPick.ToLower() == "department" || userPick.ToLower() == "departments")
            {
                Console.WriteLine("Here are the current departments:");
                //Console.WriteLine("Please press enter...");
                Console.WriteLine();
                var depos = departmentRepo.GetAllDepartments();

                PrintDepartment(depos);

                Console.WriteLine("Do you want to add a department?");
                string userResponse = Console.ReadLine();

                if (userResponse.ToLower() == "yes")
                {
                    Console.WriteLine("What is the name of your new Department??");
                    userResponse = Console.ReadLine();
                    departmentRepo.InsertDepartment(userResponse);
                    PrintDepartment(departmentRepo.GetAllDepartments());
                }
            }
            if(userPick.ToLower() == "product" || userPick.ToLower() == "products")
            {
                Console.WriteLine("Here are the current products:");
                Console.WriteLine();
                var depos = productRepo.GetAllProducts();

                PrintProducts(depos);

                Console.WriteLine("Do you want to add a product?");
                string userRepsonse = Console.ReadLine();

                if(userRepsonse.ToLower() == "yes")
                {
                    Console.WriteLine("What is the Name of your new Product?");
                    string newProductName = Console.ReadLine();
                    Console.WriteLine("What is the Price of your new Product?");
                    var newProductPrice = double.Parse(Console.ReadLine());
                    Console.WriteLine("What is the CategoryID of your new Product?");
                    int newProductCategory = int.Parse(Console.ReadLine());

                    productRepo.CreateProduct(newProductName, newProductPrice, newProductCategory);
                    PrintProducts(productRepo.GetAllProducts());
                }
            }

            Console.WriteLine("Have a great day.");
        }

        private static void PrintDepartment(IEnumerable<Department> depos)
        {
            foreach (var depo in depos)
            {
                Console.WriteLine($"ID: {depo.DepartmentID} Name: {depo.Name}");
            }
            Console.WriteLine();
        }
        private static void PrintProducts(IEnumerable<Product> depos)
        {
            foreach(var depo in depos)
            {
                Console.WriteLine($"Name: {depo.Name} \n Price: {depo.Price} \n CategoryID: {depo.CategoryID}");
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }
}