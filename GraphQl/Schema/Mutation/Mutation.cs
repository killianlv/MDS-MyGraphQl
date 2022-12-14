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

        public async Task<ProductResult> UpdateProductAsync(int id, ProductInputType productInput)
        {
            var path = "http://localhost:5080/api/Product/id?id="+id;

            var requestProduct = new ProductResult { Name = productInput.Name, Code = productInput.Code, Stock = productInput.Stock };
            var payload = JsonSerializer.Serialize(requestProduct);

            HttpContent request = new StringContent(payload, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PutAsync(path, request);

            if (!response.IsSuccessStatusCode)
            {
                throw new GraphQLException(new Error("error", "error"));
            }
            var updateProduct = await response.Content.ReadFromJsonAsync<ProductResult>();

            return updateProduct;
        }

        public bool DeleteProduct(int Id)
        {
            //TODO
            return true;
        }
    }
}
