using WebEmpresa.Components;

var builder = WebApplication.CreateBuilder(args);

// Blazor Web App + Interactive Server
builder.Services
    .AddRazorComponents()
    .AddInteractiveServerComponents();

// HttpClient para consumir la API
builder.Services.AddScoped<HttpClient>(sp =>
{
    return new HttpClient
    {
        BaseAddress = new Uri("https://localhost:7230/")
    };
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();

// Middleware de seguridad endurecido para cumplir al 100% con las reglas de CSP
app.Use(async (context, next) =>
{
    context.Response.Headers["Content-Security-Policy"] = 
        "default-src 'self'; " +
        "script-src 'self'; " +
        "style-src 'self'; " +
        "img-src 'self' data:; " +
        "connect-src 'self' ws://localhost:* wss://localhost:*; " +
        "frame-ancestors 'self'; " +
        "form-action 'self'; " +
        "object-src 'none';";
    
    context.Response.Headers["X-Content-Type-Options"] = "nosniff";

    await next();
});

app.UseStaticFiles();

app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();