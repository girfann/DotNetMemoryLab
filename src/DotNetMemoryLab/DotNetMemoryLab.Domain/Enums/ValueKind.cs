namespace DotNetMemoryLab.Domain.Enums;

/// <summary>
/// Input value category for placement explanations.
/// </summary>
public enum ValueKind
{
    /// <summary>
    /// Reference type instance.
    /// </summary>
    Class = 0,

    /// <summary>
    /// Value type instance.
    /// </summary>
    Struct = 1,

    /// <summary>
    /// Array instance.
    /// </summary>
    Array = 2
}
