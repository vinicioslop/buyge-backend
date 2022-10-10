using buyge_backend.db;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSwaggerGen();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddDbContext<bdbuygeContext>(opt =>
{
    string connectionString = builder.Configuration.GetConnectionString("buygeConnection");
    var serverVersion = ServerVersion.AutoDetect(connectionString);
    opt.UseMySql(connectionString, serverVersion);
});

var app = builder.Build();

app.UseHttpsRedirection();
app.UseSwagger();
app.UseSwaggerUI();

// COMEÇO CLIENTES
app.MapGet("/api/clientes", ([FromServices] bdbuygeContext _db) =>
{
    var query = _db.TbCliente.AsQueryable<TbCliente>();
    var clientes = query.ToList<TbCliente>();
    return Results.Ok(clientes);
});

app.MapPost("/api/clientes", ([FromServices] bdbuygeContext _db,
    [FromBody] TbCliente novoCliente
) =>
{
    if (String.IsNullOrEmpty(novoCliente.NmCliente))
    {
        return Results.BadRequest(new { mensagem = "Não é possivel incluir um cliente sem nome." });
    }

    var cliente = new TbCliente
    {
        NmCliente = novoCliente.NmCliente,
        NmSobrenome = novoCliente.NmSobrenome,
        DtNascimento = novoCliente.DtNascimento,
        NrTelefone = novoCliente.NrTelefone,
        NmLogin = novoCliente.NmLogin,
        NmSenha = novoCliente.NmSenha
    };

    _db.TbCliente.Add(cliente);
    _db.SaveChanges();

    var clientesUrl = $"/api/clientes/{cliente.CdCliente}";

    return Results.Created(clientesUrl, cliente);
});

app.MapPut("/api/clientes/{id}", ([FromServices] bdbuygeContext _db,
    [FromRoute] int id,
    [FromBody] TbCliente clienteAlterado
) =>
{
    if (clienteAlterado.CdCliente != id)
    {
        return Results.BadRequest(new { mensagem = "Id inconsistente." });
    }

    if (String.IsNullOrEmpty(clienteAlterado.NmCliente))
    {
        return Results.BadRequest(new { mensagem = "Não é permitido deixar um cliente sem nome." });
    }

    var cliente = _db.TbCliente.Find(id);

    if (cliente == null)
    {
        return Results.NotFound();
    }

    cliente.NmCliente = clienteAlterado.NmCliente;
    cliente.NmSobrenome = clienteAlterado.NmSobrenome;
    cliente.DtNascimento = clienteAlterado.DtNascimento;
    cliente.NrTelefone = clienteAlterado.NrTelefone;
    cliente.NmLogin = clienteAlterado.NmLogin;
    cliente.NmSenha = clienteAlterado.NmSenha;

    _db.SaveChanges();

    return Results.Ok(cliente);
});

app.MapDelete("/api/clientes/{id}", ([FromServices] bdbuygeContext _db,
    [FromRoute] int id
) =>
{
    var cliente = _db.TbCliente.Find(id);

    if (cliente == null)
    {
        return Results.NotFound();
    }

    _db.TbCliente.Remove(cliente);
    _db.SaveChanges();

    return Results.Ok();
});
// FINAL CLIENTES

// COMEÇO CATEGORIAS
app.MapGet("/api/categorias", ([FromServices] bdbuygeContext _db) =>
{
    var query = _db.TbCategoria.AsQueryable<TbCategoria>();
    var categorias = query.ToList<TbCategoria>();
    return Results.Ok(categorias);
});

app.MapPost("/api/categorias", ([FromServices] bdbuygeContext _db,
    [FromBody] TbCategoria novaCategoria
) =>
{
    if (String.IsNullOrEmpty(novaCategoria.DsCategoria))
    {
        return Results.BadRequest(new { mensagem = "Não é possivel incluir uma categoria sem descrição." });
    }

    var categoria = new TbCategoria
    {
        NmCategoria = novaCategoria.NmCategoria,
        DsCategoria = novaCategoria.DsCategoria,
    };

    _db.TbCategoria.Add(categoria);
    _db.SaveChanges();

    var categoriasURL = $"/api/categorias/{categoria.CdCategoria}";

    return Results.Created(categoriasURL, categoria);
});

app.MapPut("/api/categorias/{id}", ([FromServices] bdbuygeContext _db,
    [FromRoute] int id,
    [FromBody] TbCategoria categoriaAlterada
) =>
{
    if (categoriaAlterada.CdCategoria != id)
    {
        return Results.BadRequest(new { mensagem = "Id inconsistente." });
    }

    if (String.IsNullOrEmpty(categoriaAlterada.DsCategoria))
    {
        return Results.BadRequest(new { mensagem = "Não é permitido deixar uma tarefa sem título." });
    }

    var categoria = _db.TbCategoria.Find(id);

    if (categoria == null)
    {
        return Results.NotFound();
    }

    categoria.NmCategoria = categoriaAlterada.NmCategoria;
    categoria.DsCategoria = categoriaAlterada.DsCategoria;

    _db.SaveChanges();

    return Results.Ok(categoria);
});

app.MapDelete("/api/categorias/{id}", ([FromServices] bdbuygeContext _db,
    [FromRoute] int id
) =>
{
    var categoria = _db.TbCategoria.Find(id);

    if (categoria == null)
    {
        return Results.NotFound();
    }

    _db.TbCategoria.Remove(categoria);
    _db.SaveChanges();

    return Results.Ok();
});
// FINAL CATEGORIAS

// COMEÇO PRODUTOS
app.MapGet("/api/produtos", ([FromServices] bdbuygeContext _db) =>
{
    return Results.Ok(_db.TbProduto.ToList<TbProduto>());
});
// FINAL PRODUTOS

app.Run();
