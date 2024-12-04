using Microsoft.EntityFrameworkCore;
using UrfuPassSystem.Domain.Models.Statistic;

namespace UrfuPassSystem.Domain.Services.StatisticMaker;

public class StatisticMaker : IStatisticMaker
{
    public async Task<TotalImageStatistic> MakeImageStatistic(ApplicationDbContext dbContext, DateTime? from, DateTime? to)
    {
        var total = await dbContext.Images
            .Where(i => !i.IsDeleted)
            .Include(i => i.Checks!.OrderBy(c => c.CheckTime))
            .AddCheckTimeFilters(from, to)
            .CountAsync();
        var success = await dbContext.Images
            .Where(i => !i.IsDeleted)
            .Include(i => i.Checks!.OrderBy(c => c.CheckTime))
            .AddCheckTimeFilters(from, to)
            .AsAsyncEnumerable()
            .Where(i => !i.Checks!.Any(c => !c.IsDeleted && !c.IsSuccess))
            .CountAsync();
        return new TotalImageStatistic(from, to, success, total);
    }

    public async Task<TotalBadImageStatistic> MakeBadImageStatistic(ApplicationDbContext dbContext, DateTime? from, DateTime? to)
    {
        var groups = dbContext.Images
            .Where(i => !i.IsDeleted)
            .Include(i => i.Checks!.OrderBy(c => c.CheckTime))
            .AddCheckTimeFilters(from, to)
            .AsEnumerable()
            .GroupBy(i => i.Checks!.Where(c => !c.IsDeleted).MaxBy(c => c.CheckTime)!.ResultCode)
            .Where(i => i.Key != Enums.ImageCheckResultCode.Success)
            .Select(g => new BadImageStatistic(g.Key, g.Count()))
            .ToArray();
        return new TotalBadImageStatistic(from, to, groups);
    }

    //public async Task<BadImagesList> MakeBadImagesList(ApplicationDbContext dbContext, DateTime? From, DateTime? To)
    //{
    //    var badImages = await dbContext.Images
    //        .Where(i => !i.IsDeleted)
    //        .Include(i => i.Checks!.OrderByDescending(c => c.CheckTime))
    //        .Where(i => i.Checks!.Any(c => !c.IsDeleted && !c.IsSuccess))
    //        .Select(i => new { image = i, check = i.Checks!.First(c => !c.IsDeleted && !c.IsSuccess) })
    //        .Select(i => new BadImageEntry(i.image.FilePath, i.check.ResultCode));
    //    return new BadImagesList(From, To, check, total);
    //}
}
