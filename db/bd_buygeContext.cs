using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace buyge_backend.db
{
    public partial class bd_buygeContext : DbContext
    {
        public bd_buygeContext()
        {
        }

        public bd_buygeContext(DbContextOptions<bd_buygeContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TbCarrinho> TbCarrinho { get; set; } = null!;
        public virtual DbSet<TbCategoria> TbCategoria { get; set; } = null!;
        public virtual DbSet<TbCliente> TbCliente { get; set; } = null!;
        public virtual DbSet<TbEndereco> TbEndereco { get; set; } = null!;
        public virtual DbSet<TbFavorito> TbFavorito { get; set; } = null!;
        public virtual DbSet<TbItemCarrinho> TbItemCarrinho { get; set; } = null!;
        public virtual DbSet<TbItemFavorito> TbItemFavorito { get; set; } = null!;
        public virtual DbSet<TbJogador> TbJogador { get; set; } = null!;
        public virtual DbSet<TbMercante> TbMercante { get; set; } = null!;
        public virtual DbSet<TbProduto> TbProduto { get; set; } = null!;
        public virtual DbSet<TbProdutoImagem> TbProdutoImagem { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseMySql("server=localhost;port=3306;user=root;password=admin;database=bd_buyge", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.28-mysql"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("utf8_general_ci")
                .HasCharSet("utf8");

            modelBuilder.Entity<TbCarrinho>(entity =>
            {
                entity.HasKey(e => new { e.CdCarrinho, e.FkCdCliente })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

                entity.ToTable("tb_carrinho");

                entity.HasIndex(e => e.FkCdCliente, "fk_tb_carrinho_tb_cliente1_idx");

                entity.Property(e => e.CdCarrinho)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("cd_carrinho");

                entity.Property(e => e.FkCdCliente).HasColumnName("fk_cd_cliente");

                entity.HasOne(d => d.FkCdClienteNavigation)
                    .WithMany(p => p.TbCarrinho)
                    .HasForeignKey(d => d.FkCdCliente)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_tb_carrinho_tb_cliente1");
            });

            modelBuilder.Entity<TbCategoria>(entity =>
            {
                entity.HasKey(e => e.CdCategoria)
                    .HasName("PRIMARY");

                entity.ToTable("tb_categoria");

                entity.Property(e => e.CdCategoria).HasColumnName("cd_categoria");

                entity.Property(e => e.DsCategoria).HasColumnName("ds_categoria");

                entity.Property(e => e.NmCategoria)
                    .HasMaxLength(30)
                    .HasColumnName("nm_categoria");
            });

            modelBuilder.Entity<TbCliente>(entity =>
            {
                entity.HasKey(e => e.CdCliente)
                    .HasName("PRIMARY");

                entity.ToTable("tb_cliente");

                entity.Property(e => e.CdCliente).HasColumnName("cd_cliente");

                entity.Property(e => e.DtNascimentoCliente).HasColumnName("dt_nascimento_cliente");

                entity.Property(e => e.NmCliente)
                    .HasMaxLength(20)
                    .HasColumnName("nm_cliente");

                entity.Property(e => e.NmEmailCliente)
                    .HasMaxLength(30)
                    .HasColumnName("nm_email_cliente");

                entity.Property(e => e.NmLogin)
                    .HasMaxLength(30)
                    .HasColumnName("nm_login");

                entity.Property(e => e.NmSenha)
                    .HasMaxLength(16)
                    .HasColumnName("nm_senha");

                entity.Property(e => e.NmSobrenomeCliente)
                    .HasMaxLength(40)
                    .HasColumnName("nm_sobrenome_cliente");

                entity.Property(e => e.NrTelefone)
                    .HasMaxLength(11)
                    .HasColumnName("nr_telefone")
                    .IsFixedLength();
            });

            modelBuilder.Entity<TbEndereco>(entity =>
            {
                entity.HasKey(e => new { e.CdEndereco, e.FkCdCliente })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

                entity.ToTable("tb_endereco");

                entity.HasIndex(e => e.FkCdCliente, "fk_tb_endereco_tb_cliente_idx");

                entity.Property(e => e.CdEndereco)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("cd_endereco");

                entity.Property(e => e.FkCdCliente).HasColumnName("fk_cd_cliente");

                entity.Property(e => e.NmBairroEndereco)
                    .HasMaxLength(30)
                    .HasColumnName("nm_bairro_endereco");

                entity.Property(e => e.NmCidadeEndereco)
                    .HasMaxLength(30)
                    .HasColumnName("nm_cidade_endereco");

                entity.Property(e => e.NmLogradouroEndereco)
                    .HasMaxLength(30)
                    .HasColumnName("nm_logradouro_endereco");

                entity.Property(e => e.NrCepEndereco).HasColumnName("nr_cep_endereco");

                entity.Property(e => e.NrEndereco).HasColumnName("nr_endereco");

                entity.Property(e => e.SgEstado)
                    .HasMaxLength(2)
                    .HasColumnName("sg_estado")
                    .IsFixedLength();

                entity.HasOne(d => d.FkCdClienteNavigation)
                    .WithMany(p => p.TbEndereco)
                    .HasForeignKey(d => d.FkCdCliente)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_tb_endereco_tb_cliente");
            });

            modelBuilder.Entity<TbFavorito>(entity =>
            {
                entity.HasKey(e => new { e.CdFavorito, e.FkCdCliente })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

                entity.ToTable("tb_favorito");

                entity.HasIndex(e => e.FkCdCliente, "fk_tb_favorito_tb_cliente1_idx");

                entity.Property(e => e.CdFavorito)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("cd_favorito");

                entity.Property(e => e.FkCdCliente).HasColumnName("fk_cd_cliente");

                entity.HasOne(d => d.FkCdClienteNavigation)
                    .WithMany(p => p.TbFavorito)
                    .HasForeignKey(d => d.FkCdCliente)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_tb_favorito_tb_cliente1");
            });

            modelBuilder.Entity<TbItemCarrinho>(entity =>
            {
                entity.HasKey(e => e.CdItemCarrinho)
                    .HasName("PRIMARY");

                entity.ToTable("tb_item_carrinho");

                entity.HasIndex(e => e.FkCdCarrinho, "fk_tb_item_carrinho_tb_carrinho1_idx");

                entity.HasIndex(e => e.FkCdProduto, "fk_tb_item_carrinho_tb_produto1_idx");

                entity.Property(e => e.CdItemCarrinho).HasColumnName("cd_item_carrinho");

                entity.Property(e => e.FkCdCarrinho).HasColumnName("fk_cd_carrinho");

                entity.Property(e => e.FkCdProduto).HasColumnName("fk_cd_produto");

                entity.Property(e => e.VlItemCarrinho)
                    .HasPrecision(8, 2)
                    .HasColumnName("vl_item_carrinho");

                entity.HasOne(d => d.FkCdProdutoNavigation)
                    .WithMany(p => p.TbItemCarrinho)
                    .HasForeignKey(d => d.FkCdProduto)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_tb_item_carrinho_tb_produto1");
            });

            modelBuilder.Entity<TbItemFavorito>(entity =>
            {
                entity.HasKey(e => e.CdItemFavorito)
                    .HasName("PRIMARY");

                entity.ToTable("tb_item_favorito");

                entity.HasIndex(e => e.FkCdFavorito, "fk_tb_item_favorito_tb_favorito1_idx");

                entity.HasIndex(e => e.FkCdProduto, "fk_tb_item_favorito_tb_produto1_idx");

                entity.Property(e => e.CdItemFavorito).HasColumnName("cd_item_favorito");

                entity.Property(e => e.FkCdFavorito).HasColumnName("fk_cd_favorito");

                entity.Property(e => e.FkCdProduto).HasColumnName("fk_cd_produto");

                entity.HasOne(d => d.FkCdProdutoNavigation)
                    .WithMany(p => p.TbItemFavorito)
                    .HasForeignKey(d => d.FkCdProduto)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_tb_item_favorito_tb_produto1");
            });

            modelBuilder.Entity<TbJogador>(entity =>
            {
                entity.HasKey(e => new { e.CdJogador, e.FkCdCliente })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

                entity.ToTable("tb_jogador");

                entity.HasIndex(e => e.FkCdCliente, "fk_tb_jogador_tb_cliente1_idx");

                entity.Property(e => e.CdJogador)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("cd_jogador");

                entity.Property(e => e.FkCdCliente).HasColumnName("fk_cd_cliente");

                entity.Property(e => e.NmClasse)
                    .HasMaxLength(20)
                    .HasColumnName("nm_classe");

                entity.Property(e => e.NrNivelJogador).HasColumnName("nr_nivel_jogador");

                entity.Property(e => e.NrXpJogador).HasColumnName("nr_xp_jogador");

                entity.HasOne(d => d.FkCdClienteNavigation)
                    .WithMany(p => p.TbJogador)
                    .HasForeignKey(d => d.FkCdCliente)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_tb_jogador_tb_cliente1");
            });

            modelBuilder.Entity<TbMercante>(entity =>
            {
                entity.HasKey(e => e.CdMercante)
                    .HasName("PRIMARY");

                entity.ToTable("tb_mercante");

                entity.HasIndex(e => e.FkCdCliente, "fk_tb_loja_virtual_tb_cliente1_idx");

                entity.Property(e => e.CdMercante).HasColumnName("cd_mercante");

                entity.Property(e => e.DsLojaMercante).HasColumnName("ds_loja_mercante");

                entity.Property(e => e.FkCdCliente).HasColumnName("fk_cd_cliente");

                entity.Property(e => e.ImgLogoMercante)
                    .HasColumnType("blob")
                    .HasColumnName("img_logo_mercante");

                entity.Property(e => e.NmLojaMercante)
                    .HasMaxLength(30)
                    .HasColumnName("nm_loja_mercante");

                entity.HasOne(d => d.FkCdClienteNavigation)
                    .WithMany(p => p.TbMercante)
                    .HasForeignKey(d => d.FkCdCliente)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_tb_loja_virtual_tb_cliente1");
            });

            modelBuilder.Entity<TbProduto>(entity =>
            {
                entity.HasKey(e => e.CdProduto)
                    .HasName("PRIMARY");

                entity.ToTable("tb_produto");

                entity.Property(e => e.CdProduto).HasColumnName("cd_produto");

                entity.Property(e => e.DsCaracteristicas).HasColumnName("ds_caracteristicas");

                entity.Property(e => e.DsProduto).HasColumnName("ds_produto");

                entity.Property(e => e.NmProduto)
                    .HasMaxLength(30)
                    .HasColumnName("nm_produto");

                entity.Property(e => e.QtEstoqueProduto).HasColumnName("qt_estoque_produto");

                entity.Property(e => e.VlProduto)
                    .HasPrecision(8, 2)
                    .HasColumnName("vl_produto");

                entity.HasMany(d => d.FkCdCategoria)
                    .WithMany(p => p.FkCdProduto)
                    .UsingEntity<Dictionary<string, object>>(
                        "TbProdutoCategoria",
                        l => l.HasOne<TbCategoria>().WithMany().HasForeignKey("FkCdCategoria").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("fk_tb_produto_has_tb_categoria_tb_categoria1"),
                        r => r.HasOne<TbProduto>().WithMany().HasForeignKey("FkCdProduto").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("fk_tb_produto_has_tb_categoria_tb_produto1"),
                        j =>
                        {
                            j.HasKey("FkCdProduto", "FkCdCategoria").HasName("PRIMARY").HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

                            j.ToTable("tb_produto_categoria");

                            j.HasIndex(new[] { "FkCdCategoria" }, "fk_tb_produto_has_tb_categoria_tb_categoria1_idx");

                            j.HasIndex(new[] { "FkCdProduto" }, "fk_tb_produto_has_tb_categoria_tb_produto1_idx");

                            j.IndexerProperty<int>("FkCdProduto").HasColumnName("fk_cd_produto");

                            j.IndexerProperty<int>("FkCdCategoria").HasColumnName("fk_cd_categoria");
                        });
            });

            modelBuilder.Entity<TbProdutoImagem>(entity =>
            {
                entity.HasKey(e => new { e.CdProdutoImagem, e.FkCdProduto })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

                entity.ToTable("tb_produto_imagem");

                entity.HasIndex(e => e.FkCdProduto, "fk_tb_produto_imagem_tb_produto1_idx");

                entity.Property(e => e.CdProdutoImagem)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("cd_produto_imagem");

                entity.Property(e => e.FkCdProduto).HasColumnName("fk_cd_produto");

                entity.Property(e => e.ImgProduto)
                    .HasColumnType("blob")
                    .HasColumnName("img_produto");

                entity.Property(e => e.NmProdutoImagem)
                    .HasMaxLength(35)
                    .HasColumnName("nm_produto_imagem");

                entity.HasOne(d => d.FkCdProdutoNavigation)
                    .WithMany(p => p.TbProdutoImagem)
                    .HasForeignKey(d => d.FkCdProduto)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_tb_produto_imagem_tb_produto1");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
