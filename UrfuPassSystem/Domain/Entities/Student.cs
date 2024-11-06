using System.ComponentModel.DataAnnotations;

namespace UrfuPassSystem.Domain.Entities;

public class Student
{
    [Key]
    public Guid Id { get; set; }
    public string? UserName { get; set; }
    public string? LastName { get; set; }
    public string? FirstName { get; set; }
    public string? Patronymic { get; set; }
    public string? Institutes { get; set; }
    public string? Departmnets { get; set; }
    public string? Groups { get; set; }
}
