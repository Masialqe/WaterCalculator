using Microsoft.EntityFrameworkCore;
using WaterCalculator.Common.Abstractions;
using WaterCalculator.Database;
using WaterCalculator.Domain;

namespace WaterCalculator.Features.Reads.Create;

public sealed record CreateReadCommand(decimal Value, Guid ApartmentId);
public sealed class CreateReadHandler(IDbContextFactory<DatabaseContext> dbContextFactory, 
    ILogger<CreateReadHandler> logger)
{
    public async Task<Result> HandleAsync(CreateReadCommand command, 
        CancellationToken cancellationToken = default)
    {
        try
        {
            await using var context = await dbContextFactory.CreateDbContextAsync(cancellationToken);
            var newRead = Read.Create(command.Value, command.ApartmentId);
            await context.Reads.AddAsync(newRead, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
            return Result.Success();

        }
        catch (Exception ex)
        {
            logger.LogError("An error occured saving read - {ErrorMessage} - {Error}", ex.Message, ex);
            return Errors.ApplicationError;
        }
    }
}