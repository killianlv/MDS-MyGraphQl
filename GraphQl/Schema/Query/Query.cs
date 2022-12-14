using Bogus;
using HotChocolate;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace GraphQl.Schema.Query
{
    public class Query
    {
        static HttpClient client = new HttpClient();
        public IEnumerable<ProductType> GetProducts()
        {
            Faker<ProductType> productsFaker = new Faker<ProductType>();
            var products = productsFaker.Generate(5);
            return products;

        }

        public async Task<ProductType> GetProductByIdAsync(int id)
        {
            var path = "http://localhost:5080/api/Product/id?id=" + id;
            ProductType product = null;
            HttpResponseMessage response = await client.GetAsync(path);
            if (!response.IsSuccessStatusCode)
            {
                throw new GraphQLException(new Error("Product not found", "404"));
            }
            product = await response.Content.ReadFromJsonAsync<ProductType>();
            return product;
        }

        public async Task<ProductType> GetProductByCodeAsync(int code)
        {
            await Task.Delay(1000);
            Faker<ProductType> productFaker = new Faker<ProductType>();
            var product = productFaker.Generate();
            product.code = code;
            return product;
        }

        [GraphQLDeprecated("This query is deprecated.")]
        public string Instruction => "test";
    }
}
