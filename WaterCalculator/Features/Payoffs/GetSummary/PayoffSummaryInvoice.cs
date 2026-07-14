namespace WaterCalculator.Features.Payoffs.GetSummary;

public sealed record PayoffSummaryInvoice(
    string InvoiceName,
    string InvoiceNumber,
    decimal TotalMeterValue,
    decimal InvoiceValue
);