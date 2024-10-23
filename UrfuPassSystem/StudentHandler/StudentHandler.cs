using UrfuPassSystem.Data;

namespace UrfuPassSystem.StudentHandler;

public class StudentHandler(ApplicationDbContext database) : IStudentHandler
{
    private readonly ApplicationDbContext _database = database;

    public async Task<Student?> CreateStudentFromFileName(string fileName)
    {
        fileName = Path.GetFileNameWithoutExtension(fileName);
        var parts = fileName.Split(' ');
        if (parts.Length < 2)
            return null;

        var lastName = parts[0];
        var firstName = parts[1];
        var patronymic = parts.Length >= 3 ? parts[2] : null;

        // TODO: check if exist

        var student = new Student
        {
            LastName = lastName,
            FirstName = firstName,
            Patronymic = patronymic
        };
        await _database.Students.AddAsync(student);
        await _database.SaveChangesAsync();
        return student;
    }
}
