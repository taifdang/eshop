
namespace Api.Models.Requests;

/* OLD v1 */
// public record CreateOptionValueRequestDto(Guid OptionId, string Value, IFormFile? MediaFile = null);
/* END OLD v1 */

/* NEW v2 */
public record CreateOptionValueRequestDto(Guid ProductId, Guid OptionId, string Value, IFormFile? MediaFile = null);
/* END NEW v2 */
