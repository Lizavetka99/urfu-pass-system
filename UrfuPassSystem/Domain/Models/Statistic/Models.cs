using UrfuPassSystem.Domain.Enums;

namespace UrfuPassSystem.Domain.Models.Statistic;

/// <summary>Количество фотографий за промежуток времени.</summary>
/// <param name="From">Начало промежутка времени.</param>
/// <param name="To">Конец промежутка времени.</param>
/// <param name="Success">Количество хороших.</param>
/// <param name="Total">Общее количество.</param>
public record TotalImageStatistic(DateTime From, DateTime To, int Success, int Total);

/// <param name="ResultCode">Причина.</param>
/// <param name="Count">Количество фотографий.</param>
public record BadImageStatistic(ImageCheckResultCode ResultCode, int Count);

/// <summary>Количество фотографий по каждой из причин за промежуток времени.</summary>
/// <param name="From">Начало промежутка времени.</param>
/// <param name="To">Конец промежутка времени.</param>
/// <param name="Statistics">Список количества по причинам.</param>
public record TotalBadImageStatistic(DateTime From, DateTime To, IReadOnlyList<BadImageStatistic> Statistics);

/// <summary>Количество проверенных фотографий за промежуток времени.</summary>
/// <param name="From">Начало промежутка времени.</param>
/// <param name="To">Конец промежутка времени.</param>
/// <param name="Checked">Количество проверенных.</param>
/// <param name="Total">Общее количество.</param>
public record ImageCheckStatistic(DateTime From, DateTime To, int Checked, int Total);
