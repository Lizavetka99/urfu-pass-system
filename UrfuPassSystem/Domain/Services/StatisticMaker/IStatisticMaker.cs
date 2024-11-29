using UrfuPassSystem.Domain.Models.Statistic;

namespace UrfuPassSystem.Domain.Services.StatisticMaker;

/// <summary>Сервис генерации статистики.</summary>
public interface IStatisticMaker
{
    /// <summary>Генерирует статистику фотографий.</summary>
    /// <param name="dbContext">Контекст базы данных.</param>
    /// <returns>Статистика.</returns>
    public Task<TotalImageStatistic> MakeImageStatistic(ApplicationDbContext dbContext);

    /// <summary>Генерирует статистику плохих фотографий.</summary>
    /// <param name="dbContext">Контекст базы данных.</param>
    /// <returns>Статистика.</returns>
    public Task<TotalBadImageStatistic> MakeBadImageStatistic(ApplicationDbContext dbContext);

    /// <summary>Генерирует статистику проверенных фотограрфий.</summary>
    /// <param name="dbContext">Контекст базы данных.</param>
    /// <returns>Статистика.</returns>
    public Task<ImageCheckStatistic> MakeImageCheckStatistic(ApplicationDbContext dbContext);
}
