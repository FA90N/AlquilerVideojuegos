using Alquileres.Application.Extensions;
using Radzen;
using System.Reflection;

namespace Alquileres.Helpers;

public static class DisplayNameAttributesHelper
{
    public static string GetDisplayNameFromPropertyInfo(PropertyInfo propertyInfo)
    {
        var atts = propertyInfo.GetCustomAttributes(typeof(GridModelDisplayAttribute), true);
        return atts.Length == 0 ? propertyInfo.Name : (atts[0] as GridModelDisplayAttribute)!.Name;
    }

    public static string GetDisplayLabelFromPropertyInfo(PropertyInfo propertyInfo)
    {
        var atts = propertyInfo.GetCustomAttributes(typeof(FormFieldDisplayAttribute), true);
        return atts.Length == 0 ? propertyInfo.Name : (atts[0] as FormFieldDisplayAttribute)!.Label;
    }

    public static string GetFormatString(PropertyInfo propertyInfo)
    {
        var atts = propertyInfo.GetCustomAttributes(typeof(GridModelDisplayAttribute), true);
        return atts.Length == 0 ? string.Empty : (atts[0] as GridModelDisplayAttribute)!.FormatString;
    }

    public static string GetWidth(PropertyInfo propertyInfo)
    {
        var atts = propertyInfo.GetCustomAttributes(typeof(GridModelDisplayAttribute), true);
        return atts.Length == 0 ? "auto" : (atts[0] as GridModelDisplayAttribute)!.Width;
    }

    public static bool GetFooter(PropertyInfo propertyInfo)
    {
        var atts = propertyInfo.GetCustomAttributes(typeof(GridModelDisplayAttribute), true);
        return atts.Length == 0 ? false : (atts[0] as GridModelDisplayAttribute)!.HasFooter;
    }

    public static string GetFooterText(PropertyInfo propertyInfo)
    {
        var atts = propertyInfo.GetCustomAttributes(typeof(GridModelDisplayAttribute), true);
        return atts.Length == 0 ? string.Empty : (atts[0] as GridModelDisplayAttribute)!.FooterText;
    }

    public static SortOrder? GetSortOrder(PropertyInfo propertyInfo)
    {
        var atts = propertyInfo.GetCustomAttributes(typeof(GridModelDisplayAttribute), true);

        if (atts.Length == 0) return null;

        var result = (atts[0] as GridModelDisplayAttribute)!.Sort;

        switch (result)
        {
            case Application.Enums.SortOrderEnum.Ascending:
                return SortOrder.Ascending;
            case Application.Enums.SortOrderEnum.Descending:
                return SortOrder.Descending;
            case Application.Enums.SortOrderEnum.None:
                return null;
            default:
                return null;
        }

    }

    public static List<PropertyInfo> GetProperties(Type model)
    {
        var properties = model.GetProperties().ToList();

        var keys = new List<(int, PropertyInfo?)>();

        foreach (var property in properties)
        {
            var attributes = property.GetCustomAttributes(typeof(GridModelDisplayAttribute), true);

            if (attributes.Any(x => (x as GridModelDisplayAttribute).Display))
                keys.AddRange(attributes.Select(x => ((x as GridModelDisplayAttribute).Order, property)));
        }

        return keys.OrderBy(x => x.Item1).Select(x => x.Item2).ToList();
    }

    public static bool IsNumericType(Type type)
    {
        return
            type.IsPrimitive && type != typeof(bool) ||
            type == typeof(decimal) ||
            type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>) &&
             Nullable.GetUnderlyingType(type).IsPrimitive &&
             Nullable.GetUnderlyingType(type) != typeof(bool);
    }

    public static bool IsDateType(Type type)
    {
        return type == typeof(DateTime) || type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>) && Nullable.GetUnderlyingType(type) == typeof(DateTime);
    }

    public static bool IsBoolType(PropertyInfo propertyInfo)
    {
        return propertyInfo.PropertyType == typeof(bool) || propertyInfo.PropertyType == typeof(System.Boolean);
    }

    public static TextAlign GetAlign(Type type)
    {
        if (IsNumericType(type)) return TextAlign.Right;

        if (IsDateType(type)) return TextAlign.Center;

        if (type == typeof(bool)) return TextAlign.Center;

        return TextAlign.Left;
    }
}
