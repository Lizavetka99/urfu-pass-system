using UrfuPassSystem.App.Components;
using UrfuPassSystem.App.ArchiveHandler;
using UrfuPassSystem.App.StudentHandler;
using UrfuPassSystem.App.ImageHandler;
using Microsoft.EntityFrameworkCore;
using UrfuPassSystem.Domain.Services;
using UrfuPassSystem.Infrastructure.ImageStorage;
using UrfuPassSystem.Infrastructure.ImageProcessor;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;

if (!Directory.Exists("images"))
    Directory.CreateDirectory("images");
if (!Directory.Exists("images/temp"))
    Directory.CreateDirectory("images/temp");

var builder = WebApplication.CreateBuilder(args);

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

builder.Services.Configure<ImageStorageOptions>(builder.Configuration.GetSection("ImageStorage"));
builder.Services.AddSingleton<IImageStorage, ImageStorage>();

builder.Services.Configure<ImageProcessorOptions>(builder.Configuration.GetSection("ImageProcessor"));
builder.Services.AddSingleton<IArchiveHandler, ArchiveHandler>();
builder.Services.AddScoped<IStudentHandler, StudentHandler>();
builder.Services.AddSingleton<IImageProcessor, ImageProcessor>();
builder.Services.AddScoped<IImageHandler, ImageHandler>();

var app = builder.Build();

{
    var options = app.Services.GetRequiredService<IOptions<ImageStorageOptions>>().Value;
    if (!Directory.Exists(options.TempPath))
        Directory.CreateDirectory(options.TempPath);
    if (!Directory.Exists(options.ImagesPath))
        Directory.CreateDirectory(options.ImagesPath);
}

await using (var scope = app.Services.CreateAsyncScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    if (await db.Database.EnsureCreatedAsync())
        scope.ServiceProvider.GetRequiredService<ILogger<Program>>().LogInformation("Database created.");
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

//TODO: требование авторизации для доступа к изображением.
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(app.Environment.ContentRootPath, "images")),
    RequestPath = "/images"
});

app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
