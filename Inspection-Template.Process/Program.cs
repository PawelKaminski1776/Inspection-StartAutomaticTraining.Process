using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using System.Globalization;
using InspectionTemplate.Handlers;
using InspectionTemplate.Controllers.DtoFactory;
using InspectionTemplate.Process;
using InspectionTemplate.Channel.Services;
using InspectionTemplate.Messages.Dtos;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<AppConfiguration>(_ => AppConfiguration.Instance);
builder.Services.AddSingleton<IDtoFactory, DtoFactory>();

var appConfig = AppConfiguration.Instance;

builder.Services.AddScoped<MongoConnect>(provider =>
{
    var connectionString = appConfig.GetSetting("ConnectionStrings:DefaultConnection");
    return new MongoConnect(connectionString);
});

builder.Services.AddScoped<MyHandler>();


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy => policy
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});

var endpointConfiguration = new EndpointConfiguration("NServiceBusHandlers");
string instanceId = Environment.MachineName;
endpointConfiguration.MakeInstanceUniquelyAddressable(instanceId);
endpointConfiguration.EnableCallbacks();

var settings = new JsonSerializerSettings
{
    TypeNameHandling = TypeNameHandling.Auto,
    Converters =
    {
        new IsoDateTimeConverter
        {
            DateTimeStyles = DateTimeStyles.RoundtripKind
        }
    }
};
var serialization = endpointConfiguration.UseSerialization<NewtonsoftJsonSerializer>();
serialization.Settings(settings);

var transport = endpointConfiguration.UseTransport<LearningTransport>();
var persistence = endpointConfiguration.UsePersistence<LearningPersistence>();


if (builder.Environment.IsDevelopment())
{
    builder.WebHost.ConfigureKestrel(options =>
    {
        options.ListenAnyIP(5003);
        options.ListenAnyIP(5004, listenOptions =>
        {
            listenOptions.UseHttps();
        });
        transport.StorageDirectory("/app/.learningtransport");
    });
}
else
{
    builder.WebHost.ConfigureKestrel(options =>
    {
        options.ListenAnyIP(5003);
    });
    transport.StorageDirectory("/home/ubuntu/storage");

}

var routing = transport.Routing();
routing.RouteToEndpoint(typeof(MessageRequest), "NServiceBusHandlers");

var scanner = endpointConfiguration.AssemblyScanner().ScanFileSystemAssemblies = true;

builder.UseNServiceBus(endpointConfiguration);

var app = builder.Build();
app.UseRouting();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        c.RoutePrefix = string.Empty;
    });
}
app.UseCors("AllowAll");
app.UseMiddleware<LoggingMiddleware>();
app.UseAuthorization();

app.MapControllers();

app.Run();
