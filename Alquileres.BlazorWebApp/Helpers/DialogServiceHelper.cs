using Radzen;

namespace Alquileres.Helpers;

public static class DialogServiceHelper
{
    public static DialogOptions DialogOptionsBuilder(
        string width = "1000px",
        string height = "550px",
        bool resizable = true,
        bool draggable = true,
        bool closeDialogOnOverlayClick = true,
        bool showClose = false)
    {
        return new DialogOptions()
        {
            Width = width,
            Height = height,
            Resizable = resizable,
            Draggable = draggable,
            CloseDialogOnOverlayClick = closeDialogOnOverlayClick,
            ShowClose = showClose
        };
    }
}
