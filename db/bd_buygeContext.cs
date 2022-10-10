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
        public virtual DbSet<TbCompra> TbCompra { get; set; } = null!;
        public virtual DbSet<TbEndereco> TbEndereco { get; set; } = null!;
        public virtual DbSet<TbFavorito> TbFavorito { get; set; } = null!;
        public virtual DbSet<TbItemCarrinho> TbItemCarrinho { get; set; } = null!;
        public virtual DbSet<TbItemCompra> TbItemCompra { get; set; } = null!;
        public virtual DbSet<TbItemFavorito> TbItemFavorito { get; set; } = null!;
        public virtual DbSet<TbMercante> TbMercante { get; set; } = null!;
        public virtual DbSet<TbProduto> TbProduto { get; set; } = null!;
        public virtual DbSet<TbProdutoImagem> TbProdutoImagem { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            { }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("utf8mb4_0900_ai_ci")
                .HasCharSet("utf8mb4");

            modelBuilder.Entity<TbCarrinho>(entity =>
            {
                entity.HasKey(e => new { e.CdCarrinho, e.FkCdCliente })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

                entity.ToTable("tb_carrinho");

                entity.HasIndex(e => e.FkCdCliente, "fk_cd_cliente");

                entity.Property(e => e.CdCarrinho)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("cd_carrinho");

                entity.Property(e => e.FkCdCliente).HasColumnName("fk_cd_cliente");

                entity.HasOne(d => d.FkCdClienteNavigation)
                    .WithMany(p => p.TbCarrinho)
                    .HasForeignKey(d => d.FkCdCliente)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("tb_carrinho_ibfk_1");
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

                entity.HasMany(d => d.FkCdProduto)
                    .WithMany(p => p.FkCdCategoria)
                    .UsingEntity<Dictionary<string, object>>(
                        "TbProdutoCategoria",
                        l => l.HasOne<TbProduto>().WithMany().HasForeignKey("FkCdProduto").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("tb_produto_categoria_ibfk_2"),
                        r => r.HasOne<TbCategoria>().WithMany().HasForeignKey("FkCdCategoria").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("tb_produto_categoria_ibfk_1"),
                        j =>
                        {
                            j.HasKey("FkCdCategoria", "FkCdProduto").HasName("PRIMARY").HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

                            j.ToTable("tb_produto_categoria");

                            j.HasIndex(new[] { "FkCdProduto" }, "fk_cd_produto");

                            j.IndexerProperty<int>("FkCdCategoria").HasColumnName("fk_cd_categoria");

                            j.IndexerProperty<int>("FkCdProduto").HasColumnName("fk_cd_produto");
                        });
            });

            modelBuilder.Entity<TbCliente>(entity =>
            {
                entity.HasKey(e => e.CdCliente)
                    .HasName("PRIMARY");

                entity.ToTable("tb_cliente");

                entity.Property(e => e.CdCliente).HasColumnName("cd_cliente");

                entity.Property(e => e.DtNascimento).HasColumnName("dt_nascimento");

                entity.Property(e => e.NmCliente)
                    .HasMaxLength(40)
                    .HasColumnName("nm_cliente");

                entity.Property(e => e.NmLogin)
                    .HasMaxLength(30)
                    .HasColumnName("nm_login");

                entity.Property(e => e.NmSenha)
                    .HasMaxLength(16)
                    .HasColumnName("nm_senha");

                entity.Property(e => e.NmSobrenome)
                    .HasMaxLength(60)
                    .HasColumnName("nm_sobrenome");

                entity.Property(e => e.NrTelefone)
                    .HasMaxLength(11)
                    .HasColumnName("nr_telefone")
                    .IsFixedLength();
            });

            modelBuilder.Entity<TbCompra>(entity =>
            {
                entity.HasKey(e => e.CdCompra)
                    .HasName("PRIMARY");

                entity.ToTable("tb_compra");

                entity.HasIndex(e => e.TbClienteCdCliente, "tb_cliente_cd_cliente");

                entity.Property(e => e.CdCompra).HasColumnName("cd_compra");

                entity.Property(e => e.TbClienteCdCliente).HasColumnName("tb_cliente_cd_cliente");

                entity.Property(e => e.VlTotalCompra)
                    .HasMaxLength(45)
                    .HasColumnName("vl_total_compra");

                entity.HasOne(d => d.TbClienteCdClienteNavigation)
                    .WithMany(p => p.TbCompra)
                    .HasForeignKey(d => d.TbClienteCdCliente)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("tb_compra_ibfk_1");
            });

            modelBuilder.Entity<TbEndereco>(entity =>
            {
                entity.HasKey(e => e.CdEndereco)
                    .HasName("PRIMARY");

                entity.ToTable("tb_endereco");

                entity.HasIndex(e => e.FkCdCliente, "fk_cd_cliente");

                entity.Property(e => e.CdEndereco).HasColumnName("cd_endereco");

                entity.Property(e => e.FkCdCliente).HasColumnName("fk_cd_cliente");

                entity.Property(e => e.NmBairro)
                    .HasMaxLength(30)
                    .HasColumnName("nm_bairro");

                entity.Property(e => e.NmCidade)
                    .HasMaxLength(30)
                    .HasColumnName("nm_cidade");

                entity.Property(e => e.NmLogradouro)
                    .HasMaxLength(30)
                    .HasColumnName("nm_logradouro");

                entity.Property(e => e.NrCep)
                    .HasMaxLength(8)
                    .HasColumnName("nr_cep")
                    .IsFixedLength();

                entity.Property(e => e.NrEndereco).HasColumnName("nr_endereco");

                entity.Property(e => e.SgEstado)
                    .HasMaxLength(2)
                    .HasColumnName("sg_estado")
                    .IsFixedLength();

                entity.HasOne(d => d.FkCdClienteNavigation)
                    .WithMany(p => p.TbEndereco)
                    .HasForeignKey(d => d.FkCdCliente)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("tb_endereco_ibfk_1");
            });

            modelBuilder.Entity<TbFavorito>(entity =>
            {
                entity.HasKey(e => new { e.CdFavorito, e.FkCdCliente })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

                entity.ToTable("tb_favorito");

                entity.HasIndex(e => e.FkCdCliente, "fk_cd_cliente");

                entity.Property(e => e.CdFavorito)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("cd_favorito");

                entity.Property(e => e.FkCdCliente).HasColumnName("fk_cd_cliente");

                entity.HasOne(d => d.FkCdClienteNavigation)
                    .WithMany(p => p.TbFavorito)
                    .HasForeignKey(d => d.FkCdCliente)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("tb_favorito_ibfk_1");
            });

            modelBuilder.Entity<TbItemCarrinho>(entity =>
            {
                entity.HasKey(e => new { e.FkCdProduto, e.FkCdCarrinho })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

                entity.ToTable("tb_item_carrinho");

                entity.HasIndex(e => e.FkCdCarrinho, "fk_cd_carrinho");

                entity.Property(e => e.FkCdProduto).HasColumnName("fk_cd_produto");

                entity.Property(e => e.FkCdCarrinho).HasColumnName("fk_cd_carrinho");

                entity.HasOne(d => d.FkCdProdutoNavigation)
                    .WithMany(p => p.TbItemCarrinho)
                    .HasForeignKey(d => d.FkCdProduto)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("tb_item_carrinho_ibfk_1");
            });

            modelBuilder.Entity<TbItemCompra>(entity =>
            {
                entity.HasKey(e => new { e.FkCdProduto, e.FkCdCompra })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

                entity.ToTable("tb_item_compra");

                entity.HasIndex(e => e.FkCdCompra, "fk_cd_compra");

                entity.Property(e => e.FkCdProduto).HasColumnName("fk_cd_produto");

                entity.Property(e => e.FkCdCompra).HasColumnName("fk_cd_compra");

                entity.Property(e => e.VlItemCompra)
                    .HasPrecision(8, 2)
                    .HasColumnName("vl_item_compra");

                entity.HasOne(d => d.FkCdCompraNavigation)
                    .WithMany(p => p.TbItemCompra)
                    .HasForeignKey(d => d.FkCdCompra)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("tb_item_compra_ibfk_2");

                entity.HasOne(d => d.FkCdProdutoNavigation)
                    .WithMany(p => p.TbItemCompra)
                    .HasForeignKey(d => d.FkCdProduto)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("tb_item_compra_ibfk_1");
            });

            modelBuilder.Entity<TbItemFavorito>(entity =>
            {
                entity.HasKey(e => new { e.FkCdProduto, e.FkCdFavorito })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

                entity.ToTable("tb_item_favorito");

                entity.HasIndex(e => e.FkCdFavorito, "fk_cd_favorito");

                entity.Property(e => e.FkCdProduto).HasColumnName("fk_cd_produto");

                entity.Property(e => e.FkCdFavorito).HasColumnName("fk_cd_favorito");

                entity.HasOne(d => d.FkCdProdutoNavigation)
                    .WithMany(p => p.TbItemFavorito)
                    .HasForeignKey(d => d.FkCdProduto)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("tb_item_favorito_ibfk_1");
            });

            modelBuilder.Entity<TbMercante>(entity =>
            {
                entity.HasKey(e => e.CdMercante)
                    .HasName("PRIMARY");

                entity.ToTable("tb_mercante");

                entity.HasIndex(e => e.FkCdCliente, "fk_cd_cliente");

                entity.Property(e => e.CdMercante).HasColumnName("cd_mercante");

                entity.Property(e => e.DsLoja).HasColumnName("ds_loja");

                entity.Property(e => e.FkCdCliente).HasColumnName("fk_cd_cliente");

                entity.Property(e => e.ImgLogo)
                    .HasColumnType("blob")
                    .HasColumnName("img_logo");

                entity.Property(e => e.NmLoja)
                    .HasMaxLength(30)
                    .HasColumnName("nm_loja");

                entity.HasOne(d => d.FkCdClienteNavigation)
                    .WithMany(p => p.TbMercante)
                    .HasForeignKey(d => d.FkCdCliente)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("tb_mercante_ibfk_1");
            });

            modelBuilder.Entity<TbProduto>(entity =>
            {
                entity.HasKey(e => e.CdProduto)
                    .HasName("PRIMARY");

                entity.ToTable("tb_produto");

                entity.HasIndex(e => e.FkCdMercante, "fk_cd_mercante");

                entity.Property(e => e.CdProduto).HasColumnName("cd_produto");

                entity.Property(e => e.DsProduto).HasColumnName("ds_produto");

                entity.Property(e => e.FkCdMercante).HasColumnName("fk_cd_mercante");

                entity.Property(e => e.NmProduto)
                    .HasMaxLength(30)
                    .HasColumnName("nm_produto");

                entity.Property(e => e.QtEstoque).HasColumnName("qt_estoque");

                entity.Property(e => e.VlProduto)
                    .HasPrecision(8, 2)
                    .HasColumnName("vl_produto");

                entity.HasOne(d => d.FkCdMercanteNavigation)
                    .WithMany(p => p.TbProduto)
                    .HasForeignKey(d => d.FkCdMercante)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("tb_produto_ibfk_1");
            });

            modelBuilder.Entity<TbProdutoImagem>(entity =>
            {
                entity.HasKey(e => e.CdProdutoImagem)
                    .HasName("PRIMARY");

                entity.ToTable("tb_produto_imagem");

                entity.HasIndex(e => e.FkCdProduto, "fk_cd_produto");

                entity.Property(e => e.CdProdutoImagem).HasColumnName("cd_produto_imagem");

                entity.Property(e => e.DsImagemProduto)
                    .HasMaxLength(80)
                    .HasColumnName("ds_imagem_produto");

                entity.Property(e => e.FkCdProduto).HasColumnName("fk_cd_produto");

                entity.Property(e => e.ImgProduto)
                    .HasColumnType("blob")
                    .HasColumnName("img_produto");

                entity.HasOne(d => d.FkCdProdutoNavigation)
                    .WithMany(p => p.TbProdutoImagem)
                    .HasForeignKey(d => d.FkCdProduto)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("tb_produto_imagem_ibfk_1");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
