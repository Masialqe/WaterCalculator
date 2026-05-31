using Microsoft.EntityFrameworkCore;
using WaterCalculator.Common.Abstractions;
using WaterCalculator.Database;
using WaterCalculator.Domain;

namespace WaterCalculator.Features.Apartments.Create
{
    public record CreateApartmentCommand(string Name, string Details, Guid? GroupId);

    public class CreateApartmentHandler(
        IDbContextFactory<DatabaseContext> dbContextFactory,
        ILogger<CreateApartmentHandler> logger)
    {
        public async Task<Result> HandleAsync(CreateApartmentCommand command,
            CancellationToken cancellationToken = default)
        {
            try
            {
                await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);

                if (await IsAlreadyExists(dbContext, command, cancellationToken)) return Errors.AlreadyExistsError;

                var newApartment = Apartment.Create(command.Name, command.Details, command.GroupId);
                await dbContext.Apartments.AddAsync(newApartment, cancellationToken);
                await dbContext.SaveChangesAsync(cancellationToken);
                
                return Result.Success();
            }
            catch (Exception ex)
            {
                logger.LogError("Error creating apartment: {Error} - {ErrorDetails}", ex.Message, ex);
                return Errors.ApplicationError;
            }
        }

        private async Task<bool> IsAlreadyExists(DatabaseContext dbContext,
            CreateApartmentCommand command, CancellationToken cancellationToken = default)
            => await dbContext.Apartments
                .AnyAsync(x => x.Name == command.Name
                               && x.GroupId == command.GroupId, cancellationToken);
    }
}
