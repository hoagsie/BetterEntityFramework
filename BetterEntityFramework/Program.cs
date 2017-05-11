using System;
using System.Data;
using System.Linq;
using BetterEntityFramework.Extensions;
using BetterEntityFramework.StoreData;
using Microsoft.EntityFrameworkCore;
using Data = BetterEntityFramework.DataService;

namespace BetterEntityFramework
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            DisplayMenu();

            string command;

            while ((command = Console.ReadLine()) != "exit")
            {
                HandleCommand(command);
            }
        }

        private static void HandleCommand(string command)
        {
            var data = new Data();
            
            data.Service.Category.UpdateWhere(category => category.Publish == false, category => new Category {Publish = true}).Wait();
            data.Service.Product.DeleteWhere(product => product.Publish == false).Wait();
            data.Service.BulkInsert(data.Service.Category.Where(category => category.Publish)).Wait();
            data.Service.Clear<Product>();
            data.Service.ClearAll();

            Console.WriteLine(command);
        }

        private static void DisplayMenu()
        {
            var menu = new[]
            {
                "[0] - Products",
                "[1] - Bundles",
                "[2] - Categories",
                "[3] - Users",
                "[4] - Baskets"
            };

            Console.Write(string.Join(Environment.NewLine, menu) + Environment.NewLine);
            Console.WriteLine("Enter Selection:");
        }
    }
}
