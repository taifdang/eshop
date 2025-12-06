namespace Application.Common.Utilities;
//ref: https://ericlippert.com/2010/06/28/computing-a-cartesian-product-with-linq/
//ref: https://dotnettutorials.net/lesson/linq-cross-join/
public static class CombinationHelper
{
    public static IEnumerable<IEnumerable<T>> CartesianProduct<T>(IEnumerable<IEnumerable<T>> sequences)
    {
#region C1
        //if (!sequences.Any())
        //    yield break;

        //IEnumerable<IEnumerable<T>> result = new[] { Enumerable.Empty<T>() };

        //foreach (var sequence in sequences)
        //{
        //    result = result.SelectMany(
        //        prefix => sequence,
        //        (prefix, item) => prefix.Append(item)
        //    );
        //}

        //foreach (var combination in result)
        //    yield return combination;
#endregion
#region C3
        IEnumerable<IEnumerable<T>> emptyProduct = new[] { Enumerable.Empty<T>() };
        return sequences.Aggregate(
          emptyProduct,
          (accumulator, sequence) =>
            from accseq in accumulator
            from item in sequence
            select accseq.Concat(new[] { item }));
#endregion
    }
}
