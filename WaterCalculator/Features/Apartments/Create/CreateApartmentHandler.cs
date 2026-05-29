using Microsoft.EntityFrameworkCore;
using WaterCalculator.Database;

namespace WaterCalculator.Features.Apartments.Create
{
    public record CreateApartmentCommand(string Name, string Details, Guid? GroupId);
    public class CreateApartmentHandler(IDbContextFactory<DatabaseContext> dbContextFactory, 
        ILogger<CreateApartmentHandler> logger)
    {
        public async Task HandleAsync(CreateApartmentCommand command, CancellationToken cancellationToken = default)
        {

        }
    }
}
