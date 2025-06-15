using Microsoft.AspNetCore.Authentication.Cookies;
using PetAdoption.UI.Components;
using PetAdoption.UI.Helpers;
using Refit;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddBlazorBootstrap();
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/login";  // Make sure this path corresponds to your login page
    });
builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState(); // implment cascading state

builder.Services.AddRefitClient<IPetAdoptionAPI>().ConfigureHttpClient(c => c.BaseAddress = new Uri("https://localhost:7039"));
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7039") });

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
