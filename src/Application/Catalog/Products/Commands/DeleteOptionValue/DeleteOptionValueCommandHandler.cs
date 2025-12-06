using Application.Common.Interfaces;
using Application.Common.Models;
using Ardalis.GuardClauses;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Catalog.Products.Commands.DeleteOptionValue
{
    public class DeleteOptionValueCommandHandler : IRequestHandler<DeleteOptionValueCommand, Unit>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IFileService _storageService;

        public DeleteOptionValueCommandHandler(
            IApplicationDbContext dbContext, 
            IFileService storageService)
        {
            _dbContext = dbContext;
            _storageService = storageService;
        }

        public async Task<Unit> Handle(DeleteOptionValueCommand request, CancellationToken cancellationToken)
        {
            var optionValue = await _dbContext.OptionValues
                .FirstOrDefaultAsync(x => x.Id == request.OptionValueId && x.OptionId == request.OptionId);
            Guard.Against.NotFound(request.OptionValueId, optionValue);

            // remove image
            if (optionValue.Image != null)
            {
                await _storageService.DeleteFileAsync(new DeleteFileRequest { FileName = optionValue.Image.FileName });
            }

            _dbContext.OptionValues.Remove(optionValue);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
