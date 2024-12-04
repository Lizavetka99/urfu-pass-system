using UrfuPassSystem.Domain.Entities;

namespace UrfuPassSystem.Domain.Services.ImageHandler;

/// <summary>Сервис для работы с Image сущностями.</summary>
public interface IImageHandler
{
    /// <summary>Сохраняет изображение и проводит автоматическую проверку.</summary>
    /// <param name="dbContext">Контекст базы данных.</param>
    /// <param name="rawFilePath">Путь к изображению, которое нужно сохранить.</param>
    /// <param name="mustSaveChanges">Должен ли быть вызван SaveChanges() у dbCoontext после добавления изображения.</param>
    /// <returns>Объект, представляющий сохраненное изображение.</returns>
    Task<Image> SaveAndCheckImage(ApplicationDbContext dbContext, string rawFilePath, bool mustSaveChanges);
}
