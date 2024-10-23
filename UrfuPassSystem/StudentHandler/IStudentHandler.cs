using UrfuPassSystem.Data;

namespace UrfuPassSystem.StudentHandler;

public interface IStudentHandler
{
    Task<Student?> CreateStudentFromFileName(string fileName);
}
