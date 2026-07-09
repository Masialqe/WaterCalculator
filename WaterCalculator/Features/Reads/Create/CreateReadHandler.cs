using Microsoft.EntityFrameworkCore;
using WaterCalculator.Common.Infrastructure.Cache;
using WaterCalculator.Database;
using WaterCalculator.Domain;
using WaterCalculator.Domain.Abstractions;
using WaterCalculator.Domain.Reads;
using WaterCalculator.Features.Apartments;

namespace WaterCalculator.Features.Reads.Create;

public sealed record CreateReadCommand(
    decimal Value, DateTime ReadDate, Guid ApartmentId, Guid? PayoffId);
public sealed class CreateReadHandler(IDbContextFactory<DatabaseContext> dbContextFactory, 
    IAppCache cache,
    ILogger<CreateReadHandler> logger)
{
    public async Task<Result> HandleAsync(CreateReadCommand command, 
        CancellationToken cancellationToken = default)
    {
        try
        { 
            await using var context = await dbContextFactory.CreateDbContextAsync(cancellationToken);
            var isFirstRead = !await context.Reads.AnyAsync(r => r.ApartmentId == command.ApartmentId, cancellationToken);

            var read = Read.Create(
                command.Value, 
                command.ReadDate, 
                command.ApartmentId, 
                command.PayoffId);

            var readValidationResult = read.Validate(isFirstRead);
            if (readValidationResult.IsFailure) return readValidationResult.Error;


           if(!isFirstRead)
            {
                var lastReadForApartment = await context.Reads
               .Where(r => r.ApartmentId == command.ApartmentId)
               .OrderByDescending(x => x.ReadDate)
               .Select(r => r.Value)
               .FirstOrDefaultAsync();

                if (command.Value < lastReadForApartment)
                    return Errors.InvalidOperationError("Wartość odczytu nie może być mniejsza od poprzedniego.");

                read.ConsumptionFromLastRead = command.Value - lastReadForApartment;
            }

            await context.Reads.AddAsync(read, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);

            //TODO: Move from here - it doesnt fit architecture at all - palced here temporary for dev
            //Will be replaced by events
            await cache.RemoveByTagAsync(ApartmentCachePolicy.Tag);

            return Result.Success();

        }
        catch (Exception ex)
        {
            logger.LogError("An error occured saving read - {ErrorMessage} - {Error}", ex.Message, ex);
            return Errors.ApplicationError;
        }
    }
}