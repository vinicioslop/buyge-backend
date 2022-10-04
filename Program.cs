var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSwaggerGen();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/api", () => "Resposta ao método GET");
app.MapPost("/api", () => "Resposta ao método POST");
app.MapPut("/api", () => "Resposta ao método PUT");
app.MapDelete("/api", () => "Resposta ao método DELETE");
app.MapMethods("/api", new[] { "PATCH" }, () => "Resposta ao método PATCH");

app.Run();
