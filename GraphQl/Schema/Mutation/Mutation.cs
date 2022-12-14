using GraphQl.Schema.Query;
using HotChocolate;
using System.Collections.Generic;
using System.Linq;
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

            if (newProduct.Name == null || newProduct.Name == "")
            {
                var pathOpenfoodfacts = "https://world.openfoodfacts.org/api/v2/search?code=" + newProduct.Code + "&fields=code,product_name";
                var responseOpenfoodfact = await client.GetAsync(pathOpenfoodfacts);
                if (responseOpenfoodfact.IsSuccessStatusCode)
                {
                    var productOpenfoodfact = await responseOpenfoodfact.Content.ReadFromJsonAsync<openfoodfactsProduct>();
                    if (productOpenfoodfact.products.FirstOrDefault() != null)
                    {
                        newProduct.Name = productOpenfoodfact.products.FirstOrDefault().product_name;
                    }
                }
            }

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

            if (updateProduct.Name == null || updateProduct.Name == "")
            {
                var pathOpenfoodfacts = "https://world.openfoodfacts.org/api/v2/search?code=" + updateProduct.Code + "&fields=code,product_name";
                var responseOpenfoodfact = await client.GetAsync(pathOpenfoodfacts);
                if (responseOpenfoodfact.IsSuccessStatusCode)
                {
                    var productOpenfoodfact = await responseOpenfoodfact.Content.ReadFromJsonAsync<openfoodfactsProduct>();
                    if (productOpenfoodfact.products.FirstOrDefault() != null)
                    {
                        updateProduct.Name = productOpenfoodfact.products.FirstOrDefault().product_name;
                    }
                }
            }

            return updateProduct;
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            var path = "http://localhost:5080/api/Product/id?id=" + id;


            HttpResponseMessage response = await client.DeleteAsync(path);
            if (!response.IsSuccessStatusCode)
            {
                throw new GraphQLException(new Error("Product not found", "404"));
            }
            return true;
        }
    }
}
