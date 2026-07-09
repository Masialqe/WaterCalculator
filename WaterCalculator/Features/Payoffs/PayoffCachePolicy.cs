namespace WaterCalculator.Features.Payoffs
{
    public static class PayoffCachePolicy
    {
        public const string Tag = "payoffs";

        public static string UniqueKey(Guid id)
            => $"payoff:{id}";

        public static string CollectionKey = "payoffs:all";
        public static string PagedKey(int page, int pageSize)
        => $"payoff:paged:page:{page}:size:{pageSize}";

        public static string SummaryUniqueKey(Guid id) 
            => $"payoffsummary:{id}";
    }
}
