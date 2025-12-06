using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace Application.Common.Validation;
//ref: https://stackoverflow.com/questions/59358252/how-to-validate-uploaded-files-by-fluentvalidation
public class FileValidator : AbstractValidator<IFormFile?>
{
    public FileValidator(long maxFileSizeInMb, string[] permittedExtensions)
    {
        long maxFileSizeInBytes = maxFileSizeInMb * 1024 * 1024;

        When(file => file != null, () =>
        {
            RuleFor(file => file!.Length)
               .LessThanOrEqualTo(maxFileSizeInBytes)
               .WithMessage($"File size must not exceed {maxFileSizeInMb} MB.");


            RuleFor(file => System.IO.Path.GetExtension(file!.FileName).ToLowerInvariant())
                .Must(ext => permittedExtensions.Contains(ext))
                .WithMessage($"File type is not permitted. Allowed: {string.Join(", ", permittedExtensions)}");

        });
    }
}
