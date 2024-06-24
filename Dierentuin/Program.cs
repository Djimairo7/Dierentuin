using Microsoft.EntityFrameworkCore;
using Dierentuin.Data;
using Dierentuin.Models;
using Prometheus;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<DierentuinContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DierentuinContext") ?? throw new InvalidOperationException("Connection string 'DierentuinContext' not found.")));

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add health checks and Prometheus metrics
builder.Services.AddHealthChecks();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<DierentuinContext>();

    context.Database.Migrate();
    SeedData.Initialize(services);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseHttpMetrics();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllers();

// Map Prometheus metrics and health check endpoints
app.MapMetrics(); // Exposes metrics at /metrics
app.MapHealthChecks("/health"); // Exposes health checks at /health

app.Run();
