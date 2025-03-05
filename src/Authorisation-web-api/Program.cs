using Authorisation_web_api.Services; // Lisää tämä!

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMemoryCache(); // InMemory-tietokanta
builder.Services.AddSingleton<UserService>(); // Käyttäjäpalvelu
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthentication();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();
app.MapControllers();
app.Run();
