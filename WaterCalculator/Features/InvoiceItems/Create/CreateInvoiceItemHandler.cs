using Microsoft.EntityFrameworkCore;
using WaterCalculator.Common.Abstractions;
using WaterCalculator.Database;
using WaterCalculator.Domain;

namespace WaterCalculator.Features.InvoiceItems.Create
{
    public sealed class CreateInvoiceItemHandler(IDbContextFactory<DatabaseContext> dbContextFactory, 
        ILogger<CreateInvoiceItemHandler> logger)
    {
        //public async Task<Result> HandleAsync(CreateInvoiceItemCommand command, 
        //    CancellationToken cancellationToken = default)
        //{
        //    try
        //    {

        //    }
        //    catch (Exception ex) 
        //    {
        //        logger.LogError(ex, "Error occurred while creating invoice item");
        //        return Errors.ApplicationError;
        //    }
        //}
    }
}