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
using System.Linq;

namespace GraphQl.Schema.Query
{
    public class Query
    {
        static HttpClient client = new HttpClient();


        public async Task<IEnumerable<ProductType>> GetProducts()
        {
            List<ProductType> products = null;
            var path = "http://localhost:5080/api/Product";

            HttpResponseMessage response = await client.GetAsync(path);
            if (!response.IsSuccessStatusCode)
            {
                throw new GraphQLException(new Error("error", "error"));
            }
            products = await response.Content.ReadFromJsonAsync<List<ProductType>>();

            foreach(var product in products)
            {
                if(product.Name == null || product.Name == "")
                {
                    var pathOpenfoodfacts = "https://world.openfoodfacts.org/api/v2/search?code=" + product.code + "&fields=code,product_name";
                    var responseOpenfoodfact = await client.GetAsync(pathOpenfoodfacts);
                    if (responseOpenfoodfact.IsSuccessStatusCode)
                    {
                        var productOpenfoodfact = await responseOpenfoodfact.Content.ReadFromJsonAsync<openfoodfactsProduct>();
                        if (productOpenfoodfact.products.FirstOrDefault() != null)
                        {
                            product.Name = productOpenfoodfact.products.FirstOrDefault().product_name;
                        }
                    }
                }
            }
            return products;
        }

        public async Task<ProductType> GetProductByIdAsync(int id)
        {
            ProductType product = null;
            var path = "http://localhost:5080/api/Product/id?id=" + id;
            

            HttpResponseMessage response = await client.GetAsync(path);
            if (!response.IsSuccessStatusCode)
            {
                throw new GraphQLException(new Error("Product not found", "404"));
            }
            product = await response.Content.ReadFromJsonAsync<ProductType>();

            if(product.Name == null || product.Name == "")
            {
                var pathOpenfoodfacts = "https://world.openfoodfacts.org/api/v2/search?code="+ product.code + "&fields=code,product_name";
                var responseOpenfoodfact = await client.GetAsync(pathOpenfoodfacts);
                if (responseOpenfoodfact.IsSuccessStatusCode)
                {
                    var productOpenfoodfact = await responseOpenfoodfact.Content.ReadFromJsonAsync<openfoodfactsProduct>();
                    if(productOpenfoodfact.products.FirstOrDefault() != null)
                    {
                        product.Name = productOpenfoodfact.products.FirstOrDefault().product_name;
                    }
                }
            }
            return product;
        }

        public async Task<ProductType> GetProductByCodeAsync(int code)
        {
            //TODO API
            await Task.Delay(1000);
            Faker<ProductType> productFaker = new Faker<ProductType>();
            var product = productFaker.Generate();
            product.code = code;
            return product;
        }


    }
}
