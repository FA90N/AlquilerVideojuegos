namespace Alquileres.Application.Extensions
{
    public static class StringExtension
    {
        public static bool IsNullOrWhiteSpace(this string? value) => string.IsNullOrWhiteSpace(value);

        public static bool IsNullOrEmpty(this string? value) => string.IsNullOrEmpty(value);

        public static bool IsNullOrEmptyOrWhiteSpace(this string? value) => value.IsNullOrEmpty() || value.IsNullOrWhiteSpace();

        public static bool IsNotNullOrWhiteSpace(this string? value) => !string.IsNullOrWhiteSpace(value);

        public static bool IsNotNullOrEmpty(this string? value) => !string.IsNullOrEmpty(value);
    }
}
