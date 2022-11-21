using System.Text;
using buyge_backend;
using buyge_backend.db;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

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

var key = Encoding.ASCII.GetBytes(Settings.Secret);
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Padrão", policy => policy.RequireRole("Padrão"));
    options.AddPolicy("Lojista", policy => policy.RequireRole("Lojista"));
});

var app = builder.Build();

app.UseHttpsRedirection();
app.UseSwagger();
app.UseSwaggerUI();

app.UseCors(x => x
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(origin => true)
);

app.UseAuthentication();
app.UseAuthorization();

app.MapPost("/api/login", ([FromServices] bdbuygeContext _db, [FromBody] User user) =>
{
    var query = _db.TbCliente.AsQueryable<TbCliente>();
    var clientes = query.ToList<TbCliente>().Where(c => c.NmEmail == user.login && c.NmSenha == user.senha);

    var cliente = clientes.FirstOrDefault();

    if (cliente == null)
    {
        return Results.NotFound();
    }

    var token = TokenService.GenerateToken(cliente);

    cliente.NmSenha = "";

    return Results.Ok(
        new
        {
            cliente = cliente,
            token = token
        }
    );
}).AllowAnonymous();

app.MapGet("/api/token", ([FromServices] bdbuygeContext _db) =>
{
    return Results.Ok();
}).RequireAuthorization();

// COMEÇO CLIENTES
app.MapGet("/api/clientes", ([FromServices] bdbuygeContext _db) =>
{
    var query = _db.TbCliente.AsQueryable<TbCliente>();
    var clientes = query.ToList<TbCliente>();
    return Results.Ok(
        new
        {
            clientes = clientes
        }
    );
}).RequireAuthorization("Lojista");

app.MapGet("/api/clientes/{id}", ([FromServices] bdbuygeContext _db, [FromRoute] int id) =>
{
    var cliente = _db.TbCliente.Find(id);

    if (cliente == null)
    {
        return Results.NotFound();
    }

    return Results.Ok(cliente);
}).RequireAuthorization();

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
        DtNascimento = novoCliente.DtNascimento,
        NmEmail = novoCliente.NmEmail,
        NmSenha = novoCliente.NmSenha,
        NmTipoConta = novoCliente.NmTipoConta
    };

    _db.TbCliente.Add(cliente);

    _db.SaveChanges();

    var clientesUrl = $"/api/clientes/{cliente.CdCliente}";

    return Results.Created(clientesUrl, cliente);
}).AllowAnonymous();

app.MapMethods("/api/clientes/{id}", new[] { "PATCH" }, ([FromServices] bdbuygeContext _db,
    [FromRoute] int id,
    [FromBody] TbCliente clienteAlterado
) =>
{
    if (clienteAlterado.CdCliente != id)
    {
        return Results.BadRequest(new { mensagem = "Id inconsistente." });
    }

    var cliente = _db.TbCliente.Find(id);

    if (cliente == null)
    {
        return Results.NotFound();
    }

    if (!String.IsNullOrEmpty(clienteAlterado.NmCliente)) cliente.NmCliente = clienteAlterado.NmCliente;
    if (!String.IsNullOrEmpty(clienteAlterado.NmSobrenome)) cliente.NmSobrenome = clienteAlterado.NmSobrenome;
    if (!String.IsNullOrEmpty(clienteAlterado.NrCpf)) cliente.NrCpf = clienteAlterado.NrCpf;
    if (clienteAlterado.DtNascimento != null) cliente.DtNascimento = clienteAlterado.DtNascimento;
    if (!String.IsNullOrEmpty(clienteAlterado.NrTelefone)) cliente.NrTelefone = clienteAlterado.NrTelefone;
    if (!String.IsNullOrEmpty(clienteAlterado.NmEmail)) cliente.NmEmail = clienteAlterado.NmEmail;

    _db.SaveChanges();

    return Results.Ok();
}).RequireAuthorization();

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
}).RequireAuthorization();
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
}).RequireAuthorization();

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
}).RequireAuthorization();

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
}).RequireAuthorization();

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
}).RequireAuthorization();
// FINAL ENDEREÇOS

// COMEÇO CATEGORIAS
app.MapGet("/api/categorias", ([FromServices] bdbuygeContext _db) =>
{
    var query = _db.TbCategoria.AsQueryable<TbCategoria>();
    var categorias = query.ToList<TbCategoria>();
    return Results.Ok(categorias);
}).AllowAnonymous();

app.MapGet("/api/categorias/{id}", ([FromServices] bdbuygeContext _db, [FromRoute] int id) =>
{
    var categoria = _db.TbCategoria.Find(id);

    if (categoria == null)
    {
        return Results.NotFound();
    }

    return Results.Ok(categoria);
}).AllowAnonymous();

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
}).RequireAuthorization();

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
}).RequireAuthorization();

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
}).RequireAuthorization();
// FINAL CATEGORIAS

