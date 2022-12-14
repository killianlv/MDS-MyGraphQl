using GraphQl.Schema.Query;
using HotChocolate;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GraphQl.Schema.Mutation
{
    public class Mutation
    {

        static HttpClient client = new HttpClient();



        public async Task<ProductResult> CreateProductAsync(ProductInputType productInput)
        {
            var path = "http://localhost:5080/api/Product";

            var requestProduct = new ProductResult { Name = productInput.Name, Code = productInput.Code, Stock = productInput.Stock };
            var payload = JsonSerializer.Serialize(requestProduct);

            HttpContent request = new StringContent(payload, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync(path, request);

            if (!response.IsSuccessStatusCode)
            {
                throw new GraphQLException(new Error("error", "error"));
            }
            var newProduct = await response.Content.ReadFromJsonAsync<ProductResult>();

            return newProduct;
        }

        public ProductResult UpdateProduct(int Id, ProductInputType productInput)
        {
            var product = new ProductResult { Id = Id, Name = productInput.Name, Code = productInput.Code, Stock = productInput.Stock };
            //TODO
            //throw new GraphQLException(new Error("not found", "404"));
            return product;
        }

        public bool DeleteProduct(int Id)
        {
            //TODO
            return true;
        }
    }
}
