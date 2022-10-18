﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace buyge_backend.db
{
    public partial class bdbuygeContext : DbContext
    {
        public bdbuygeContext()
        {
        }

        public bdbuygeContext(DbContextOptions<bdbuygeContext> options)
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

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("utf8_general_ci")
                .HasCharSet("utf8");

            modelBuilder.Entity<TbCarrinho>(entity =>
            {
                entity.HasKey(e => e.CdCarrinho)
                    .HasName("PRIMARY");

                entity.ToTable("tb_carrinho");

                entity.HasIndex(e => e.FkCdCliente, "fk_tb_carrinho_tb_cliente1_idx");

                entity.Property(e => e.CdCarrinho).HasColumnName("cd_carrinho");

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

                entity.HasIndex(e => e.FkCdCliente, "fk_tb_compra_tb_cliente1_idx");

                entity.Property(e => e.CdCompra).HasColumnName("cd_compra");

                entity.Property(e => e.FkCdCliente).HasColumnName("fk_cd_cliente");

                entity.Property(e => e.VlTotalCompra)
                    .HasPrecision(8, 2)
                    .HasColumnName("vl_total_compra");

                entity.HasOne(d => d.FkCdClienteNavigation)
                    .WithMany(p => p.TbCompra)
                    .HasForeignKey(d => d.FkCdCliente)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_tb_compra_tb_cliente1");
            });

            modelBuilder.Entity<TbEndereco>(entity =>
            {
                entity.HasKey(e => e.CdEndereco)
                    .HasName("PRIMARY");

                entity.ToTable("tb_endereco");

                entity.HasIndex(e => e.FkCdCliente, "fk_tb_endereco_tb_cliente1_idx");

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
                    .HasConstraintName("fk_tb_endereco_tb_cliente1");
            });

            modelBuilder.Entity<TbFavorito>(entity =>
            {
                entity.HasKey(e => e.CdFavorito)
                    .HasName("PRIMARY");

                entity.ToTable("tb_favorito");

                entity.HasIndex(e => e.FkCdCliente, "fk_tb_favorito_tb_cliente1_idx");

                entity.Property(e => e.CdFavorito).HasColumnName("cd_favorito");

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

                entity.HasIndex(e => e.FkCdCarrinho, "fk_tb_produto_has_tb_carrinho_tb_carrinho1_idx");

                entity.HasIndex(e => e.FkCdProduto, "fk_tb_produto_has_tb_carrinho_tb_produto1_idx");

                entity.Property(e => e.CdItemCarrinho).HasColumnName("cd_item_carrinho");

                entity.Property(e => e.FkCdCarrinho).HasColumnName("fk_cd_carrinho");

                entity.Property(e => e.FkCdProduto).HasColumnName("fk_cd_produto");

                entity.HasOne(d => d.FkCdCarrinhoNavigation)
                    .WithMany(p => p.TbItemCarrinho)
                    .HasForeignKey(d => d.FkCdCarrinho)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_tb_produto_has_tb_carrinho_tb_carrinho1");

                entity.HasOne(d => d.FkCdProdutoNavigation)
                    .WithMany(p => p.TbItemCarrinho)
                    .HasForeignKey(d => d.FkCdProduto)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_tb_produto_has_tb_carrinho_tb_produto1");
            });

            modelBuilder.Entity<TbItemCompra>(entity =>
            {
                entity.HasKey(e => e.CdItemCompra)
                    .HasName("PRIMARY");

                entity.ToTable("tb_item_compra");

                entity.HasIndex(e => e.FkCdCompra, "fk_tb_produto_has_tb_compra_tb_compra1_idx");

                entity.HasIndex(e => e.FkCdProduto, "fk_tb_produto_has_tb_compra_tb_produto1_idx");

                entity.Property(e => e.CdItemCompra).HasColumnName("cd_item_compra");

                entity.Property(e => e.FkCdCompra).HasColumnName("fk_cd_compra");

                entity.Property(e => e.FkCdProduto).HasColumnName("fk_cd_produto");

                entity.Property(e => e.VlItemCompra)
                    .HasPrecision(8, 2)
                    .HasColumnName("vl_item_compra");

                entity.HasOne(d => d.FkCdCompraNavigation)
                    .WithMany(p => p.TbItemCompra)
                    .HasForeignKey(d => d.FkCdCompra)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_tb_produto_has_tb_compra_tb_compra1");

                entity.HasOne(d => d.FkCdProdutoNavigation)
                    .WithMany(p => p.TbItemCompra)
                    .HasForeignKey(d => d.FkCdProduto)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_tb_produto_has_tb_compra_tb_produto1");
            });

            modelBuilder.Entity<TbItemFavorito>(entity =>
            {
                entity.HasKey(e => e.CdItemFavorito)
                    .HasName("PRIMARY");

                entity.ToTable("tb_item_favorito");

                entity.HasIndex(e => e.FkCdFavorito, "fk_tb_produto_has_tb_favorito_tb_favorito1_idx");

                entity.HasIndex(e => e.FkCdProduto, "fk_tb_produto_has_tb_favorito_tb_produto1_idx");

                entity.Property(e => e.CdItemFavorito).HasColumnName("cd_item_favorito");

                entity.Property(e => e.FkCdFavorito).HasColumnName("fk_cd_favorito");

                entity.Property(e => e.FkCdProduto).HasColumnName("fk_cd_produto");

                entity.HasOne(d => d.FkCdFavoritoNavigation)
                    .WithMany(p => p.TbItemFavorito)
                    .HasForeignKey(d => d.FkCdFavorito)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_tb_produto_has_tb_favorito_tb_favorito1");

                entity.HasOne(d => d.FkCdProdutoNavigation)
                    .WithMany(p => p.TbItemFavorito)
                    .HasForeignKey(d => d.FkCdProduto)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_tb_produto_has_tb_favorito_tb_produto1");
            });

            modelBuilder.Entity<TbMercante>(entity =>
            {
                entity.HasKey(e => e.CdMercante)
                    .HasName("PRIMARY");

                entity.ToTable("tb_mercante");

                entity.HasIndex(e => e.FkCdCliente, "fk_tb_mercante_tb_cliente1_idx");

                entity.Property(e => e.CdMercante).HasColumnName("cd_mercante");

                entity.Property(e => e.DsLoja).HasColumnName("ds_loja");

                entity.Property(e => e.FkCdCliente).HasColumnName("fk_cd_cliente");

                entity.Property(e => e.ImgLogo)
                    .HasColumnType("mediumtext")
                    .HasColumnName("img_logo");

                entity.Property(e => e.NmLoja)
                    .HasMaxLength(30)
                    .HasColumnName("nm_loja");

                entity.Property(e => e.NrCnpj)
                    .HasMaxLength(14)
                    .HasColumnName("nr_cnpj")
                    .IsFixedLength();

                entity.HasOne(d => d.FkCdClienteNavigation)
                    .WithMany(p => p.TbMercante)
                    .HasForeignKey(d => d.FkCdCliente)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_tb_mercante_tb_cliente1");
            });

            modelBuilder.Entity<TbProduto>(entity =>
            {
                entity.HasKey(e => e.CdProduto)
                    .HasName("PRIMARY");

                entity.ToTable("tb_produto");

                entity.HasIndex(e => e.FkCdCategoria, "fk_tb_produto_tb_categoria1_idx");

                entity.HasIndex(e => e.FkCdMercante, "fk_tb_produto_tb_mercante1_idx");

                entity.Property(e => e.CdProduto).HasColumnName("cd_produto");

                entity.Property(e => e.DsProduto).HasColumnName("ds_produto");

                entity.Property(e => e.FkCdCategoria).HasColumnName("fk_cd_categoria");

                entity.Property(e => e.FkCdMercante).HasColumnName("fk_cd_mercante");

                entity.Property(e => e.NmProduto)
                    .HasMaxLength(30)
                    .HasColumnName("nm_produto");

                entity.Property(e => e.QtProduto).HasColumnName("qt_produto");

                entity.Property(e => e.VlProduto)
                    .HasPrecision(8, 2)
                    .HasColumnName("vl_produto");

                entity.HasOne(d => d.FkCdCategoriaNavigation)
                    .WithMany(p => p.TbProduto)
                    .HasForeignKey(d => d.FkCdCategoria)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_tb_produto_tb_categoria1");

                entity.HasOne(d => d.FkCdMercanteNavigation)
                    .WithMany(p => p.TbProduto)
                    .HasForeignKey(d => d.FkCdMercante)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_tb_produto_tb_mercante1");
            });

            modelBuilder.Entity<TbProdutoImagem>(entity =>
            {
                entity.HasKey(e => e.CdProdutoImagem)
                    .HasName("PRIMARY");

                entity.ToTable("tb_produto_imagem");

                entity.HasIndex(e => e.FkCdProduto, "fk_tb_produto_imagem_tb_produto1_idx");

                entity.Property(e => e.CdProdutoImagem).HasColumnName("cd_produto_imagem");

                entity.Property(e => e.DsImagemProduto).HasColumnName("ds_imagem_produto");

                entity.Property(e => e.FkCdProduto).HasColumnName("fk_cd_produto");

                entity.Property(e => e.ImgProduto)
                    .HasColumnType("mediumtext")
                    .HasColumnName("img_produto");

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
