namespace PIMS.Model
{
    public class ProductModel
    {
        public string SKU { get; set; }
        public string Name { get; set; }
        public string Desciption { get; set; }
        public decimal Price { get; set; }

        //For key
        public int CatId { get; set; }
    }
}
