
using FindDriver;

var builder = WebApplication.CreateBuilder(args);
//Startup.cs
var startup = new Startup(builder.Configuration);
//Startup.cs ConfigureService
startup.ConfigureServices(builder.Services);

var app = builder.Build();

//Startup.cs Configure
startup.Configure(app);
app.Run();
