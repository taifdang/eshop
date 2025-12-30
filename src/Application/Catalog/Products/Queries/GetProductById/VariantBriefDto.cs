namespace Application.Catalog.Products.Queries.GetProductById
{
    // case: product no option, must have variant bref to return
    public class VariantBriefDto
    {
        public Guid? Id { get; set; }
        public int Quantity { get; set; }
        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }
    }
}
