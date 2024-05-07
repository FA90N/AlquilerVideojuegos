using Alquileres.Application.Enums;

namespace Alquileres.Application.Extensions;

[AttributeUsage(AttributeTargets.Property)]
public sealed class GridModelDisplayAttribute : Attribute
{
    public string? Name { get; set; }

    public int Order { get; set; } = 99;

    public bool Display { get; set; } = true;

    public string FormatString { get; set; }

    public string Width { get; set; }

    public SortOrderEnum Sort { get; set; }

    public bool HasFooter { get; set; }

    public string FooterText { get; set; }



}