// COMEÇO MERCANTES
app.MapGet("/api/mercantes", ([FromServices] bdbuygeContext _db) =>
{
    var query = _db.TbMercante.AsQueryable<TbMercante>();
    var mercantes = query.ToList<TbMercante>();
    return Results.Ok(mercantes);
}).AllowAnonymous();

app.MapGet("/api/mercantes/{id}", ([FromServices] bdbuygeContext _db, [FromRoute] int id) =>
{
    var mercante = _db.TbMercante.Find(id);

    if (mercante == null)
    {
        return Results.NotFound();
    }

    return Results.Ok(mercante);
}).AllowAnonymous();

app.MapGet("/api/mercantes/lojista/{id}", ([FromServices] bdbuygeContext _db, [FromRoute] int id
) =>
{
    var query = _db.TbMercante.AsQueryable<TbMercante>();
    var mercantes = query.ToList<TbMercante>().Where(m => m.FkCdCliente == id);

    if (mercantes == null)
    {
        return Results.NotFound();
    }

    return Results.Ok(mercantes);
}).RequireAuthorization("Lojista");

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
        ImgLogoLink = novoMercante.ImgLogoLink,
        ImgLogo = null,
        NrCnpj = novoMercante.NrCnpj,
        FkCdCliente = novoMercante.FkCdCliente
    };

    _db.TbMercante.Add(mercante);
    _db.SaveChanges();

    var mercanteUrl = $"/api/mercantes/{mercante.CdMercante}";

    return Results.Created(mercanteUrl, mercante);
}).RequireAuthorization("Lojista");

app.MapMethods("/api/mercantes/{idMercante}", new[] { "PATCH" }, ([FromServices] bdbuygeContext _db,
    [FromRoute] int idMercante,
    [FromBody] TbMercante mercanteAlterado
) =>
{
    if (mercanteAlterado.CdMercante != idMercante)
    {
        return Results.BadRequest(new { mensagem = "Id inconsistente." });
    }

    var mercante = _db.TbMercante.Find(idMercante);

    if (mercante == null)
    {
        return Results.NotFound();
    }

    if (mercante.FkCdCliente != mercanteAlterado.FkCdCliente)
    {
        return Results.BadRequest(new { mensagem = "Cliente não possui permissão de alterar loja." });
    }

    if (!String.IsNullOrEmpty(mercanteAlterado.NmLoja)) mercante.NmLoja = mercanteAlterado.NmLoja;
    if (!String.IsNullOrEmpty(mercanteAlterado.DsLoja)) mercante.DsLoja = mercanteAlterado.DsLoja;
    if (!String.IsNullOrEmpty(mercanteAlterado.ImgLogoLink)) mercante.ImgLogoLink = mercanteAlterado.ImgLogoLink;
    if (!String.IsNullOrEmpty(mercanteAlterado.NrCnpj)) mercante.NrCnpj = mercanteAlterado.NrCnpj;

    _db.SaveChanges();

    return Results.Ok(mercante);
}).RequireAuthorization("Lojista");

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
}).RequireAuthorization("Lojista");
// FINAL MERCANTES

// COMEÇO PRODUTOS
app.MapGet("/api/produtos", ([FromServices] bdbuygeContext _db) =>
{
    var query = _db.TbProduto.AsQueryable<TbProduto>();
    var produtos = query.ToList<TbProduto>();
    return Results.Ok(produtos);
}).AllowAnonymous();

app.MapGet("/api/produtos/{id}", ([FromServices] bdbuygeContext _db, [FromRoute] int id) =>
{
    var produto = _db.TbProduto.Find(id);

    if (produto == null)
    {
        return Results.NotFound();
    }

    return Results.Ok(produto);
}).AllowAnonymous();

app.MapGet("/api/produtos/mercante/{id}", ([FromServices] bdbuygeContext _db, [FromRoute] int id) =>
{
    var query = _db.TbProduto.AsQueryable<TbProduto>();
    var produtos = query.ToList<TbProduto>().Where(p => p.FkCdMercante == id);

    if (produtos == null)
    {
        return Results.NotFound();
    }

    return Results.Ok(produtos);
}).AllowAnonymous();

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
}).RequireAuthorization("Lojista");

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

    return Results.Ok();
}).RequireAuthorization("Lojista");

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
}).RequireAuthorization("Lojista");
// FINAL PRODUTOS

// COMEÇO IMAGENS PRODUTOS
app.MapGet("/api/produtos/produto-imagem", ([FromServices] bdbuygeContext _db) =>
{
    var query = _db.TbProdutoImagem.AsQueryable<TbProdutoImagem>();
    var produtosImagens = query.ToList<TbProdutoImagem>();
    return Results.Ok(produtosImagens);
}).AllowAnonymous();

app.MapGet("/api/produtos/produto-imagem/{id}", ([FromServices] bdbuygeContext _db, [FromRoute] int id) =>
{
    var imagem = _db.TbProdutoImagem.Find(id);

    if (imagem == null)
    {
        return Results.NotFound();
    }

    return Results.Ok(imagem);
}).AllowAnonymous();

