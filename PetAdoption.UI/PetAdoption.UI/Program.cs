using Blazored.LocalStorage;
using Blazored.SessionStorage;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.ResponseCompression;
using MudBlazor.Services;
using PetAdoption.UI.Auth;
using PetAdoption.UI.Components;
using PetAdoption.UI.Interfaces;
using PetAdoption.UI.Services;
using PetAdoption.UI.SignalRHubs;
using Refit;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add MudBlazor services
builder.Services.AddMudServices();

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents().AddCircuitOptions(e => e.DetailedErrors = true);

builder.Services.AddRefitClient<IPetAdoptionAPI>(provider => new RefitSettings
     {
         ContentSerializer = new SystemTextJsonContentSerializer(new JsonSerializerOptions
         {
             PropertyNameCaseInsensitive = true,
             Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
         })
     })
    .ConfigureHttpClient(c => c.BaseAddress = new Uri("https://localhost:7039"))
    .AddHttpMessageHandler<AuthorizationHandler>();
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7039") });

builder.Services.AddAuthentication().AddScheme<AuthenticationSchemeOptions, CustomJwtAuthenticateHandler>("jwt", options => { });
builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState(); // implment cascading state

builder.Services.AddHttpContextAccessor(); // Required
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddBlazoredSessionStorage();
builder.Services.AddScoped<LocalStorageService>();
builder.Services.AddScoped<CookieStorageService>();
//builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
builder.Services.AddScoped<AuthorizationHandler>();
builder.Services.AddScoped<CustomAuthStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(provider =>
    provider.GetRequiredService<CustomAuthStateProvider>());

builder.Services.AddSingleton<LoaderService>();

builder.Services.AddSignalR();

builder.Services.AddResponseCompression(opts =>
{
    opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
        ["application/octet-stream"]);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseResponseCompression();
app.MapHub<RequestHub>("/requestHub");

app.UseHttpsRedirection();
app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.UseAuthentication();
app.UseAuthorization();

app.Run();