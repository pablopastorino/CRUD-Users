using Microsoft.AspNetCore.Mvc;

using System.Collections.Generic;

namespace UserManagement.Controllers
{
    public class Product
    {
        public required string Name { get; set; }
        public decimal Price { get; set; }
        public required string Description { get; set; }
    }

    [ApiController]

    [Route("api/[controller]")]

    public class ProductsController : ControllerBase

    {

        [HttpGet]

        public ActionResult<List<Product>> Get()

        {

            return new List<Product>

            {

                new() { Name = "Apple", Price = 0.5m, Description = "Fresh apple" },

                new() { Name = "Banana", Price = 0.3m, Description = "Ripe banana" },

                new() { Name = "Orange", Price = 0.4m, Description = "Juicy orange" }

            };

        }

        [HttpPost]

        public ActionResult<string> Post([FromBody] Product newProduct)

        {

            return $"Added: {newProduct.Name}";

        }

        [HttpPut("{id}")]

        public ActionResult<string> Put(int id, [FromBody] Product updatedProduct)

        {

            return $"Updated product {id} to: {updatedProduct.Name}";

        }

        [HttpDelete("{id}")]

        public ActionResult<string> Delete(int id)

        {

            return $"Deleted product with ID: {id}";

        }

    }

}