app.MapGet("/api/produtos/produto-imagem/{id}/todas", ([FromServices] bdbuygeContext _db, [FromRoute] int id) =>
{
    var query = _db.TbProdutoImagem.AsQueryable<TbProdutoImagem>();
    var produtoImagens = query.ToList<TbProdutoImagem>().Where(pi => pi.FkCdProduto == id);

    if (produtoImagens == null)
    {
        return Results.NotFound();
    }

    return Results.Ok(produtoImagens);
}).AllowAnonymous();

app.MapPost("/api/produtos/produto-imagem", ([FromServices] bdbuygeContext _db,
    [FromBody] TbProdutoImagem novaImagem
) =>
{
    Boolean valido = false;

    var produtoImagem = new TbProdutoImagem
    {
        ImgProdutoLink = null,
        ImgProduto = null,
        DsImagemProduto = novaImagem.DsImagemProduto,
        IdPrincipal = novaImagem.IdPrincipal,
        FkCdProduto = novaImagem.FkCdProduto
    };

    if (novaImagem.ImgProduto != null)
    {
        produtoImagem.ImgProduto = novaImagem.ImgProduto;
        valido = true;
    }

    if (!String.IsNullOrEmpty(novaImagem.ImgProdutoLink))
    {
        produtoImagem.ImgProdutoLink = novaImagem.ImgProdutoLink;
        valido = true;
    }

    if (!valido)
    {
        return Results.BadRequest(new { mensagem = "Não é possível inserir uma imagem sem pelo menos 1 fonte;" });
    }

    _db.TbProdutoImagem.Add(produtoImagem);
    _db.SaveChanges();

    var produtoImagemUrl = $"/api/produtos/produto-imagens/{produtoImagem.CdProdutoImagem}";

    return Results.Created(produtoImagemUrl, produtoImagem);
}).RequireAuthorization("Lojista");

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

    if (!String.IsNullOrEmpty(produtoImagemAlterado.ImgProdutoLink)) produtoImagem.ImgProdutoLink = produtoImagemAlterado.ImgProdutoLink;
    if (!String.IsNullOrEmpty(produtoImagemAlterado.DsImagemProduto)) produtoImagem.DsImagemProduto = produtoImagemAlterado.DsImagemProduto;

    _db.SaveChanges();

    return Results.Ok(produtoImagem);
}).RequireAuthorization("Lojista");

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
}).RequireAuthorization("Lojista");
// FINAL IMAGENS PRODUTOS

// COMEÇO ITEM CARRINHO
app.MapGet("/api/carrinho/items/{idCliente}", ([FromServices] bdbuygeContext _db, [FromRoute] int idCliente) =>
{
    var query = _db.TbItemCarrinho.AsQueryable<TbItemCarrinho>();
    var items = query.ToList<TbItemCarrinho>().Where(i => i.FkCdCliente == idCliente);
    return Results.Ok(items);
}).RequireAuthorization();

app.MapPost("/api/carrinho/items/{idCliente}/{idProduto}", ([FromServices] bdbuygeContext _db,
    [FromRoute] int idCliente, [FromRoute] int idProduto
) =>
{
    var itemCarrinho = new TbItemCarrinho
    {
        FkCdProduto = idProduto,
        FkCdCliente = idCliente
    };

    _db.TbItemCarrinho.Add(itemCarrinho);
    _db.SaveChanges();

    var itemCarrinhoUrl = $"/api/carrinho/items/{itemCarrinho.FkCdCliente}";

    return Results.Created(itemCarrinhoUrl, itemCarrinho);
}).RequireAuthorization();

app.MapDelete("/api/carrinho/items/{idItemCarrinho}", ([FromServices] bdbuygeContext _db,
    [FromRoute] int idItemCarrinho
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
}).RequireAuthorization();
// FINAL ITEM CARRINHO

// COMEÇO ITEM FAVORITO
app.MapGet("/api/favorito/items/{idCliente}", ([FromServices] bdbuygeContext _db, [FromRoute] int idCliente) =>
{
    var query = _db.TbItemFavorito.AsQueryable<TbItemFavorito>();
    var items = query.ToList<TbItemFavorito>().Where(i => i.FkCdCliente == idCliente);
    return Results.Ok(items);
}).RequireAuthorization();

app.MapPost("/api/favorito/items/{idCliente}/{idProduto}", ([FromServices] bdbuygeContext _db,
    [FromRoute] int idCliente, [FromRoute] int idProduto
) =>
{
    var itemFavorito = new TbItemFavorito
    {
        FkCdCliente = idCliente,
        FkCdProduto = idProduto
    };

    _db.TbItemFavorito.Add(itemFavorito);
    _db.SaveChanges();

    var itemFavoritoUrl = $"/api/favorito/items/{itemFavorito.FkCdCliente}";

    return Results.Created(itemFavoritoUrl, itemFavorito);
}).RequireAuthorization();

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
}).RequireAuthorization();
// FINAL ITEM FAVORITO
app.Run();
