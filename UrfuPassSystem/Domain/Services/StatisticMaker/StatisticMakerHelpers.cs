using UrfuPassSystem.Domain.Entities;

internal static class StatisticMakerHelpers
{
    public static IQueryable<Image> AddCheckTimeFilters(this IQueryable<Image> images, DateTime? from, DateTime? to)
    {
        images = images.Where(i => i.Checks!.Any(c => !c.IsDeleted));
        if (from is not null)
            images = images.Where(i => i.Checks!.Last(c => !c.IsDeleted).CheckTime >= from);
        if (to is not null)
            images = images.Where(i => i.Checks!.Last(c => !c.IsDeleted).CheckTime <= to);
        return images;
    }
}
