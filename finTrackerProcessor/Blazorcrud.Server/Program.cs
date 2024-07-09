using Blazorcrud.Server.Authorization;
using Blazorcrud.Server.Helpers;
using Blazorcrud.Server.Models;
using Blazorcrud.Server.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;
using EvolveDb;
using Quartz;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlite("Filename=./BlazorServerCRUD.sqlite"));
builder.Services.AddScoped<IPersonRepository, PersonRepository>();
builder.Services.AddScoped<IUploadRepository, UploadRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IFileRepository, FileRepository>();
builder.Services.AddScoped<IExcelReader, ExcelReader>();
builder.Services.AddScoped<IJwtUtils, JwtUtils>();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo 
    { 
        Title = "Blazor CRUD API",
        Version = "v1",
        Description = "CRUD API Services that act as the backend to the Blazor CRUD website."
    });

    // Set the comments path for the Swagger JSON and UI.
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
    c.CustomSchemaIds(r => r.FullName);
});

builder.Services.AddQuartz(q =>
{
    q.UseMicrosoftDependencyInjectionJobFactory();
    q.AddJobAndTrigger<UploadProcessorJob>(builder.Configuration);
});
builder.Services.AddQuartzHostedService(
    q => q.WaitForJobsToComplete = true);

builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();

    RunDbMigration(services, logger);

    await RunCreateEntities(services, logger);
}


void RunDbMigration(IServiceProvider services, ILogger<Program> logger2)
{
    try
    {
        var appDbContext = services.GetRequiredService<AppDbContext>();
        var evolve = new Evolve(appDbContext.Database.GetDbConnection(), msg => logger2.LogInformation(msg))
        {
            Locations = new[] { "sql/db/migrations" },
            IsEraseDisabled = true,
            OutOfOrder = true
        };

        evolve.Migrate();
    }
    catch (Exception ex)
    {
        logger2.LogError(ex, "Database migration failed.");
    }
}


async Task RunCreateEntities(IServiceProvider serviceProvider, ILogger<Program> logger)
{
    try
    {
        var fileRepository = serviceProvider.GetRequiredService<FileRepository>();
        var appDbContext = serviceProvider.GetRequiredService<AppDbContext>();
        await DataGenerator.Initialize(appDbContext, fileRepository);
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "An error occurred creating the DB.");
    }
}


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseSwagger();
app.UseSwaggerUI(c => 
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "blazorcrud.api v1");
    c.DefaultModelsExpandDepth(-1);
});

app.UseHttpsRedirection();
app.UseBlazorFrameworkFiles();
app.UseStaticFiles();
app.UseRouting();

app.UseMiddleware<ErrorHandlerMiddleware>();
app.UseMiddleware<JwtMiddleware>();

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();