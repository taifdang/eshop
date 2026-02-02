namespace Api.Models.Requests;

/* OLD v1 */
// public record UpdateVariantRequestDto(Guid Id, decimal RegularPrice, int Quantity);
/* END OLD v1 */

/* NEW v2 */
public record UpdateVariantRequestDto(Guid ProductId, Guid Id, decimal RegularPrice, int Quantity);
/* END NEW v2 */
