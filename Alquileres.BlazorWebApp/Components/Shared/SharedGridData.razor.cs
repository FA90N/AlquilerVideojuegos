using Alquileres.Application.Extensions;
using Alquileres.Application.Interfaces.Application;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Radzen;
using Radzen.Blazor;
using System.Text.Json;

namespace Alquileres.Components.Shared;

public partial class SharedGridData<TItem>
{
    enum ExportDataType
    {
        CSV,
        Excel,
        Pdf
    }

    [Parameter]
    public IEnumerable<TItem> Data { get; set; } = null!;

    [Parameter]
    public int TotalCount { get; set; }

    [Parameter]
    public EventCallback<TItem> OnClickEditRowCallback { get; set; }

    [Parameter]
    public EventCallback<TItem> OnClickDeleteRowCallback { get; set; }

    [Parameter]
    public EventCallback<TItem> OnClickPreviewRowCallback { get; set; }

    [Parameter]
    public EventCallback<TItem> OnClickCreateButtonCallback { get; set; }

    [Parameter]
    public EventCallback<bool> OnClickChangeSelectorCallback { get; set; }

    [Parameter]
    public EventCallback<LoadDataArgs> OnLoadDataCallback { get; set; }

    [Parameter]
    public EventCallback<bool> OnAfterRenderAsync { get; set; }

    [Parameter]
    public EventCallback<DataGridCellMouseEventArgs<TItem>> CellClick { get; set; }

    [Parameter]
    public EventCallback<DataGridCellMouseEventArgs<TItem>> CellContextMenu { get; set; }

    [Parameter]
    public EventCallback<RadzenSplitButtonItem> OnClickSplitButtonCallback { get; set; }

    [Parameter]
    public FilterMode FilterMode { get; set; } = FilterMode.Advanced;

    [Parameter]
    public EventCallback<TItem> OnClickExtraButtonCallback { get; set; }

    [Parameter]
    public IList<TItem> SelectedItems { get; set; } = new List<TItem>();

    [Parameter]
    public string PanelHeight { get; set; } = "84vh";

    [Parameter]
    public string GridHeight { get; set; } = "74vh";

    public int PageSize { get; set; } = 15;

    public IEnumerable<int> PageSizeOptions { get; set; } = new int[] { 15, 50, 500 };

    RadzenDataGrid<TItem> grid = null!;

    public IList<RadzenDataGridColumn<TItem>> ColumnsCollection => grid.ColumnsCollection;

    DateTime? filterDate;

    public bool IsLoading { get; set; }

    [Parameter]
    public bool PreviewIcon { get; set; }

    [Parameter]
    public bool AllowFiltering { get; set; } = true;

    [Parameter]
    public bool AllowPaging { get; set; } = true;

    [Parameter]
    public bool ShowCreateButton { get; set; } = true;

    [Parameter]
    public bool ShowEditButton { get; set; } = true;

    [Parameter]
    public bool ShowDeleteButton { get; set; } = true;

    [Parameter]
    public bool ShowExportButtons { get; set; } = true;

    [Parameter]
    public bool ShowSplitButton { get; set; }

    [Parameter]
    public string SplitButtonLabel { get; set; } = null!;

    [Parameter]
    public List<KeyValuePair<string, string>> SplitButtonItems { get; set; } = new List<KeyValuePair<string, string>>();

    [Parameter]
    public bool ShowExtraButton { get; set; }

    [Parameter]
    public string ExtraButtonLabel { get; set; } = null!;

    [Parameter]
    public bool AllowRowSelectOnRowClick { get; set; } = false;

    [Parameter]
    public string CreateButtonText { get; set; } = "CREAR";

    [Inject]
    public IJSRuntime JSRuntime { get; set; } = null!;

    [Inject]
    public NavigationManager Navigator { get; set; } = null!;

    [Inject]
    public IExportServices ExportServices { get; set; } = null!;

    public Dictionary<string, string> PickerColumnsSelected { get; set; } = [];

    async Task LoadData(LoadDataArgs args)
    {
    
        IsLoading = true;
        await Task.Yield();
        if (args != null)
        {
            args.Filter = args.Filter.Replace("np(", "(");
        }

        await OnLoadDataCallback.InvokeAsync(args);
        IsLoading = false;
    }

    async Task EditRow(TItem item)
    {
        await OnClickEditRowCallback.InvokeAsync(item);
    }

    async Task DeleteRow(TItem item)
    {
        await OnClickDeleteRowCallback.InvokeAsync(item);
    }

    async Task PreviewRow(TItem item)
    {
        await OnClickPreviewRowCallback.InvokeAsync(item);
    }

    async Task CreateButton()
    {
        await OnClickCreateButtonCallback.InvokeAsync();
    }

    async Task ExtraButtonClick()
    {
        await OnClickExtraButtonCallback.InvokeAsync();
    }

    async Task OnChangeHeaderTemplateRadzenCheckBox(bool? args)
    {
        SelectedItems = args == true ? Data.ToList() : new List<TItem>();
        await OnClickChangeSelectorCallback.InvokeAsync(!SelectedItems.Any());
    }

    async Task OnChangeTemplateRadzenCheckBox(bool args, TItem data)
    {
        if (!AllowRowSelectOnRowClick) { await grid.SelectRow(data); }
        await OnClickChangeSelectorCallback.InvokeAsync(SelectedItems.Contains(data));
    }

    async Task OnRowSelected(TItem data)
    {
        await OnClickChangeSelectorCallback.InvokeAsync(SelectedItems.Contains(data));
    }

    async Task RowDeselect(TItem data)
    {
        await OnClickChangeSelectorCallback.InvokeAsync(!SelectedItems.Any() || SelectedItems.Count == 1);
    }

    public async Task ReloadData()
    {
        await grid.Reload();
    }

    async Task OnClickExportData(ExportDataType type)
    {
        if (!Data.Any()) return;

        var uri = Navigator.Uri.Split("/");
        var page = uri[^1];
        var jsonString = JsonSerializer.Serialize(Data);

        var replacements = ColumnsCollection
            .Where(x => x.Property.IsNotNullOrEmpty())
            .Select(x => new KeyValuePair<string, string>(x.Property, x.Title))
            .ToDictionary();

        if (PickerColumnsSelected.Count > 0)
        {
            replacements = PickerColumnsSelected;
        }

        var result = ExportServices.BuildJson(jsonString, replacements);

        byte[] data = null!;
        string fileName = null!;
        string contentType = null!;

        switch (type)
        {
            case ExportDataType.CSV:
                data = ExportServices.ToCSV(result);
                fileName = $"{page}.csv";
                contentType = "text/csv";
                break;
            case ExportDataType.Excel:
                data = ExportServices.ToExcel(result);
                fileName = $"{page}.xlsx";
                contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                break;
            case ExportDataType.Pdf:
                data = ExportServices.ToPdf(result);
                fileName = $"{page}.pdf";
                contentType = "application/pdf";
                break;
        }

        await JSRuntime.InvokeVoidAsync("downloadFileFromStream", fileName, contentType, data);
    }

    async void OnClickSplitButton(RadzenSplitButtonItem item)
    {
        await OnClickSplitButtonCallback.InvokeAsync(item);
    }

    void OnPickedColumnsChanged(DataGridPickedColumnsChangedEventArgs<TItem> args)
    {
        PickerColumnsSelected = args.Columns
            .Select(x => new KeyValuePair<string, string>(x.Property, x.Title))
            .ToDictionary();
    }
}
