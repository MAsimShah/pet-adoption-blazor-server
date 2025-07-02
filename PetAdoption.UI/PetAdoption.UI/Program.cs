using Blazored.LocalStorage;
using Blazored.SessionStorage;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor.Services;
using PetAdoption.UI.Auth;
using PetAdoption.UI.Components;
using PetAdoption.UI.Interfaces;
using PetAdoption.UI.Services;
using Refit;

var builder = WebApplication.CreateBuilder(args);

// Add MudBlazor services
builder.Services.AddMudServices();

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents().AddCircuitOptions(e => e.DetailedErrors = true);

builder.Services.AddRefitClient<IPetAdoptionAPI>().ConfigureHttpClient(c => c.BaseAddress = new Uri("https://localhost:7039"));
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7039") });

builder.Services.AddAuthentication().AddScheme<AuthenticationSchemeOptions, CustomJwtAuthenticateHandler>("jwt", options => { });
builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState(); // implment cascading state

builder.Services.AddBlazoredLocalStorage();
builder.Services.AddBlazoredSessionStorage();
builder.Services.AddScoped<LocalStorageService>();
builder.Services.AddScoped<CookieStorageService>();
//builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
builder.Services.AddScoped<CustomAuthStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(provider =>
    provider.GetRequiredService<CustomAuthStateProvider>());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.UseAuthentication();
app.UseAuthorization();

app.Run();