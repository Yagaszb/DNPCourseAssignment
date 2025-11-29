using BlazorApp.Auth;
using BlazorApp.Components;
using BlazorApp.Service;
using Microsoft.AspNetCore.Components.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddHttpClient<IUserService, HttpUserService>(c =>
    c.BaseAddress = new Uri("https://localhost:7144/"));
builder.Services.AddHttpClient<IPostService, HttpPostService>(c =>
    c.BaseAddress = new Uri("https://localhost:7144/"));
builder.Services.AddHttpClient<ICommentService, HttpCommentService>(c =>
    c.BaseAddress = new Uri("https://localhost:7144/"));
builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri("https://localhost:7144/")
});

// 🔑 Correct lifetime for auth provider
builder.Services.AddScoped<AuthenticationStateProvider, SimpleAuthProvider>();
builder.Services.AddAuthorizationCore();

/*builder.Services.AddScoped<IUserService, HttpUserService>();
builder.Services.AddScoped<IPostService, HttpPostService>();
builder.Services.AddScoped<ICommentService, HttpCommentService>();

builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri("http://localhost:5142/")
});*/

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/not-found");

app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();