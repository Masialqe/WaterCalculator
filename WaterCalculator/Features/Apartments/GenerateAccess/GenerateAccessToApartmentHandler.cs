using Microsoft.EntityFrameworkCore;
using WaterCalculator.Common.Generators;
using WaterCalculator.Common.Infrastructure.Cache;
using WaterCalculator.Database;
using WaterCalculator.Domain;
using WaterCalculator.Domain.Abstractions;
using WaterCalculator.Features.InvoiceItems.Create;

namespace WaterCalculator.Features.Apartments.GenerateAccess
{
    public sealed record GenerateAccessTokenResponse(string Token, string Code);
    public sealed class GenerateAccessToApartmentHandler(IDbContextFactory<DatabaseContext> dbContextFactory,
        AccessCodeHasher accessCodeHasher,
        IAppCache cache,
        ILogger<CreateInvoiceItemHandler> logger)
    {
        public async Task<Result<GenerateAccessTokenResponse>> HandleAsync(Guid apartmentId, 
            CancellationToken cancellationToken = default)
        {
            try
            {
                await using var context = await dbContextFactory.CreateDbContextAsync(cancellationToken);

                var apartment = await context.Apartments
                    .FirstOrDefaultAsync(a => a.Id == apartmentId, cancellationToken);

                if (apartment is null)
                    return Errors.NotFoundError;

                if (apartment.HasAccessConfigured)
                    return Errors.InvalidOperationError("Dostęp został już skonfigurowany.");

                var publicToken = TokenGenerator.Generate();
                var rawAccessCode = AccessCodeProvider.Generate(10);
                var accessCodeHash = accessCodeHasher.Hash(apartment, rawAccessCode);

                var accessCode = ApartmentAccessCode.Create(accessCodeHash, apartmentId);
             
                apartment.PublicToken = publicToken;
                apartment.AccessCodeId = accessCode.Id;

                await context.ApartmentAccessCodes.AddAsync(accessCode, cancellationToken);
                await context.SaveChangesAsync(cancellationToken);

                await cache.RemoveByTagAsync(ApartmentCachePolicy.Tag);

                return new GenerateAccessTokenResponse(publicToken, rawAccessCode);

            }
            catch(Exception ex) 
            {
                logger.LogError("An error occured generating access for apartment {ApartmentId} - {Exception}",
                    apartmentId,
                    ex);
                return Errors.ApplicationError;
            }
        }
    }
}
