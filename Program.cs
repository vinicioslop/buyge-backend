using buyge_backend.db;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSwaggerGen();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddDbContext<bd_buygeContext>(opt => {
    string connectionString = builder.Configuration.GetConnectionString("buygeConnection");
    var serverVersion = ServerVersion.AutoDetect(connectionString);
    opt.UseMySql(connectionString, serverVersion);
});

var app = builder.Build();

app.UseHttpsRedirection();
app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/api/clientes", ([FromServices] bd_buygeContext _db) =>
{
    return Results.Ok(_db.TbCliente.ToList<TbCliente>());
});

app.MapGet("/api/categorias", ([FromServices] bd_buygeContext _db) =>
{
    return Results.Ok(_db.TbCategoria.ToList<TbCategoria>());
});

app.MapGet("/api/produtos", ([FromServices] bd_buygeContext _db) =>
{
    return Results.Ok(_db.TbProduto.ToList<TbProduto>());
});

app.Run();
