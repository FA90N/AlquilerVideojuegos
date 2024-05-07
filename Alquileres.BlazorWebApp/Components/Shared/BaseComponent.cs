using CurrieTechnologies.Razor.SweetAlert2;
using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Radzen;

namespace Alquileres.Components.Shared;

public abstract class BaseComponent : ComponentBase
{
    [Inject]
    public DialogService DialogService { get; set; } = null!;

    [Inject]
    public IMediator Mediator { get; set; } = null!;

    [Inject]
    public NavigationManager NavigationManager { get; set; } = null!;

    [Inject]
    public NotificationService NotificationService { get; set; } = null!;

    [Inject]
    public SweetAlertService Swal { get; set; } = null!;

    [Inject]
    public IJSRuntime JSRuntime { get; set; } = null!;
}
