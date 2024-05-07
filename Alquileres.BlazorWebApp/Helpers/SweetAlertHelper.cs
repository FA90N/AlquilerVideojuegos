using CurrieTechnologies.Razor.SweetAlert2;

namespace Alquileres.Helpers;

public static class SweetAlertHelper
{
    public static async Task ShowConfirmAlertAsync(SweetAlertService swal, string title, string text, Func<Task<bool>> executeFunction)
    {
        var result = await swal.FireAsync(new SweetAlertOptions
        {
            Title = title,
            Text = text,
            Icon = SweetAlertIcon.Question,
            ShowCancelButton = true,
            ConfirmButtonText = "Sí",
            CancelButtonText = "No"
        });

        if (!string.IsNullOrEmpty(result.Value))
        {
            var resultFunct = await executeFunction.Invoke();

            if (resultFunct)
            {
                await swal.FireAsync(icon: SweetAlertIcon.Success);
            }
            else
            {
                await swal.FireAsync("Error", "Ha ocurrido un problema", SweetAlertIcon.Error);
            }
        }
        else if (result.Dismiss == DismissReason.Cancel)
        {
            await swal.CloseAsync();
        }
    }

    public static async Task ShowThrowErrorAlertAsync(SweetAlertService swal, Exception ex)
    {
        var html = string.Empty;
        var width = "auto";

#if DEBUG
        html = @$"<div>
                <h2 style=""color: #007bff;"">Error:</h2>
                <p><strong>Mensaje de la Excepción:</strong> <span style=""color: #ff6347;"">{ex.Message}</span></p>
                <h3 style=""color: #007bff;"">Stack Trace:</h3>
                <pre style=""background-color: #f2f2f2; color: #333; padding: 10px; border-radius: 5px; border: 1px solid #ccc;"">{ex.StackTrace}</pre>
            </div>";

        width = "1000px";
#else
        html = "Se ha producido un error.";
#endif

        await swal.FireAsync(new SweetAlertOptions
        {
            Title = "Se ha producido un error",
            Html = html,
            Icon = SweetAlertIcon.Error,
            ConfirmButtonText = "Aceptar",
            Width = width
        });
    }

    public static async Task ShowDeleteAlertAsync(SweetAlertService swal, Func<Task<bool>> executeFunction)
    {
        var result = await swal.FireAsync(new SweetAlertOptions
        {
            Title = "Confirmación",
            Text = "¿Estás seguro de que quieres borrar el elemento seleccionado?",
            Icon = SweetAlertIcon.Warning,
            ShowCancelButton = true,
            ConfirmButtonText = "Sí",
            CancelButtonText = "No"
        });

        if (!string.IsNullOrEmpty(result.Value))
        {
            var resultFunct = await executeFunction.Invoke();

            if (resultFunct)
            {
                await swal.FireAsync(
                  "Borrado con éxito",
                  "El elemento se ha borrado con éxito",
                  SweetAlertIcon.Success);
            }
            else
            {
                await swal.FireAsync(
                  "Error",
                  "Ha ocurrido un problema borrando el elemento seleccionado",
                  SweetAlertIcon.Error);
            }
        }
        else if (result.Dismiss == DismissReason.Cancel)
        {
            await swal.CloseAsync();
        }
    }

    public static async Task<bool> ShowGenerateInvoicesAlertAsync(SweetAlertService swal, int totalCount, double? totalInvoiced)
    {
        var result = await swal.FireAsync(new SweetAlertOptions
        {
            Title = "Confirmación",
            Text = $"Has seleccionado {totalCount} albaranes por un importe de {totalInvoiced.Value}€. ¿Estás seguro de que quieres generar las facturas correspondientes?",
            Icon = SweetAlertIcon.Question,
            ShowCancelButton = true,
            ConfirmButtonText = "Sí",
            CancelButtonText = "No"
        });

        if (!string.IsNullOrEmpty(result.Value))
        {
            return true;
        }
        else if (result.Dismiss == DismissReason.Cancel)
        {
            await swal.CloseAsync();

            return false;
        }

        return false;
    }

    public static async Task ShowError(SweetAlertService swal)
    {
        var result = await swal.FireAsync(new SweetAlertOptions
        {
            Title = "No se puede borrar",
            Text = "Tienen alquileres de videojuegos",
            Icon = SweetAlertIcon.Error,
            ConfirmButtonText = "Entendido",
        });

        if (result.Dismiss == DismissReason.Cancel)
        {
            await swal.CloseAsync();
        }
    }
}
