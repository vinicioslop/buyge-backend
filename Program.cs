using buyge_backend.db;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSwaggerGen();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCors();

builder.Services.AddDbContext<bdbuygeContext>(opt =>
{
    string connectionString = builder.Configuration.GetConnectionString("bdBuygeConnection");
    var serverVersion = ServerVersion.AutoDetect(connectionString);
    opt.UseMySql(connectionString, serverVersion);
});

var app = builder.Build();

app.UseCors(x => x
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(origin => true)
);

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

app.MapGet("/api/clientes/{id}", ([FromServices] bdbuygeContext _db, [FromRoute] int id) =>
{
    var cliente = _db.TbCliente.Find(id);

    if (cliente == null)
    {
        return Results.NotFound();
    }

    return Results.Ok(cliente);
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

// COMEÇO ENDEREÇOS
app.MapGet("/api/enderecos/{id}", ([FromServices] bdbuygeContext _db, [FromRoute] int id
) =>
{
    var query = _db.TbEndereco.AsQueryable<TbEndereco>();
    var enderecos = query.ToList<TbEndereco>().Where(e => e.FkCdCliente == id);

    if (enderecos == null)
    {
        return Results.NotFound();
    }

    return Results.Ok(enderecos);
});

app.MapPost("/api/enderecos", ([FromServices] bdbuygeContext _db,
    [FromBody] TbEndereco novoEndereco
) =>
{
    if (String.IsNullOrEmpty(novoEndereco.NmLogradouro))
    {
        return Results.BadRequest(new { mensagem = "Não é possivel incluir um endereço sem logradouro." });
    }

    var endereco = new TbEndereco
    {
        CdEndereco = 0,
        NmLogradouro = novoEndereco.NmLogradouro,
        NrEndereco = novoEndereco.NrEndereco,
        NmBairro = novoEndereco.NmBairro,
        NrCep = novoEndereco.NrCep,
        NmCidade = novoEndereco.NmCidade,
        SgEstado = novoEndereco.SgEstado,
        FkCdCliente = novoEndereco.FkCdCliente
    };

    _db.TbEndereco.Add(endereco);
    _db.SaveChanges();

    var enderecoUrl = $"/api/endereco/{endereco.CdEndereco}";

    return Results.Created(enderecoUrl, endereco);
});

app.MapPut("/api/enderecos/{id}", ([FromServices] bdbuygeContext _db,
    [FromRoute] int id,
    [FromBody] TbEndereco enderecoAlterado
) =>
{
    if (enderecoAlterado.CdEndereco != id)
    {
        return Results.BadRequest(new { mensagem = "Id inconsistente." });
    }

    if (String.IsNullOrEmpty(enderecoAlterado.NmLogradouro))
    {
        return Results.BadRequest(new { mensagem = "Não é permitido deixar endereço sem logradouro." });
    }

    var endereco = _db.TbEndereco.Find(id);

    if (endereco == null)
    {
        return Results.NotFound();
    }

    endereco.NmLogradouro = enderecoAlterado.NmLogradouro;
    endereco.NrEndereco = enderecoAlterado.NrEndereco;
    endereco.NmBairro = enderecoAlterado.NmBairro;
    endereco.NrCep = enderecoAlterado.NrCep;
    endereco.NmCidade = enderecoAlterado.NmCidade;
    endereco.SgEstado = enderecoAlterado.SgEstado;
    endereco.FkCdCliente = enderecoAlterado.FkCdCliente;

    _db.SaveChanges();

    return Results.Ok(endereco);
});

app.MapDelete("/api/enderecos/{id}", ([FromServices] bdbuygeContext _db,
    [FromRoute] int id
) =>
{
    var endereco = _db.TbEndereco.Find(id);

    if (endereco == null)
    {
        return Results.NotFound();
    }

    _db.TbEndereco.Remove(endereco);
    _db.SaveChanges();

    return Results.Ok();
});
// FINAL ENDEREÇOS

// COMEÇO CATEGORIAS
app.MapGet("/api/categorias", ([FromServices] bdbuygeContext _db) =>
{
    var query = _db.TbCategoria.AsQueryable<TbCategoria>();
    var categorias = query.ToList<TbCategoria>();
    return Results.Ok(categorias);
});

app.MapGet("/api/categorias/{id}", ([FromServices] bdbuygeContext _db, [FromRoute] int id) =>
{
    var categoria = _db.TbCategoria.Find(id);

    if (categoria == null)
    {
        return Results.NotFound();
    }

    return Results.Ok(categoria);
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

// COMEÇO MERCANTES
app.MapGet("/api/mercantes", ([FromServices] bdbuygeContext _db) =>
{
    var query = _db.TbMercante.AsQueryable<TbMercante>();
    var mercantes = query.ToList<TbMercante>();
    return Results.Ok(mercantes);
});

app.MapGet("/api/mercantes/{id}", ([FromServices] bdbuygeContext _db, [FromRoute] int id) =>
{
    var mercante = _db.TbMercante.Find(id);

    if (mercante == null)
    {
        return Results.NotFound();
    }

    return Results.Ok(mercante);
});

app.MapPost("/api/mercantes", ([FromServices] bdbuygeContext _db,
    [FromBody] TbMercante novoMercante
) =>
{
    if (String.IsNullOrEmpty(novoMercante.NmLoja))
    {
        return Results.BadRequest(new { mensagem = "Não é possivel incluir uma loja sem nome." });
    }

    var mercante = new TbMercante
    {
        NmLoja = novoMercante.NmLoja,
        DsLoja = novoMercante.DsLoja,
        ImgLogo = novoMercante.ImgLogo,
        FkCdCliente = novoMercante.FkCdCliente
    };

    _db.TbMercante.Add(mercante);
    _db.SaveChanges();

    var mercanteUrl = $"/api/mercantes/{mercante.CdMercante}";

    return Results.Created(mercanteUrl, mercante);
});

app.MapPut("/api/mercantes/{id}", ([FromServices] bdbuygeContext _db,
    [FromRoute] int id,
    [FromBody] TbMercante mercanteAlterado
) =>
{
    if (mercanteAlterado.CdMercante != id)
    {
        return Results.BadRequest(new { mensagem = "Id inconsistente." });
    }

    if (String.IsNullOrEmpty(mercanteAlterado.NmLoja))
    {
        return Results.BadRequest(new { mensagem = "Não é permitido deixar uma loja sem nome." });
    }

    var mercante = _db.TbMercante.Find(id);

    if (mercante == null)
    {
        return Results.NotFound();
    }

    mercante.NmLoja = mercanteAlterado.NmLoja;
    mercante.DsLoja = mercanteAlterado.DsLoja;
    mercante.ImgLogo = mercanteAlterado.ImgLogo;
    mercante.FkCdCliente = mercanteAlterado.FkCdCliente;

    _db.SaveChanges();

    return Results.Ok(mercante);
});

app.MapDelete("/api/mercantes/{id}", ([FromServices] bdbuygeContext _db,
    [FromRoute] int id
) =>
{
    var mercante = _db.TbMercante.Find(id);

    if (mercante == null)
    {
        return Results.NotFound();
    }

    _db.TbMercante.Remove(mercante);
    _db.SaveChanges();

    return Results.Ok();
});
// FINAL MERCANTES

// COMEÇO PRODUTOS
app.MapGet("/api/produtos", ([FromServices] bdbuygeContext _db) =>
{
    var query = _db.TbProduto.AsQueryable<TbProduto>();
    var produtos = query.ToList<TbProduto>();
    return Results.Ok(produtos);
});

app.MapGet("/api/produtos/{id}", ([FromServices] bdbuygeContext _db, [FromRoute] int id) =>
{
    var produto = _db.TbProduto.Find(id);

    if (produto == null)
    {
        return Results.NotFound();
    }

    return Results.Ok(produto);
});

app.MapPost("/api/produtos", ([FromServices] bdbuygeContext _db,
    [FromBody] TbProduto novoProduto
) =>
{
    if (String.IsNullOrEmpty(novoProduto.NmProduto))
    {
        return Results.BadRequest(new { mensagem = "Não é possivel incluir um produto sem nome." });
    }

    var produto = new TbProduto
    {
        NmProduto = novoProduto.NmProduto,
        DsProduto = novoProduto.DsProduto,
        VlProduto = novoProduto.VlProduto,
        QtProduto = novoProduto.QtProduto,
        FkCdMercante = novoProduto.FkCdMercante,
        FkCdCategoria = novoProduto.FkCdCategoria
    };

    _db.TbProduto.Add(produto);

    _db.SaveChanges();

    var produtoUrl = $"/api/produtos/{produto.CdProduto}";

    return Results.Created(produtoUrl, produto);
});

app.MapMethods("/api/produtos/{id}", new[] { "PATCH" }, ([FromServices] bdbuygeContext _db,
    [FromRoute] int id,
    [FromBody] TbProduto produtoAlterado
) =>
{
    if (produtoAlterado.CdProduto != id)
    {
        return Results.BadRequest(new { mensagem = "Id inconsistente." });
    }

    var produto = _db.TbProduto.Find(id);

    if (produto == null)
    {
        return Results.NotFound();
    }

    if (!String.IsNullOrEmpty(produtoAlterado.NmProduto)) produto.NmProduto = produtoAlterado.NmProduto;
    if (!String.IsNullOrEmpty(produtoAlterado.DsProduto)) produto.DsProduto = produtoAlterado.DsProduto;
    if (produtoAlterado.VlProduto > 0) produto.VlProduto = produtoAlterado.VlProduto;
    if (produtoAlterado.QtProduto > 0) produto.QtProduto = produtoAlterado.QtProduto;
    if (produtoAlterado.FkCdCategoria > 0) produto.FkCdCategoria = produtoAlterado.FkCdCategoria;

    _db.SaveChanges();

    return Results.Ok(produto);
});

app.MapDelete("/api/produtos/{id}", ([FromServices] bdbuygeContext _db,
    [FromRoute] int id
) =>
{
    var produto = _db.TbProduto.Find(id);

    if (produto == null)
    {
        return Results.NotFound();
    }

    _db.TbProduto.Remove(produto);
    _db.SaveChanges();

    return Results.Ok();
});
// FINAL PRODUTOS

// COMEÇO IMAGENS PRODUTOS
app.MapGet("/api/produtos/produto-imagens", ([FromServices] bdbuygeContext _db) =>
{
    var query = _db.TbProdutoImagem.AsQueryable<TbProdutoImagem>();
    var produtosImagens = query.ToList<TbProdutoImagem>();
    return Results.Ok(produtosImagens);
});

app.MapGet("/api/produtos/produto-imagem/{id}", ([FromServices] bdbuygeContext _db, [FromRoute] int id) =>
{
    var query = _db.TbProdutoImagem.AsQueryable<TbProdutoImagem>();
    var produtoImagens = query.ToList<TbProdutoImagem>().Where(pi => pi.FkCdProduto == id);

    if (produtoImagens == null)
    {
        return Results.NotFound();
    }

    return Results.Ok(produtoImagens);
});

app.MapPost("/api/produtos/produto-imagens", ([FromServices] bdbuygeContext _db,
    [FromBody] TbProdutoImagem novaImagem
) =>
{
    if (String.IsNullOrEmpty(novaImagem.ImgProduto))
    {
        return Results.BadRequest(new { mensagem = "Não é possivel incluir uma imagem sem endereço." });
    }

    var produtoImagem = new TbProdutoImagem
    {
        ImgProduto = novaImagem.ImgProduto,
        DsImagemProduto = novaImagem.DsImagemProduto,
        FkCdProduto = novaImagem.FkCdProduto
    };

    _db.TbProdutoImagem.Add(produtoImagem);
    _db.SaveChanges();

    var produtoImagemUrl = $"/api/produtos/produto-imagens/{produtoImagem.CdProdutoImagem}";

    return Results.Created(produtoImagemUrl, produtoImagem);
});

app.MapMethods("/api/produtos/produto-imagem/{id}", new[] { "PATCH" }, ([FromServices] bdbuygeContext _db,
    [FromRoute] int id,
    [FromBody] TbProdutoImagem produtoImagemAlterado
) =>
{
    if (produtoImagemAlterado.CdProdutoImagem != id)
    {
        return Results.BadRequest(new { mensagem = "Id inconsistente." });
    }

    var produtoImagem = _db.TbProdutoImagem.Find(id);

    if (produtoImagem == null)
    {
        return Results.NotFound();
    }

    if (!String.IsNullOrEmpty(produtoImagemAlterado.ImgProduto)) produtoImagem.ImgProduto = produtoImagemAlterado.ImgProduto;
    if (!String.IsNullOrEmpty(produtoImagemAlterado.DsImagemProduto)) produtoImagem.DsImagemProduto = produtoImagemAlterado.DsImagemProduto;

    _db.SaveChanges();

    return Results.Ok(produtoImagem);
});

app.MapDelete("/api/produtos/produto-imagem/{id}", ([FromServices] bdbuygeContext _db,
    [FromRoute] int id
) =>
{
    var produtoImagem = _db.TbProdutoImagem.Find(id);

    if (produtoImagem == null)
    {
        return Results.NotFound();
    }

    _db.TbProdutoImagem.Remove(produtoImagem);
    _db.SaveChanges();

    return Results.Ok();
});
// FINAL IMAGENS PRODUTOS

// COMEÇO CARRINHO
app.MapGet("/api/carrinho/{idCliente}", ([FromServices] bdbuygeContext _db, [FromRoute] int idCliente
) =>
{
    var query = _db.TbCarrinho.AsQueryable<TbCarrinho>();
    var carrinho = query.ToList<TbCarrinho>().Where(c => c.FkCdCliente == idCliente);

    if (carrinho == null)
    {
        return Results.NotFound();
    }

    return Results.Ok(carrinho);
});

app.MapPost("/api/carrinho/{idCliente}", ([FromServices] bdbuygeContext _db,
[FromRoute] int idCliente
) =>
{
    var query = _db.TbCarrinho.AsQueryable<TbCarrinho>();
    var carrinho = query.ToList<TbCarrinho>().Where(c => c.FkCdCliente == idCliente);

    if (carrinho.ToArray().Length >= 1)
    {
        return Results.BadRequest(new { mensagem = "Não é permitido adicionar mais de um carrinho por cliente." });
    }

    var novoCarrinho = new TbCarrinho
    {
        FkCdCliente = idCliente
    };

    _db.TbCarrinho.Add(novoCarrinho);
    _db.SaveChanges();

    var carrinhoUrl = $"/api/carrinho/{novoCarrinho.CdCarrinho}";

    return Results.Created(carrinhoUrl, novoCarrinho);
});
// FINAL CARRINHO

// COMEÇO ITEM CARRINHO
app.MapGet("/api/carrinho/items/{idCarrinho}", ([FromServices] bdbuygeContext _db, [FromRoute] int idCarrinho) =>
{
    var query = _db.TbItemCarrinho.AsQueryable<TbItemCarrinho>();
    var items = query.ToList<TbItemCarrinho>().Where(i => i.FkCdCarrinho == idCarrinho);
    return Results.Ok(items);
});

app.MapPost("/api/carrinho/items/{idCarrinho}/{idProduto}", ([FromServices] bdbuygeContext _db,
    [FromRoute] int idCarrinho, [FromRoute] int idProduto
) =>
{
    var itemCarrinho = new TbItemCarrinho
    {
        FkCdProduto = idProduto,
        FkCdCarrinho = idCarrinho
    };

    _db.TbItemCarrinho.Add(itemCarrinho);
    _db.SaveChanges();

    var itemCarrinhoUrl = $"/api/carrinho/items/{itemCarrinho.FkCdCarrinho}/{itemCarrinho.FkCdProduto}";

    return Results.Created(itemCarrinhoUrl, itemCarrinho);
});

app.MapDelete("/api/carrinho/items/{idCarrinho}/{idItemCarrinho}", ([FromServices] bdbuygeContext _db,
    [FromRoute] int idCarrinho, [FromRoute] int idItemCarrinho
) =>
{
    var itemCarrinho = _db.TbItemCarrinho.Find(idItemCarrinho);

    if (itemCarrinho == null)
    {
        return Results.NotFound();
    }

    _db.TbItemCarrinho.Remove(itemCarrinho);
    _db.SaveChanges();

    return Results.Ok();
});
// FINAL ITEM CARRINHO

// COMEÇO FAVORITO
app.MapGet("/api/favoritos/{idCliente}", ([FromServices] bdbuygeContext _db, [FromRoute] int idCliente
) =>
{
    var query = _db.TbFavorito.AsQueryable<TbFavorito>();
    var favoritos = query.ToList<TbFavorito>().Where(f => f.FkCdCliente == idCliente);

    if (favoritos == null)
    {
        return Results.NotFound();
    }

    return Results.Ok(favoritos);
});

app.MapPost("/api/favoritos/{idCliente}", ([FromServices] bdbuygeContext _db,
[FromRoute] int idCliente
) =>
{
    var query = _db.TbFavorito.AsQueryable<TbFavorito>();
    var favorito = query.ToList<TbFavorito>().Where(c => c.FkCdCliente == idCliente);

    if (favorito.ToArray().Length >= 1)
    {
        return Results.BadRequest(new { mensagem = "Não é permitido adicionar mais de uma lista de favoritos por cliente." });
    }

    var novoFavorito = new TbFavorito
    {
        FkCdCliente = idCliente
    };

    _db.TbFavorito.Add(novoFavorito);
    _db.SaveChanges();

    var favoritoUrl = $"/api/favoritos/{novoFavorito.FkCdCliente}/{novoFavorito.CdFavorito}";

    return Results.Created(favoritoUrl, novoFavorito);
});
// FINAL FAVORITO

// COMEÇO ITEM FAVORITO
app.MapGet("/api/favorito/items/{idFavorito}", ([FromServices] bdbuygeContext _db, [FromRoute] int idFavorito) =>
{
    var query = _db.TbItemFavorito.AsQueryable<TbItemFavorito>();
    var items = query.ToList<TbItemFavorito>().Where(i => i.FkCdFavorito == idFavorito);
    return Results.Ok(items);
});

app.MapPost("/api/favorito/items/{idFavorito}/{idProduto}", ([FromServices] bdbuygeContext _db,
    [FromRoute] int idFavorito, [FromRoute] int idProduto
) =>
{
    var itemFavorito = new TbItemFavorito
    {
        FkCdFavorito = idFavorito,
        FkCdProduto = idProduto
    };

    _db.TbItemFavorito.Add(itemFavorito);
    _db.SaveChanges();

    var itemFavoritoUrl = $"/api/favorito/items/{itemFavorito.FkCdFavorito}/{itemFavorito.FkCdProduto}";

    return Results.Created(itemFavoritoUrl, itemFavorito);
});

app.MapDelete("/api/favorito/items/{idItemFavorito}", ([FromServices] bdbuygeContext _db,
    [FromRoute] int idItemFavorito
) =>
{
    var itemFavorito = _db.TbItemFavorito.Find(idItemFavorito);

    if (itemFavorito == null)
    {
        return Results.NotFound();
    }

    _db.TbItemFavorito.Remove(itemFavorito);
    _db.SaveChanges();

    return Results.Ok();
});
// FINAL ITEM FAVORITO

app.Run();
