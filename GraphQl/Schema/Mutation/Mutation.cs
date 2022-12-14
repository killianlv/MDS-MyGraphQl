using HotChocolate;

namespace GraphQl.Schema.Mutation
{
    public class Mutation
    {
        public ProductResult CreateProduct(ProductInputType productInput)
        {
            var product = new ProductResult { Id = 0, Name = productInput.Name, Code = productInput.Code, Stock = productInput.Stock };
            //TODO
            return product;
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
