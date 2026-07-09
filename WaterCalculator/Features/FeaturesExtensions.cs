using WaterCalculator.Features.Apartments.Create;
using WaterCalculator.Features.Apartments.Delete;
using WaterCalculator.Features.Apartments.GenerateAccess;
using WaterCalculator.Features.Apartments.GetAsList;
using WaterCalculator.Features.Groups.Create;
using WaterCalculator.Features.Groups.Delete;
using WaterCalculator.Features.Groups.Get;
using WaterCalculator.Features.Groups.GetDictionary;
using WaterCalculator.Features.Groups.GetList;
using WaterCalculator.Features.Invoices.Create;
using WaterCalculator.Features.Payoffs.Create;
using WaterCalculator.Features.Payoffs.Get;
using WaterCalculator.Features.Payoffs.Settle;
using WaterCalculator.Features.Payoffs.Summarize;
using WaterCalculator.Features.Reads.Create;
using WaterCalculator.Features.Settlements;

namespace WaterCalculator.Features
{
    public static class FeaturesExtensions
    {
        extension(IServiceCollection services)
        {
            public void AddApplicationFeatures()
            {
                //Apartments
                services.AddScoped<UpsertApartmentHandler>();
                services.AddScoped<GetApartmentsHandler>();
                services.AddScoped<GetApartmentByIdHandler>();
                services.AddScoped<GenerateAccessToApartmentHandler>();
                services.AddSingleton<AccessCodeHasher>();
                services.AddScoped<DeleteApartmentHandler>();
                services.AddScoped<RegenerateAccessCodeHandler>();

                //Reads
                services.AddScoped<CreateReadHandler>();

                //Invoices
                services.AddScoped<CreateInvoiceHandler>();

                //Groups
                services.AddScoped<UpsertGroupHandler>();
                services.AddScoped<GetGroupByIdHandler>();
                services.AddScoped<GetGroupsHandler>();
                services.AddScoped<GetGroupDictionaryHandler>();
                services.AddScoped<DeleteGroupHandler>();

                //Settlements
                services.AddScoped<GenerateSettlementForGroupHandler>();

                //Payoffs
                services.AddScoped<GetPayoffDetailsHandler>();
                services.AddScoped<CreatePayoffHandler>();
                services.AddScoped<SummarizePayoffHandler>();
                services.AddSingleton<InvoiceStateValidator>();
                services.AddSingleton<PayoffSettlementCalculator>();
                services.AddScoped<SettlePayoffHandler>();
                services.AddSingleton<PayoffSettlementCalculator>();
                services.AddSingleton<InvoiceStateValidator>();
            }
        }
    }
}
