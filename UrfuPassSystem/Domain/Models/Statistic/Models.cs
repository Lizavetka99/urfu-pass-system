using UrfuPassSystem.Domain.Enums;

namespace UrfuPassSystem.Domain.Models.Statistic;

/// <summary>Количество фотографий за промежуток времени.</summary>
/// <param name="From">Начало промежутка времени.</param>
/// <param name="To">Конец промежутка времени.</param>
/// <param name="Success">Количество хороших.</param>
/// <param name="Total">Общее количество.</param>
public record TotalImageStatistic(DateTime? From, DateTime? To, int Success, int Total);

/// <param name="ResultCode">Причина.</param>
/// <param name="Count">Количество фотографий.</param>
public record BadImageStatistic(ImageCheckResultCode ResultCode, int Count);

/// <summary>Количество фотографий по каждой из причин за промежуток времени.</summary>
/// <param name="From">Начало промежутка времени.</param>
/// <param name="To">Конец промежутка времени.</param>
/// <param name="Statistics">Список количества по причинам.</param>
public record TotalBadImageStatistic(DateTime? From, DateTime? To, IReadOnlyList<BadImageStatistic> Statistics);

///// <summary></summary>
///// <param name="ImagePath"></param>
///// <param name="ResultCode"></param>
///// <param name="Group"></param>
//public record BadImageEntry(string ImagePath, ImageCheckResultCode ResultCode, string Group);

///// <summary>Список неподходящих фотографий.</summary>
///// <param name="From">Начало промежутка времени.</param>
///// <param name="To">Конец промежутка времени.</param>
///// <param name="Entries">Список фотографий.</param>
//public record BadImagesList(DateTime? From, DateTime? To, IReadOnlyList<BadImageEntry> Entries);
