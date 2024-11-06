using UrfuPassSystem.App.Data;

namespace UrfuPassSystem.App.StudentHandler;

public interface IStudentHandler
{
    Task<Student?> CreateStudentFromFileName(string fileName);
}
