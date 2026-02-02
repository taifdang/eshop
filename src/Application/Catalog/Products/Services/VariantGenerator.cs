namespace Application.Catalog.Products.Services;

public class VariantGenerator : IVariantGenerator
{
    //ref: https://ericlippert.com/2010/06/28/computing-a-cartesian-product-with-linq/
    //ref: https://dotnettutorials.net/lesson/linq-cross-join/
    public IEnumerable<IEnumerable<T>> CartesianProduct<T>(IEnumerable<IEnumerable<T>> sequences)
    {
        IEnumerable<IEnumerable<T>> emptyProduct = new[] { Enumerable.Empty<T>() };
        return sequences.Aggregate(
          emptyProduct,
          (accumulator, sequence) =>
            from accseq in accumulator
            from item in sequence
            select accseq.Concat(new[] { item }));
    }

    public string GenerateSkuFromOptions(IEnumerable<string> optionValues)
    {
        return string.Join("-", optionValues).ToUpperInvariant();
    }
}
