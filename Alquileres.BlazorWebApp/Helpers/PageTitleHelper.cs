namespace Alquileres.Helpers;

public static class PageTitleHelper
{
    public static string GetFullAppTitle(string pageTitle) => typeof(Program).Assembly.GetName().Name!.Split(".")[0] + " - " + pageTitle;
}
