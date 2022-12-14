using System.Collections.Generic;

namespace GraphQl.Schema.Query
{
    public class openfoodfactsProduct
    {
        public int count { get; set; }
        public int page { get; set; }
        public int page_count { get; set; }
        public int page_size { get; set; }
        public List<Product> products { get; set; }
        public int skip { get; set; }
    }

    public class Product
    {
        public long code { get; set; }
        public string product_name { get; set; }
        
    }
}
