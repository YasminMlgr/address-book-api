using AddressBookApi.Data;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Register the in-memory database.
builder.Services.AddDbContext<AddressBookContext>(options =>
    options.UseInMemoryDatabase("AddressBookDb"));

// Register controllers and display enums as strings.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(
            new JsonStringEnumConverter());
    });

// Register API documentation services.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Enable Swagger documentation and the interactive UI.
app.UseSwagger();
app.UseSwaggerUI();

// Redirect the home page to Swagger.
app.MapGet("/", () => Results.Redirect("/swagger"));

app.UseAuthorization();
app.MapControllers();

app.Run();