var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMemoryCache(); // InMemory-tietokanta
builder.Services.AddSingleton<UserService>(); // Käyttäjäpalvelu
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseAuthorization();
app.MapControllers();
app.Run();
