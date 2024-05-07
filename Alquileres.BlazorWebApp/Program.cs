using Alquileres.Application.Behaviours;
using Alquileres.Application.Interfaces.Application;
using Alquileres.Application.Interfaces.Infrastructure.Repositories;
using Alquileres.Application.Interfaces.Infrastructure.Services;
using Alquileres.Application.Services;
using Alquileres.Components;
using Alquileres.Components.Account;
using Alquileres.Infrastructure.Persistence;
using Alquileres.Infrastructure.Repositories;
using Alquileres.Infrastructure.Services;
using CurrieTechnologies.Razor.SweetAlert2;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Radzen;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddLocalization();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

builder.Services.AddDbContextFactory<AppDbContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DBConnection"), opt => { opt.CommandTimeout(60); });
});

builder.Services.AddDbContext<AppIdentityDbContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("DBConnection")));

builder.Services.AddIdentityCore<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<AppIdentityDbContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();

builder.Services.AddAuthorization();
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = IdentityConstants.ApplicationScheme;
    options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
})
.AddIdentityCookies();

builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
builder.Services.AddTransient(typeof(IRepositoryBase<>), typeof(RepositoryBase<>));
builder.Services.AddScoped<IExportServices, ExportServices>();
builder.Services.AddScoped<IHtmlToPdfService, HtmlToPdfService>();
builder.Services.AddScoped<IPdfService, PdfService>();
builder.Services.AddScoped<IAzureStorageService, AzureStorageService>();
builder.Services.AddScoped<ISmtpMailSenderService, SmtpMailSenderService>();

// Add AutoMapper
builder.Services.AddAutoMapper(config =>
{
    config.AllowNullCollections = true;
    config.AllowNullDestinationValues = true;
}, typeof(Alquileres.Application.Application).Assembly);

// Add MediatR
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(typeof(Alquileres.Application.Application).Assembly);
    config.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
});

// Add FluentValidation
builder.Services.AddValidatorsFromAssembly(typeof(Alquileres.Application.Application).Assembly);

builder.Services.AddRadzenComponents();
builder.Services.AddSweetAlert2();

var app = builder.Build();

app.UseExceptionHandler("/Error", createScopeForErrors: true);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.UseRequestLocalization("es-ES");

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Add additional endpoints required by the Identity /Account Razor components.
app.MapAdditionalIdentityEndpoints();

app.UseStatusCodePages(async context =>
{
    if (context.HttpContext.Response.StatusCode == StatusCodes.Status404NotFound)
    {
        // Redirecciona a la URL de la página de errores
        context.HttpContext.Response.Redirect("/pagina-no-encontrada");
    }
});

app.Run();