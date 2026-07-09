using Microsoft.EntityFrameworkCore;
using WaterCalculator.Common.Infrastructure.Cache;
using WaterCalculator.Database;
using WaterCalculator.Domain;
using WaterCalculator.Domain.Abstractions;

namespace WaterCalculator.Features.Apartments.Create
{
    public record UpsertApartmentCommand(string Name,string Details,
        Guid GroupId, Guid? ApartmentId);

    public class UpsertApartmentHandler(
        IDbContextFactory<DatabaseContext> dbContextFactory,
        IAppCache cache,
        ILogger<UpsertApartmentHandler> logger)
    {
        public async Task<Result<Guid>> HandleAsync(UpsertApartmentCommand command,
                CancellationToken cancellationToken = default)
        {
            try
            {
                await using var context =
                    await dbContextFactory.CreateDbContextAsync(cancellationToken);

                var result = command.ApartmentId == null || command.ApartmentId == Guid.Empty
                    ? await CreateApartmentAsync(context, command, cancellationToken)
                    : await UpdateApartmentAsync(context, command, cancellationToken);

                if (result.IsFailure)
                    return result.Error;

                await context.SaveChangesAsync(cancellationToken);
                await cache.RemoveByTagAsync(ApartmentCachePolicy.Tag);

                return result.Value;
            }
            catch (Exception ex)
            {
                logger.LogError(
                    "An error occurred while upserting apartment - {ApartmentName} - {ErrorMessage} - {Error}",
                    command.Name,
                    ex.Message,
                    ex);

                return Errors.ApplicationError;
            }
        }

        private async Task<Result<Guid>> CreateApartmentAsync(
            DatabaseContext context,
            UpsertApartmentCommand command,
            CancellationToken cancellationToken = default)
        {
            if (command.GroupId == Guid.Empty)
                return Errors.InvalidOperationError("Grupa musi zostać przypisana.");

            if (await IsApartmentExists(context, command.Name,
                    command.GroupId, null, cancellationToken)) return Errors.AlreadyExistsError;

            var apartment = Apartment.Create(
                command.Name,
                command.Details,
                command.GroupId);

            await context.AddAsync(apartment, cancellationToken);

            return apartment.Id;
        }

        private async Task<Result<Guid>> UpdateApartmentAsync(
            DatabaseContext context,
            UpsertApartmentCommand command,
            CancellationToken cancellationToken = default)
        {
            var existingState = await context.Apartments
                .FirstOrDefaultAsync(
                    x => x.Id == command.ApartmentId,
                    cancellationToken);

            if (existingState == null)
                return Errors.NotFoundError;

            if (command.GroupId == Guid.Empty)
                return Errors.InvalidOperationError("Grupa musi zostać przypisana.");

            if (await IsApartmentExists(
                    context, command.Name, command.GroupId,
                    existingState.Id, cancellationToken)) return Errors.AlreadyExistsError;


            existingState.Name = command.Name;
            existingState.Details = command.Details;
            existingState.GroupId = command.GroupId;

            return existingState.Id;
        }

        private async Task<bool> IsApartmentExists(
            DatabaseContext context,
            string apartmentName,
            Guid groupId,
            Guid? excludedApartmentId = null,
            CancellationToken cancellationToken = default)
            => await context.Apartments.AnyAsync(
                x =>
                    x.Name == apartmentName &&
                    x.GroupId == groupId &&
                    (excludedApartmentId == null || x.Id != excludedApartmentId),
                cancellationToken);
    }
}
