namespace WaterCalculator.Common.Abstractions
{
    
    public sealed record PageResult<T>(
        IReadOnlyList<T> Items,
        int CurrentPage,
        int PageSize,
        int TotalCount,
        int TotalPages);
}
