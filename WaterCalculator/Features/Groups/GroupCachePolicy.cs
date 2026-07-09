namespace WaterCalculator.Features.Groups
{
    public static class GroupCachePolicy
    {
        public const string Tag = "groups";
    
        public static string UniqueKey(Guid id)
            => $"group:{id}";

        public static string CollectionKey = "groups:all";
        public static string PagedKey(int page, int pageSize)
        => $"groups:paged:page:{page}:size:{pageSize}";
    }
}
