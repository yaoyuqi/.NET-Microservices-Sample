using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ProductService.Infrastructure.Contexts;
using ProductService.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductService.Data
{
    public static class DataSeeder
    {
        public async static Task Seed(IServiceProvider sp)
        {
            using var scope = sp.CreateScope();

            var logger = scope.ServiceProvider.GetRequiredService<ILoggerFactory>()
                .CreateLogger(nameof(DataSeeder));
            using var context = scope.ServiceProvider.GetRequiredService<ProductDatabaseContext>();
            if (!context.Products.Any())
            {
                logger.LogInformation("start seeding product data");
                List<Category> categories = new string[] { "Book", "Food", "Game" }
                .Select(str => CreateCategory(str))
                .ToList();

                context.Categories.AddRange(categories);

                var products = new[]
                {
                    new {Name= "Ring of King", Price= 13, cat = 0},
                    new {Name="Use you habit", Price = 12, cat = 0},
                     new {Name="Sanwitch", Price = 8, cat = 1},
                      new {Name="Milk", Price = 4, cat = 1},
                       new {Name="Mario", Price = 50, cat = 2},

                };

                foreach (var p in products)
                {
                    var product = CreateProduct(p.Name, p.Price);
                    product.Category = categories[p.cat];

                    context.Products.Add(product);
                }


                await context.SaveChangesAsync();
            }
            else
            {
                logger.LogInformation("No product need to seed");
            }


        }

        private static Category CreateCategory(string name)
        {
            var category1 = new Category();
            category1.Id = Guid.NewGuid();
            category1.Name = name;
            category1.Description = name;
            return category1;
        }

        private static Product CreateProduct(string name, int price)
        {
            var product = new Product
            {
                Name = name,
                Description = name,
                Image = "https://placehold.co/600x400",
                Price = price
            };

            return product;
        }

    }
}
