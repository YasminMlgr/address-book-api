using AddressBookApi.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<AddressBookContext>(options =>
    options.UseInMemoryDatabase("AddressBookDb"));


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI();


app.MapGet("/", () => Results.Redirect("/swagger"));

app.UseAuthorization();
app.MapControllers();

app.Run();