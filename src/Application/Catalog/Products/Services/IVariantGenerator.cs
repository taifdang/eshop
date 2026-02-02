namespace Application.Catalog.Products.Services;

public interface IVariantGenerator
{
    IEnumerable<IEnumerable<T>> CartesianProduct<T>(IEnumerable<IEnumerable<T>> sequences);
    string GenerateSkuFromOptions(IEnumerable<string> optionValues);
}
