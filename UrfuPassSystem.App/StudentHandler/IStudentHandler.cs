using UrfuPassSystem.Domain.Entities;

namespace UrfuPassSystem.App.StudentHandler;

public interface IStudentHandler
{
    Task<Student?> CreateStudentFromFileName(string fileName);
}
