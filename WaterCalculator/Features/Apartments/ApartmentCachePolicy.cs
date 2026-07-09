namespace WaterCalculator.Features.Apartments
{
    public static class ApartmentCachePolicy
    {
        public const string Tag = "apartments";

        public static string UniqueKey(Guid id)
            => $"apartments:{id}";

        public static string CollectionKey = "apartments:all";

        public static string PagedKey(int page, int pageSize)
        => $"apartments:paged:page:{page}:size:{pageSize}";
    }
}
