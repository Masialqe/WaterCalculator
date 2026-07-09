using Microsoft.EntityFrameworkCore;
using WaterCalculator.Database;
using WaterCalculator.Domain;
using WaterCalculator.Domain.Abstractions;
using WaterCalculator.Features.Groups.GetList;

namespace WaterCalculator.Features.Apartments.GenerateAccess
{
    public sealed class RegenerateAccessCodeHandler(IDbContextFactory<DatabaseContext> dbContextFactory,
        AccessCodeHasher accessCodeHasher,
        ILogger<GetGroupsHandler> logger)
    {
        public async Task<Result<string>> HandleAsync(RegenerateAccessCodeCommand command,
            CancellationToken cancellationToken = default)
        {
            try
            {
                await using var context = await dbContextFactory.CreateDbContextAsync(cancellationToken);
                var apartment = await context.Apartments
                    .AsNoTracking()
                    .FirstOrDefaultAsync(a => a.Id == command.ApartmentId &&
                        a.PublicToken == command.PublicToken, cancellationToken);

                if (apartment is null) return Errors.NotFoundError;

                var rawAccessCode = AccessCodeProvider.Generate(10);
                var accessCodeHash = accessCodeHasher.Hash(apartment, rawAccessCode);

                var result = await context.ApartmentAccessCodes
                    .Where(ac => ac.Id == apartment.AccessCodeId)
                    .ExecuteUpdateAsync(s => s.SetProperty(a => a.Code, accessCodeHash));

                if (result == 0) return Errors.InvalidOperationError("Nie udało się zapisać kodu.");

                return rawAccessCode;

            }
            catch(Exception ex)
            {
                logger.LogError("An error occured regenerating access code for - {ApartmentId} - {Exception}",
                    command.ApartmentId,
                    ex);
                return Errors.ApplicationError;
            }
        }
    }
}
