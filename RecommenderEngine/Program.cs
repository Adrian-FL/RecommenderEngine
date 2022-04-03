using RecommenderUtils.Engine;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddHostedService<ClusterBackgroundService>();

var app = builder.Build();

app.UseRouting();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action}/{id?}");

RecommenderUtils.DAL.RavenDb.Init();


app.Run();