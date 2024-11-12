using UrfuPassSystem.App.Components;
using UrfuPassSystem.App.ArchiveHandler;
using UrfuPassSystem.App.StudentHandler;
using UrfuPassSystem.App.ImageHandler;
using UrfuPassSystem.App.ImageProcessor;
using Microsoft.EntityFrameworkCore;
using UrfuPassSystem.Domain.Services;

if (!Directory.Exists("images"))
    Directory.CreateDirectory("images");
if (!Directory.Exists("images/temp"))
    Directory.CreateDirectory("images/temp");
if (!Directory.Exists("images/raw"))
    Directory.CreateDirectory("images/raw");
if (!Directory.Exists("images/processed"))
    Directory.CreateDirectory("images/processed");

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddDbContext<ApplicationDbContext>(o => o
        .UseInMemoryDatabase("inmemorydb"));
}
else
{
    builder.Services.AddDbContext<ApplicationDbContext>(o => o
        .UseNpgsql(builder.Configuration.GetConnectionString("database")
            ?? throw new InvalidOperationException("Connection string 'database' not found.")));
}

builder.Services.AddOptions<ImageProcessorOptions>("ImageProcessor");
builder.Services.AddSingleton<IArchiveHandler, ArchiveHandler>();
builder.Services.AddScoped<IStudentHandler, StudentHandler>();
builder.Services.AddSingleton<IImageProcessor, ImageProcessor>();
builder.Services.AddScoped<IImageHandler, ImageHandler>();

var app = builder.Build();

await using (var scope = app.Services.CreateAsyncScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    if (await db.Database.EnsureCreatedAsync())
        scope.ServiceProvider.GetRequiredService<ILogger<Program>>().LogInformation("Database created.");
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
