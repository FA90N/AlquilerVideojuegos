namespace Alquileres.Application.Extensions;

[AttributeUsage(AttributeTargets.Property)]
public sealed class FormFieldDisplayAttribute(Type type) : Attribute
{
    public bool Display { get; set; } = true;

    public int Order { get; set; } = 99;

    public int Column { get; set; } = -1;

    public string? Label { get; set; }

    public string? Name { get; set; }

    public Type Type { get; set; } = type;
}
