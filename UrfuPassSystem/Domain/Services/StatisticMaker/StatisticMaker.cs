using Microsoft.EntityFrameworkCore;
using UrfuPassSystem.Domain.Models.Statistic;

namespace UrfuPassSystem.Domain.Services.StatisticMaker;

public class StatisticMaker : IStatisticMaker
{
    public async Task<TotalImageStatistic> MakeImageStatistic(ApplicationDbContext dbContext)
    {
        var total = await dbContext.Images
            .Where(i => !i.IsDeleted)
            .CountAsync();
        var success = await dbContext.Images
            .Where(i => !i.IsDeleted)
            .Include(i => i.Checks)
            .Where(i => !i.Checks!.Any(c => !c.IsDeleted && !c.IsSuccess))
            .CountAsync();
        return new TotalImageStatistic(DateTime.UnixEpoch, DateTime.UtcNow, success, total);
    }

    public async Task<TotalBadImageStatistic> MakeBadImageStatistic(ApplicationDbContext dbContext)
    {
        var groups = await dbContext.Images
            .Where(i => !i.IsDeleted)
            .Include(i => i.Checks!.OrderBy(c => c.CheckTime))
            .GroupBy(i => i.Checks!.Last(c => !c.IsDeleted).ResultCode)
            .Where(i => i.Key != Enums.ImageCheckResultCode.Success)
            .Select(g => new BadImageStatistic(g.Key, g.Count()))
            .ToArrayAsync();
        return new TotalBadImageStatistic(DateTime.UnixEpoch, DateTime.UtcNow, groups);
    }

    public async Task<ImageCheckStatistic> MakeImageCheckStatistic(ApplicationDbContext dbContext)
    {
        var total = await dbContext.Images
            .Where(i => !i.IsDeleted)
            .CountAsync();
        var check = await dbContext.Images
            .Where(i => !i.IsDeleted)
            .Include(i => i.Checks)
            .Where(i => i.Checks!.Any(c => !c.IsDeleted && !c.IsAuto))
            .CountAsync();
        return new ImageCheckStatistic(DateTime.UnixEpoch, DateTime.UtcNow, check, total);
    }
}
