namespace UrfuPassSystem.Domain.Enums;

public enum ModeratorAccessLevel
{
    None = 0,
    /// <summary>Имеет доступ к фотографиям, доступным организации.</summary>
    OrganizationEmployee = 1,
    /// <summary>Имеет доступ и к управлению персоналом организации.</summary>
    OrganizationAdmin = 2,
    /// <summary>Имеет доступ ко всем фотографиям и к управлению всеми организациями.</summary>
    GlobalAdmin = 3
}
