using UrfuPassSystem.Domain.Models.Statistic;

namespace UrfuPassSystem.Domain.Services.StatisticMaker;

/// <summary>Сервис генерации статистики.</summary>
public interface IStatisticMaker
{
    /// <summary>Генерирует статистику фотографий.</summary>
    /// <param name="dbContext">Контекст базы данных.</param>
    /// <returns>Статистика.</returns>
    public Task<TotalImageStatistic> MakeImageStatistic(ApplicationDbContext dbContext, DateTime? from, DateTime? to);

    /// <summary>Генерирует статистику плохих фотографий.</summary>
    /// <param name="dbContext">Контекст базы данных.</param>
    /// <returns>Статистика.</returns>
    public Task<TotalBadImageStatistic> MakeBadImageStatistic(ApplicationDbContext dbContext, DateTime? from, DateTime? to);

    ///// <summary>Генерирует список.</summary>
    ///// <param name="dbContext">Контекст базы данных.</param>
    ///// <returns>Список.</returns>
    //public Task<BadImagesList> MakeBadImagesList(ApplicationDbContext dbContext, DateTime? From, DateTime? To);
}
