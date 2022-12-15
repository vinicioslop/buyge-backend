using buyge_backend;
using buyge_backend.db;
using MercadoPago.Client.Preference;
using MercadoPago.Resource.Preference;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

using MercadoPago.Config;
using System.Text.Json.Serialization;

// ACESS TOKEN TESTE
MercadoPagoConfig.AccessToken = "TEST-2863067349326898-112719-b6619df8821b7a6437236c816ff370f5-265323495";

// ACESS TOKEN PRODUÇÃO
//MercadoPagoConfig.AccessToken = "APP_USR-2863067349326898-112719-4ccb4eabd31fd1564fc303658a0faf5e-265323495";

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
    options.AddPolicy("Vendedor", policy => policy.RequireRole("Vendedor"));
});

builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options =>
{
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors(x => x
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyMethod()
    .AllowAnyHeader()
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
app.MapGet("/api/cliente/{idCliente}", ([FromServices] bdbuygeContext _db, [FromRoute] int idCliente) =>
{
    var cliente = _db.TbCliente.Find(idCliente);

    if (cliente == null)
    {
        return Results.NotFound();
    }

    cliente.NmSenha = "";

    return Results.Ok(cliente);
}).RequireAuthorization();

app.MapPost("/api/cliente/cadastrar", ([FromServices] bdbuygeContext _db,
    [FromBody] TbCliente novoCliente
) =>
{
    if (String.IsNullOrEmpty(novoCliente.NmCliente)) return Results.BadRequest(new { mensagem = "Não é possivel incluir um cliente sem nome." });


    var cliente = new TbCliente
    {
        NmCliente = novoCliente.NmCliente,
        NmSobrenome = novoCliente.NmSobrenome,
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

app.MapMethods("/api/cliente/atualizar/{idCliente}", new[] { "PATCH" }, ([FromServices] bdbuygeContext _db,
    [FromRoute] int idCliente,
    [FromBody] TbCliente clienteAlterado
) =>
{
    if (clienteAlterado.CdCliente != idCliente)
    {
        return Results.BadRequest(new { mensagem = "Id inconsistente." });
    }

    var cliente = _db.TbCliente.Find(idCliente);

    if (cliente == null)
    {
        return Results.NotFound();
    }

    if (!String.IsNullOrEmpty(clienteAlterado.NmCliente)) cliente.NmCliente = clienteAlterado.NmCliente;
    if (!String.IsNullOrEmpty(clienteAlterado.NmSobrenome)) cliente.NmSobrenome = clienteAlterado.NmSobrenome;
    if (!String.IsNullOrEmpty(clienteAlterado.NrCpf)) cliente.NrCpf = clienteAlterado.NrCpf;
    cliente.DtNascimento = clienteAlterado.DtNascimento;
    if (!String.IsNullOrEmpty(clienteAlterado.NrTelefone)) cliente.NrTelefone = clienteAlterado.NrTelefone;
    if (!String.IsNullOrEmpty(clienteAlterado.NmEmail)) cliente.NmEmail = clienteAlterado.NmEmail;

    _db.SaveChanges();

    return Results.Ok();
}).RequireAuthorization();

app.MapMethods("/api/cliente/vendedor/{idCliente}", new[] { "PATCH" }, ([FromServices] bdbuygeContext _db,
    [FromRoute] int idCliente
) =>
{
    var cliente = _db.TbCliente.Find(idCliente);

    if (cliente == null)
    {
        return Results.NotFound();
    }

    if (cliente.NmTipoConta == "Vendedor")
    {
        return Results.BadRequest(new { mensagem = "Cliente já possui conta de vendedor" });
    }

    cliente.NmTipoConta = "Vendedor";

    _db.SaveChanges();

    return Results.Ok();
}).RequireAuthorization();

app.MapDelete("/api/cliente/remover/{idCliente}", ([FromServices] bdbuygeContext _db,
    [FromRoute] int idCliente
) =>
{
    var cliente = _db.TbCliente.Find(idCliente);

    if (cliente == null)
    {
        return Results.NotFound();
    }

    _db.TbCliente.Remove(cliente);
    _db.SaveChanges();

    return Results.Ok();
}).RequireAuthorization();
// FINAL CLIENTES

// COMEÇO SENHA
app.MapPost("/api/cliente/senha/trocar/{idCliente}", ([FromServices] bdbuygeContext _db,
    [FromRoute] int idCliente, [FromBody] TrocaSenha novaSenha
) =>
{
    if (String.IsNullOrEmpty(novaSenha.senhaAtual) || String.IsNullOrEmpty(novaSenha.novaSenha))
    {
        return Results.BadRequest(new { mensagem = "Não é possivel alterar a senha sem os valores." });
    }

    var cliente = _db.TbCliente.Find(idCliente);

    if (cliente == null)
    {
        return Results.NotFound();
    }

    if (cliente.NmSenha == novaSenha.senhaAtual)
    {
        cliente.NmSenha = novaSenha.novaSenha;
    }
    else
    {
        return Results.BadRequest(new { mensagem = "Senha informada é diferente da atual." });
    }

    _db.SaveChanges();

    cliente.NmSenha = "";

    var clientesUrl = $"/api/clientes/{cliente.CdCliente}";

    return Results.Created(clientesUrl, cliente);
}).RequireAuthorization();
// FINAL SENHA

// COMEÇO ENDEREÇOS
app.MapGet("/api/endereco/{idEndereco}", ([FromServices] bdbuygeContext _db, [FromRoute] int idEndereco) =>
{
    var endereco = _db.TbEndereco.Find(idEndereco);

    if (endereco == null)
    {
        return Results.NotFound();
    }

    return Results.Ok(endereco);
}).RequireAuthorization();

app.MapGet("/api/enderecos/cliente/{idCliente}", ([FromServices] bdbuygeContext _db, [FromRoute] int idCliente
) =>
{
    var query = _db.TbEndereco.AsQueryable<TbEndereco>();
    var enderecos = query.ToList<TbEndereco>().Where(e => e.FkCdCliente == idCliente);

    if (enderecos == null)
    {
        return Results.NotFound();
    }

    return Results.Ok(enderecos);
}).RequireAuthorization();

app.MapPost("/api/enderecos/adicionar", ([FromServices] bdbuygeContext _db,
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
        NmTituloEndereco = novoEndereco.NmTituloEndereco,
        NmTipoEndereco = novoEndereco.NmTipoEndereco,
        IdPrincipal = novoEndereco.IdPrincipal,
        FkCdCliente = novoEndereco.FkCdCliente
    };

    _db.TbEndereco.Add(endereco);
    _db.SaveChanges();

    var enderecoUrl = $"/api/endereco/{endereco.CdEndereco}";

    return Results.Created(enderecoUrl, endereco);
}).RequireAuthorization();

app.MapMethods("/api/endereco/atualizar/{idEndereco}", new[] { "PATCH" }, ([FromServices] bdbuygeContext _db,
    [FromRoute] int idEndereco,
    [FromBody] TbEndereco enderecoAlterado
) =>
{
    if (enderecoAlterado.CdEndereco != idEndereco)
    {
        return Results.BadRequest(new { mensagem = "Id inconsistente." });
    }

    var endereco = _db.TbEndereco.Find(idEndereco);

    if (endereco == null)
    {
        return Results.NotFound();
    }

    if (endereco.FkCdCliente != enderecoAlterado.FkCdCliente)
    {
        return Results.BadRequest(new { mensagem = "Id inconsistente." });
    }

    if (!String.IsNullOrEmpty(enderecoAlterado.NmLogradouro)) endereco.NmLogradouro = enderecoAlterado.NmLogradouro;
    if (enderecoAlterado.NrEndereco > 0) endereco.NrEndereco = enderecoAlterado.NrEndereco;
    if (!String.IsNullOrEmpty(enderecoAlterado.NmBairro)) endereco.NmBairro = enderecoAlterado.NmBairro;
    if (!String.IsNullOrEmpty(enderecoAlterado.NrCep)) endereco.NrCep = enderecoAlterado.NrCep;
    if (!String.IsNullOrEmpty(enderecoAlterado.NmCidade)) endereco.NmCidade = enderecoAlterado.NmCidade;
    if (!String.IsNullOrEmpty(enderecoAlterado.SgEstado)) endereco.SgEstado = enderecoAlterado.SgEstado;
    if (!String.IsNullOrEmpty(enderecoAlterado.NmTituloEndereco)) endereco.NmTituloEndereco = enderecoAlterado.NmTituloEndereco;
    if (!String.IsNullOrEmpty(enderecoAlterado.NmTipoEndereco)) endereco.NmTipoEndereco = enderecoAlterado.NmTipoEndereco;

    _db.SaveChanges();

    return Results.Ok(endereco);
}).RequireAuthorization();

app.MapMethods("/api/endereco/principal/{idEndereco}", new[] { "PATCH" }, ([FromServices] bdbuygeContext _db,
    [FromRoute] int idEndereco
) =>
{
    var endereco = _db.TbEndereco.Find(idEndereco);

    if (endereco == null)
    {
        return Results.NotFound();
    }

    endereco.IdPrincipal = 1;

    var query = _db.TbEndereco.AsQueryable<TbEndereco>();
    var enderecos = query.ToList<TbEndereco>().Where(e => e.FkCdCliente == endereco.FkCdCliente);

    var listaEnderecos = enderecos.ToList<TbEndereco>();

    listaEnderecos.ForEach((item) =>
    {
        if (item.CdEndereco != endereco.CdEndereco)
        {
            item.IdPrincipal = 0;
        }
    });

    _db.SaveChanges();

    return Results.Ok(endereco);
}).RequireAuthorization();

app.MapDelete("/api/endereco/remover/{idEndereco}", ([FromServices] bdbuygeContext _db,
    [FromRoute] int idEndereco
) =>
{
    var endereco = _db.TbEndereco.Find(idEndereco);

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

app.MapGet("/api/categoria/{idCategoria}", ([FromServices] bdbuygeContext _db, [FromRoute] int idCategoria) =>
{
    var categoria = _db.TbCategoria.Find(idCategoria);

    if (categoria == null)
    {
        return Results.NotFound();
    }

    return Results.Ok(categoria);
}).AllowAnonymous();
// FINAL CATEGORIAS

// COMEÇO MERCANTES
app.MapGet("/api/mercantes", ([FromServices] bdbuygeContext _db) =>
{
    var query = _db.TbMercante.AsQueryable<TbMercante>();
    var mercantes = query.ToList<TbMercante>();
    return Results.Ok(mercantes);
}).AllowAnonymous();

app.MapGet("/api/mercante/{idMercante}", ([FromServices] bdbuygeContext _db, [FromRoute] int idMercante) =>
{
    var mercante = _db.TbMercante.Find(idMercante);

    if (mercante == null)
    {
        return Results.NotFound();
    }

    return Results.Ok(mercante);
}).AllowAnonymous();

app.MapGet("/api/mercantes/vendedor/{idCliente}", ([FromServices] bdbuygeContext _db, [FromRoute] int idCliente
) =>
{
    var query = _db.TbMercante.AsQueryable<TbMercante>();
    var mercantes = query.ToList<TbMercante>().Where(m => m.FkCdCliente == idCliente);

    if (mercantes == null)
    {
        return Results.NotFound();
    }

    return Results.Ok(mercantes);
}).RequireAuthorization();

app.MapPost("/api/mercantes/cadastrar", ([FromServices] bdbuygeContext _db,
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
}).RequireAuthorization();

app.MapMethods("/api/mercante/atualizar/{idMercante}", new[] { "PATCH" }, ([FromServices] bdbuygeContext _db,
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
    if (!String.IsNullOrEmpty(mercanteAlterado.NmEmail)) mercante.NmEmail = mercanteAlterado.NmEmail;
    if (!String.IsNullOrEmpty(mercanteAlterado.ImgLogoLink)) mercante.ImgLogoLink = mercanteAlterado.ImgLogoLink;
    if (mercanteAlterado.ImgLogo == null) mercante.ImgLogo = mercanteAlterado.ImgLogo;
    if (!String.IsNullOrEmpty(mercanteAlterado.NrCnpj)) mercante.NrCnpj = mercanteAlterado.NrCnpj;
    if (!String.IsNullOrEmpty(mercanteAlterado.NrTelefoneFixo)) mercante.NrTelefoneFixo = mercanteAlterado.NrTelefoneFixo;
    if (!String.IsNullOrEmpty(mercanteAlterado.NrTelefoneCelular)) mercante.NrTelefoneCelular = mercanteAlterado.NrTelefoneCelular;

    _db.SaveChanges();

    return Results.Ok(mercante);
}).AllowAnonymous();

app.MapDelete("/api/mercante/remover/{idMercante}", ([FromServices] bdbuygeContext _db,
    [FromRoute] int idMercante
) =>
{
    var mercante = _db.TbMercante.Find(idMercante);

    if (mercante == null)
    {
        return Results.NotFound();
    }

    _db.TbMercante.Remove(mercante);
    _db.SaveChanges();

    return Results.Ok();
}).RequireAuthorization();
// FINAL MERCANTES

// COMEÇO ENDEREÇOS MERCANTES
app.MapGet("/api/mercante/enderecos/{idMercante}", ([FromServices] bdbuygeContext _db, [FromRoute] int idMercante
) =>
{
    var query = _db.TbEnderecoLoja.AsQueryable<TbEnderecoLoja>();
    var enderecosLoja = query.ToList<TbEnderecoLoja>().Where(e => e.FkCdMercante == idMercante);

    if (enderecosLoja == null)
    {
        return Results.NotFound();
    }

    return Results.Ok(enderecosLoja);
}).RequireAuthorization();

app.MapPost("/api/mercante/endereco", ([FromServices] bdbuygeContext _db,
    [FromBody] TbEnderecoLoja novoEnderecoLoja
) =>
{
    if (String.IsNullOrEmpty(novoEnderecoLoja.NmLogradouro))
    {
        return Results.BadRequest(new { mensagem = "Não é possivel incluir um endereço sem logradouro." });
    }

    var query = _db.TbEnderecoLoja.AsQueryable<TbEnderecoLoja>();
    var enderecos = query.ToList<TbEnderecoLoja>().Where(ej => ej.FkCdMercante == novoEnderecoLoja.FkCdMercante);

    var listaEnderecos = enderecos.ToList<TbEnderecoLoja>();

    if (listaEnderecos.Count > 0)
    {
        return Results.BadRequest(new { mensagem = "Loja já possui endereço cadastrado" });
    }

    var endereco = new TbEnderecoLoja
    {
        NmLogradouro = novoEnderecoLoja.NmLogradouro,
        NrEndereco = novoEnderecoLoja.NrEndereco,
        NmBairro = novoEnderecoLoja.NmBairro,
        NrCep = novoEnderecoLoja.NrCep,
        NmCidade = novoEnderecoLoja.NmCidade,
        SgEstado = novoEnderecoLoja.SgEstado,
        FkCdMercante = novoEnderecoLoja.FkCdMercante
    };

    _db.TbEnderecoLoja.Add(endereco);
    _db.SaveChanges();

    var enderecoUrl = $"/api/mercante/enderecos/{endereco.CdEndereco}";

    return Results.Created(enderecoUrl, endereco);
}).AllowAnonymous();

app.MapMethods("/api/mercante/endereco/{idEndereco}", new[] { "PATCH" }, ([FromServices] bdbuygeContext _db,
    [FromRoute] int idEndereco,
    [FromBody] TbEnderecoLoja enderecoLojaAlterado
) =>
{
    if (enderecoLojaAlterado.CdEndereco != idEndereco)
    {
        return Results.BadRequest(new { mensagem = "Id inconsistente." });
    }

    if (String.IsNullOrEmpty(enderecoLojaAlterado.NmLogradouro))
    {
        return Results.BadRequest(new { mensagem = "Não é permitido deixar endereço sem logradouro." });
    }

    var enderecoLoja = _db.TbEnderecoLoja.Find(idEndereco);

    if (enderecoLoja == null)
    {
        return Results.NotFound();
    }

    if (!String.IsNullOrEmpty(enderecoLojaAlterado.NmLogradouro)) enderecoLoja.NmLogradouro = enderecoLojaAlterado.NmLogradouro;
    if (enderecoLojaAlterado.NrEndereco > 0) enderecoLoja.NrEndereco = enderecoLojaAlterado.NrEndereco;
    if (!String.IsNullOrEmpty(enderecoLojaAlterado.NmBairro)) enderecoLoja.NmBairro = enderecoLojaAlterado.NmBairro;
    if (!String.IsNullOrEmpty(enderecoLojaAlterado.NrCep)) enderecoLoja.NrCep = enderecoLojaAlterado.NrCep;
    if (!String.IsNullOrEmpty(enderecoLojaAlterado.NmCidade)) enderecoLoja.NmCidade = enderecoLojaAlterado.NmCidade;
    if (!String.IsNullOrEmpty(enderecoLojaAlterado.SgEstado)) enderecoLoja.SgEstado = enderecoLojaAlterado.SgEstado;

    _db.SaveChanges();

    return Results.Ok(enderecoLoja);
}).RequireAuthorization();

app.MapDelete("/api/mercante/endereco/remover/{idEndereco}", ([FromServices] bdbuygeContext _db,
    [FromRoute] int idEndereco
) =>
{
    var enderecoLoja = _db.TbEnderecoLoja.Find(idEndereco);

    if (enderecoLoja == null)
    {
        return Results.NotFound();
    }

    _db.TbEnderecoLoja.Remove(enderecoLoja);
    _db.SaveChanges();

    return Results.Ok();
}).RequireAuthorization();
// FINAL ENDEREÇOS LOJA

// COMEÇO PRODUTOS
app.MapGet("/api/produtos", ([FromServices] bdbuygeContext _db) =>
{
    var query = _db.TbProduto.AsQueryable<TbProduto>();
    var produtos = query.ToList<TbProduto>();
    return Results.Ok(produtos);
}).AllowAnonymous();

app.MapGet("/api/produto/{idProduto}", ([FromServices] bdbuygeContext _db, [FromRoute] int idProduto) =>
{
    var produto = _db.TbProduto.Find(idProduto);

    if (produto == null)
    {
        return Results.NotFound();
    }

    return Results.Ok(produto);
}).AllowAnonymous();

app.MapGet("/api/produtos/mercante/{idMercante}", ([FromServices] bdbuygeContext _db, [FromRoute] int idMercante) =>
{
    var query = _db.TbProduto.AsQueryable<TbProduto>();
    var produtos = query.ToList<TbProduto>().Where(p => p.FkCdMercante == idMercante);

    if (produtos == null)
    {
        return Results.NotFound();
    }

    return Results.Ok(produtos);
}).AllowAnonymous();

app.MapPost("/api/produto/adicionar", ([FromServices] bdbuygeContext _db,
    [FromBody] ProdutoComImagem novoProdutoComImagem
) =>
{
    if (String.IsNullOrEmpty(novoProdutoComImagem.produto.NmProduto))
    {
        return Results.BadRequest(new { mensagem = "Não é possivel incluir um produto sem nome." });
    }

    var produto = new TbProduto
    {
        NmProduto = novoProdutoComImagem.produto.NmProduto,
        DsProduto = novoProdutoComImagem.produto.DsProduto,
        VlProduto = novoProdutoComImagem.produto.VlProduto,
        QtProduto = novoProdutoComImagem.produto.QtProduto,
        DtCriacao = DateTime.Now,
        FkCdMercante = novoProdutoComImagem.produto.FkCdMercante,
        FkCdCategoria = novoProdutoComImagem.produto.FkCdCategoria
    };

    _db.TbProduto.Add(produto);

    _db.SaveChanges();

    if (String.IsNullOrEmpty(novoProdutoComImagem.imagem.ImgProdutoLink))
    {
        return Results.BadRequest(new { mensagem = "Não é possivel incluir uma imagem sem link." });
    }

    var imagem = new TbProdutoImagem
    {
        ImgProdutoLink = novoProdutoComImagem.imagem.ImgProdutoLink,
        AltImagemProduto = novoProdutoComImagem.imagem.AltImagemProduto,
        IdPrincipal = 1,
        FkCdProduto = produto.CdProduto
    };

    _db.TbProdutoImagem.Add(imagem);

    _db.SaveChanges();

    var produtoUrl = $"/api/produtos/{produto.CdProduto}";

    return Results.Created(produtoUrl, produto);
}).RequireAuthorization();

app.MapMethods("/api/produto/atualizar/{idProduto}", new[] { "PATCH" }, ([FromServices] bdbuygeContext _db,
    [FromRoute] int idProduto,
    [FromBody] TbProduto produtoAlterado
) =>
{
    if (produtoAlterado.CdProduto != idProduto)
    {
        return Results.BadRequest(new { mensagem = "Id inconsistente." });
    }

    var produto = _db.TbProduto.Find(idProduto);

    if (produto == null)
    {
        return Results.NotFound();
    }

    if (!String.IsNullOrEmpty(produtoAlterado.NmProduto)) produto.NmProduto = produtoAlterado.NmProduto;
    if (!String.IsNullOrEmpty(produtoAlterado.DsProduto)) produto.DsProduto = produtoAlterado.DsProduto;
    if (produtoAlterado.VlProduto > 0) produto.VlProduto = produtoAlterado.VlProduto;
    if (produtoAlterado.QtProduto > 0) produto.QtProduto = produtoAlterado.QtProduto;
    if (produtoAlterado.VlPeso > 0) produto.VlPeso = produtoAlterado.VlPeso;
    if (produtoAlterado.VlTamanho > 0) produto.VlTamanho = produtoAlterado.VlTamanho;
    if (produtoAlterado.VlFrete > 0) produto.VlFrete = produtoAlterado.VlFrete;
    produto.IdDisponibilidade = produtoAlterado.IdDisponibilidade;
    if (produtoAlterado.FkCdCategoria > 0) produto.FkCdCategoria = produtoAlterado.FkCdCategoria;

    _db.SaveChanges();

    return Results.Ok();
}).AllowAnonymous();

app.MapDelete("/api/produto/remover/{idProduto}", ([FromServices] bdbuygeContext _db,
    [FromRoute] int idProduto
) =>
{
    var produto = _db.TbProduto.Find(idProduto);

    if (produto == null)
    {
        return Results.NotFound();
    }

    var query = _db.TbProdutoImagem.AsQueryable<TbProdutoImagem>();
    var imagensProduto = query.ToList<TbProdutoImagem>().Where(ip => ip.FkCdProduto == idProduto);

    var listaImagensProduto = imagensProduto.ToList<TbProdutoImagem>();

    if (listaImagensProduto.Count() > 0)
    {
        listaImagensProduto.ForEach((imagem) =>
        {
            _db.TbProdutoImagem.Remove(imagem);
        });
    }

    _db.TbProduto.Remove(produto);
    _db.SaveChanges();

    return Results.Ok();
}).RequireAuthorization();
// FINAL PRODUTOS

// COMEÇO IMAGENS PRODUTOS
app.MapGet("/api/produtos/produto-imagem", ([FromServices] bdbuygeContext _db) =>
{
    var query = _db.TbProdutoImagem.AsQueryable<TbProdutoImagem>();
    var produtosImagens = query.ToList<TbProdutoImagem>();
    return Results.Ok(produtosImagens);
}).AllowAnonymous();

app.MapGet("/api/produtos/produto-imagem/{idProdutoImagem}", ([FromServices] bdbuygeContext _db, [FromRoute] int idProdutoImagem) =>
{
    var imagem = _db.TbProdutoImagem.Find(idProdutoImagem);

    if (imagem == null)
    {
        return Results.NotFound();
    }

    return Results.Ok(imagem);
}).AllowAnonymous();

app.MapGet("/api/produtos/produto-imagem/{idProduto}/todas", ([FromServices] bdbuygeContext _db, [FromRoute] int idProduto) =>
{
    var query = _db.TbProdutoImagem.AsQueryable<TbProdutoImagem>();
    var produtoImagens = query.ToList<TbProdutoImagem>().Where(pi => pi.FkCdProduto == idProduto);

    if (produtoImagens == null)
    {
        return Results.NotFound();
    }

    return Results.Ok(produtoImagens);
}).AllowAnonymous();

app.MapPost("/api/produtos/produto-imagem/adicionar", ([FromServices] bdbuygeContext _db,
    [FromBody] TbProdutoImagem novaImagem
) =>
{
    Boolean valido = false;

    var produtoImagem = new TbProdutoImagem
    {
        ImgProdutoLink = null,
        ImgProduto = null,
        AltImagemProduto = novaImagem.AltImagemProduto,
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
}).RequireAuthorization();

app.MapMethods("/api/produtos/produto-imagem/{idProdutoImagem}", new[] { "PATCH" }, ([FromServices] bdbuygeContext _db,
    [FromRoute] int idProdutoImagem,
    [FromBody] TbProdutoImagem produtoImagemAlterado
) =>
{
    if (produtoImagemAlterado.CdProdutoImagem != idProdutoImagem)
    {
        return Results.BadRequest(new { mensagem = "Id inconsistente." });
    }

    var produtoImagem = _db.TbProdutoImagem.Find(idProdutoImagem);

    if (produtoImagem == null)
    {
        return Results.NotFound();
    }

    if (!String.IsNullOrEmpty(produtoImagemAlterado.ImgProdutoLink)) produtoImagem.ImgProdutoLink = produtoImagemAlterado.ImgProdutoLink;
    if (!String.IsNullOrEmpty(produtoImagemAlterado.AltImagemProduto)) produtoImagem.AltImagemProduto = produtoImagemAlterado.AltImagemProduto;

    _db.SaveChanges();

    return Results.Ok(produtoImagem);
}).RequireAuthorization();

app.MapDelete("/api/produtos/produto-imagem/remover/{idProdutoImagem}", ([FromServices] bdbuygeContext _db,
    [FromRoute] int idProdutoImagem
) =>
{
    var produtoImagem = _db.TbProdutoImagem.Find(idProdutoImagem);

    if (produtoImagem == null)
    {
        return Results.NotFound();
    }

    _db.TbProdutoImagem.Remove(produtoImagem);
    _db.SaveChanges();

    return Results.Ok();
}).RequireAuthorization();
// FINAL IMAGENS PRODUTOS

// COMEÇO ITEM FAVORITO
app.MapGet("/api/favoritos/{idCliente}", ([FromServices] bdbuygeContext _db, [FromRoute] int idCliente) =>
{
    var query = _db.TbItemFavorito.AsQueryable<TbItemFavorito>();
    var items = query.ToList<TbItemFavorito>().Where(i => i.FkCdCliente == idCliente);

    return Results.Ok(items);
}).RequireAuthorization();

app.MapPost("/api/favorito/adicionar/{idCliente}/{idProduto}", ([FromServices] bdbuygeContext _db,
    [FromRoute] int idCliente, [FromRoute] int idProduto
) =>
{
    Boolean valido = true;

    var produto = _db.TbProduto.Find(idProduto);

    if (produto == null)
    {
        return Results.NotFound();
    }

    var query = _db.TbItemFavorito.AsQueryable<TbItemFavorito>();
    var items = query.ToList<TbItemFavorito>().Where(i => i.FkCdCliente == idCliente);

    var favoritos = items.ToList<TbItemFavorito>();

    favoritos.ForEach((favorito) =>
    {
        if (favorito.FkCdProduto == idProduto)
        {
            valido = false;
        }
    });

    if (!valido)
    {
        return Results.BadRequest(new { mensagem = "Produto já adicionado aos favoritos" });
    }

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

app.MapDelete("/api/favorito/remover/{idCliente}/{idProduto}", ([FromServices] bdbuygeContext _db,
    [FromRoute] int idCliente, [FromRoute] int idProduto
) =>
{
    int idItemFavorito = -1;

    var produto = _db.TbProduto.Find(idProduto);

    if (produto == null)
    {
        return Results.NotFound();
    }

    var query = _db.TbItemFavorito.AsQueryable<TbItemFavorito>();
    var items = query.ToList<TbItemFavorito>().Where(i => i.FkCdCliente == idCliente);

    var favoritos = items.ToList<TbItemFavorito>();

    favoritos.ForEach((favorito) =>
    {
        if (favorito.FkCdProduto == produto.CdProduto)
        {
            idItemFavorito = favorito.CdItemFavorito;
        }
    });

    if (idItemFavorito == -1)
    {
        return Results.NotFound();
    }

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

// COMEÇO ITEM CARRINHO
app.MapGet("/api/carrinho/items/{idCliente}", ([FromServices] bdbuygeContext _db, [FromRoute] int idCliente) =>
{
    var query = _db.TbItemCarrinho.AsQueryable<TbItemCarrinho>();
    var itemsCarrinho = query.ToList<TbItemCarrinho>().Where(i => i.FkCdCliente == idCliente);

    if (itemsCarrinho == null)
    {
        return Results.NotFound();
    }

    return Results.Ok(itemsCarrinho);
}).RequireAuthorization();

app.MapPost("/api/carrinho/item/novo/{idCliente}/{idProduto}", ([FromServices] bdbuygeContext _db,
    [FromRoute] int idCliente, [FromRoute] int idProduto
) =>
{
    Boolean valido = true;

    var cliente = _db.TbCliente.Find(idCliente);

    if (cliente == null)
    {
        return Results.NotFound();
    }

    var produto = _db.TbProduto.Find(idProduto);

    if (produto == null)
    {
        return Results.NotFound();
    }

    var query = _db.TbItemCarrinho.AsQueryable<TbItemCarrinho>();
    var items = query.ToList<TbItemCarrinho>().Where(ic => ic.FkCdCliente == idCliente);

    var itemsCarrinho = items.ToList<TbItemCarrinho>();

    itemsCarrinho.ForEach((item) =>
    {
        if (item.FkCdProduto == idProduto)
        {
            valido = false;
        }
    });

    if (!valido)
    {
        return Results.BadRequest(new { mensagem = "Produto já adicionado no carrinho." });
    }

    var itemCarrinho = new TbItemCarrinho
    {
        QtItemCarrinho = 1,
        FkCdProduto = produto.CdProduto,
        FkCdCliente = cliente.CdCliente
    };

    _db.TbItemCarrinho.Add(itemCarrinho);
    _db.SaveChanges();

    var itemsCarrinhoUrl = $"/api/carrinho/items/{itemCarrinho.FkCdCliente}";

    return Results.Created(itemsCarrinhoUrl, itemCarrinho);
}).RequireAuthorization();

app.MapMethods("/api/carrinho/item/{idItemCarrinho}", new[] { "PATCH" }, ([FromServices] bdbuygeContext _db,
    [FromRoute] int idItemCarrinho,
    [FromBody] TbItemCarrinho itemCarrinhoAlterado
) =>
{
    if (itemCarrinhoAlterado.CdItemCarrinho != idItemCarrinho)
    {
        return Results.BadRequest(new { mensagem = "Id inconsistente." });
    }

    var itemCarrinho = _db.TbItemCarrinho.Find(idItemCarrinho);

    if (itemCarrinho == null)
    {
        return Results.NotFound();
    }

    if (itemCarrinhoAlterado.QtItemCarrinho > 0) itemCarrinho.QtItemCarrinho = itemCarrinhoAlterado.QtItemCarrinho;

    _db.SaveChanges();

    return Results.Ok(itemCarrinho);
}).RequireAuthorization();

app.MapDelete("/api/carrinho/item/remover/{idItemCarrinho}", ([FromServices] bdbuygeContext _db,
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

// COMPRA DE PRODUTOS
app.MapGet("/api/compras/{idCliente}", ([FromServices] bdbuygeContext _db, [FromRoute] int idCliente
) =>
{
    var query = _db.TbCompra.AsQueryable<TbCompra>();
    var compras = query.ToList<TbCompra>().Where(c => c.FkCdCliente == idCliente);

    if (compras == null)
    {
        return Results.NotFound();
    }

    return Results.Ok(compras);
}).RequireAuthorization();

app.MapGet("/api/compra/{idCompra}", ([FromServices] bdbuygeContext _db, [FromRoute] int idCompra) =>
{
    var compra = _db.TbCompra.Find(idCompra);

    if (compra == null)
    {
        return Results.NotFound();
    }

    return Results.Ok(compra);
}).RequireAuthorization();

app.MapPost("/api/comprar/{idCliente}", async ([FromServices] bdbuygeContext _db, [FromRoute] int idCliente
) =>
{
    /*var cliente = _db.TbCliente.Find(idCliente);

    if (cliente == null)
    {
        return Results.NotFound();
    }*/

    var query = _db.TbItemCarrinho.AsQueryable<TbItemCarrinho>();
    var itemsCarrinho = query.ToList<TbItemCarrinho>().Where(i => i.FkCdCliente == idCliente);

    var listaItemsCarrinho = itemsCarrinho.ToList<TbItemCarrinho>();
    var produtos = new List<TbProduto>();

    listaItemsCarrinho.ForEach((item) =>
    {
        var produto = _db.TbProduto.Find(item.FkCdProduto);

        if (produto != null)
        {
            produtos.Add(produto);
        }
    });

    var Items = new List<PreferenceItemRequest> { };

    produtos.ForEach((produto) =>
    {
        var item = new PreferenceItemRequest
        {
            Title = produto.NmProduto,
            Quantity = 1,
            Description = produto.DsProduto,
            CurrencyId = "BRL",
            UnitPrice = produto.VlProduto,
        };

        Items.Add(item);
    });

    /*var Payer = new PreferencePayerRequest
    {
        Name = cliente.NmCliente,
        Surname = cliente.NmSobrenome,
        Email = cliente.NmEmail,
        Phone = {
            AreaCode = "11",
            Number = "4444-4444"
        },
        Identification = {
            Type = "CPF",
            Number = cliente.NrCpf
        },
        Address = {
            StreetName = "Street",
            StreetNumber = "123",
            ZipCode = "06233200"
        }
    };*/

    var BackUrls = new PreferenceBackUrlsRequest
    {
        Success = "http://www.buyge.com.br/src/pages/compras/compra.html",
        Failure = "http://www.buyge.com.br/src/pages/compras/compra.html",
        Pending = "http://www.buyge.com.br/src/pages/compras/compra.html"
    };

    var request = new PreferenceRequest
    {
        Items = Items,
        /*Payer = Payer,*/
        BackUrls = BackUrls,
        AutoReturn = "approved"
    };

    // Cria a preferência usando o client
    var client = new PreferenceClient();
    Preference preference = await client.CreateAsync(request);

    return Results.Ok(preference);
}).RequireAuthorization();

app.MapPost("/api/comprar/salvar/{idCliente}", ([FromServices] bdbuygeContext _db,
    [FromRoute] int idCliente, [FromBody] TbCompra novaCompra
) =>
{
    var cliente = _db.TbCliente.Find(idCliente);

    if (cliente == null)
    {
        return Results.NotFound();
    }

    var compra = new TbCompra
    {
        IdPreferencia = novaCompra.IdPreferencia,
        NmStatus = novaCompra.NmStatus,
        NmCollectionId = novaCompra.NmCollectionId,
        NmMerchantOrderId = novaCompra.NmMerchantOrderId,
        NmPaymentId = novaCompra.NmPaymentId,
        NmPaymentType = novaCompra.NmPaymentType,
        NmCollectionStatus = novaCompra.NmCollectionStatus,
        FkCdCliente = cliente.CdCliente
    };

    _db.TbCompra.Add(compra);

    _db.SaveChanges();

    var query = _db.TbItemCarrinho.AsQueryable<TbItemCarrinho>();
    var itemsCarrinho = query.ToList<TbItemCarrinho>().Where(i => i.FkCdCliente == idCliente);

    var listaItemsCarrinho = itemsCarrinho.ToList<TbItemCarrinho>();

    listaItemsCarrinho.ForEach((item) =>
    {
        var produto = _db.TbProduto.Find(item.FkCdProduto);

        if (produto != null)
        {
            var itemCompra = new TbItemCompra
            {
                NmProduto = produto.NmProduto,
                DsProduto = produto.DsProduto,
                VlItemCompra = produto.VlProduto,
                QtItemCompra = item.QtItemCarrinho,
                FkCdCompra = compra.CdCompra
            };

            _db.TbItemCompra.Add(itemCompra);

            _db.TbItemCarrinho.Remove(item);
        }
    });

    _db.SaveChanges();

    var pesquisa = _db.TbItemCompra.AsQueryable<TbItemCompra>();
    var itemsCompra = pesquisa.ToList<TbItemCompra>().Where(ic => ic.FkCdCompra == compra.CdCompra);

    var listaItemsCompra = itemsCompra.ToList<TbItemCompra>();

    Decimal valorTotal = 0;

    listaItemsCompra.ForEach((item) =>
    {
        valorTotal += item.VlItemCompra * item.QtItemCompra;
    });

    var compraAtual = _db.TbCompra.Find(compra.CdCompra);

    if (compraAtual == null)
    {
        return Results.NotFound();
    }

    compraAtual.VlTotalCompra = valorTotal;

    _db.SaveChanges();

    var compraUrl = $"/api/compras/{compra.CdCompra}";

    return Results.Created(compraUrl, compra);
}).RequireAuthorization();
// FINAL COMPRA DE PRODUTOS

// ITEMS DE COMPRA
app.MapGet("/api/compras/items/{idCompra}", ([FromServices] bdbuygeContext _db, [FromRoute] int idCompra) =>
{
    var query = _db.TbItemCompra.AsQueryable<TbItemCompra>();
    var itemsCompra = query.ToList<TbItemCompra>().Where(ic => ic.FkCdCompra == idCompra);

    if (itemsCompra == null)
    {
        return Results.NotFound();
    }

    return Results.Ok(itemsCompra);
}).RequireAuthorization();
// FINAL ITEMS DE COMPRA
app.Run();
