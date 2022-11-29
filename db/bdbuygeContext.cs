using Microsoft.EntityFrameworkCore;

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

        public virtual DbSet<TbCategoria> TbCategoria { get; set; } = null!;
        public virtual DbSet<TbCliente> TbCliente { get; set; } = null!;
        public virtual DbSet<TbCompra> TbCompra { get; set; } = null!;
        public virtual DbSet<TbEndereco> TbEndereco { get; set; } = null!;
        public virtual DbSet<TbEnderecoLoja> TbEnderecoLoja { get; set; } = null!;
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
            modelBuilder.UseCollation("utf8mb4_general_ci")
                .HasCharSet("utf8mb4");

            modelBuilder.Entity<TbCategoria>(entity =>
            {
                entity.HasKey(e => e.CdCategoria)
                    .HasName("PRIMARY");

                entity.ToTable("tb_categoria");

                entity.Property(e => e.CdCategoria)
                    .HasColumnType("int(11)")
                    .HasColumnName("cd_categoria");

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

                entity.HasIndex(e => e.NmEmail, "nm_email")
                    .IsUnique();

                entity.Property(e => e.CdCliente)
                    .HasColumnType("int(11)")
                    .HasColumnName("cd_cliente");

                entity.Property(e => e.DtNascimento).HasColumnName("dt_nascimento");

                entity.Property(e => e.NmCliente)
                    .HasMaxLength(50)
                    .HasColumnName("nm_cliente");

                entity.Property(e => e.NmEmail)
                    .HasMaxLength(100)
                    .HasColumnName("nm_email");

                entity.Property(e => e.NmSenha)
                    .HasMaxLength(16)
                    .HasColumnName("nm_senha");

                entity.Property(e => e.NmSobrenome)
                    .HasMaxLength(50)
                    .HasColumnName("nm_sobrenome");

                entity.Property(e => e.NmTipoConta)
                    .HasMaxLength(12)
                    .HasColumnName("nm_tipo_conta");

                entity.Property(e => e.NrCpf)
                    .HasMaxLength(11)
                    .HasColumnName("nr_cpf")
                    .IsFixedLength();

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

                entity.HasIndex(e => e.FkCdCliente, "fk_cd_cliente");

                entity.Property(e => e.CdCompra)
                    .HasColumnType("int(11)")
                    .HasColumnName("cd_compra");

                entity.Property(e => e.FkCdCliente)
                    .HasColumnType("int(11)")
                    .HasColumnName("fk_cd_cliente");

                entity.Property(e => e.VlTotalCompra)
                    .HasPrecision(8, 2)
                    .HasColumnName("vl_total_compra");

                entity.HasOne(d => d.FkCdClienteNavigation)
                    .WithMany(p => p.TbCompra)
                    .HasForeignKey(d => d.FkCdCliente)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("tb_compra_ibfk_1");
            });

            modelBuilder.Entity<TbEndereco>(entity =>
            {
                entity.HasKey(e => e.CdEndereco)
                    .HasName("PRIMARY");

                entity.ToTable("tb_endereco");

                entity.HasIndex(e => e.FkCdCliente, "fk_cd_cliente");

                entity.Property(e => e.CdEndereco)
                    .HasColumnType("int(11)")
                    .HasColumnName("cd_endereco");

                entity.Property(e => e.FkCdCliente)
                    .HasColumnType("int(11)")
                    .HasColumnName("fk_cd_cliente");

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

                entity.Property(e => e.NrEndereco)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("nr_endereco");

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

            modelBuilder.Entity<TbEnderecoLoja>(entity =>
            {
                entity.HasKey(e => e.CdEndereco)
                    .HasName("PRIMARY");

                entity.ToTable("tb_endereco_loja");

                entity.HasIndex(e => e.FkCdMercante, "fk_cd_mercante");

                entity.Property(e => e.CdEndereco)
                    .HasColumnType("int(11)")
                    .HasColumnName("cd_endereco");

                entity.Property(e => e.FkCdMercante)
                    .HasColumnType("int(11)")
                    .HasColumnName("fk_cd_mercante");

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

                entity.Property(e => e.NrEndereco)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("nr_endereco");

                entity.Property(e => e.SgEstado)
                    .HasMaxLength(2)
                    .HasColumnName("sg_estado")
                    .IsFixedLength();

                entity.HasOne(d => d.FkCdMercanteNavigation)
                    .WithMany(p => p.TbEnderecoLoja)
                    .HasForeignKey(d => d.FkCdMercante)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("tb_endereco_loja_ibfk_1");
            });

            modelBuilder.Entity<TbItemCarrinho>(entity =>
            {
                entity.HasKey(e => e.CdItemCarrinho)
                    .HasName("PRIMARY");

                entity.ToTable("tb_item_carrinho");

                entity.HasIndex(e => e.FkCdCliente, "fk_cd_cliente");

                entity.HasIndex(e => e.FkCdProduto, "fk_cd_produto");

                entity.Property(e => e.CdItemCarrinho)
                    .HasColumnType("int(11)")
                    .HasColumnName("cd_item_carrinho");

                entity.Property(e => e.FkCdCliente)
                    .HasColumnType("int(11)")
                    .HasColumnName("fk_cd_cliente");

                entity.Property(e => e.FkCdProduto)
                    .HasColumnType("int(11)")
                    .HasColumnName("fk_cd_produto");

                entity.HasOne(d => d.FkCdClienteNavigation)
                    .WithMany(p => p.TbItemCarrinho)
                    .HasForeignKey(d => d.FkCdCliente)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("tb_item_carrinho_ibfk_2");

                entity.HasOne(d => d.FkCdProdutoNavigation)
                    .WithMany(p => p.TbItemCarrinho)
                    .HasForeignKey(d => d.FkCdProduto)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("tb_item_carrinho_ibfk_1");
            });

            modelBuilder.Entity<TbItemCompra>(entity =>
            {
                entity.HasKey(e => e.CdItemCompra)
                    .HasName("PRIMARY");

                entity.ToTable("tb_item_compra");

                entity.HasIndex(e => e.FkCdCompra, "fk_cd_compra");

                entity.Property(e => e.CdItemCompra)
                    .HasColumnType("int(11)")
                    .HasColumnName("cd_item_compra");

                entity.Property(e => e.DsProduto).HasColumnName("ds_produto");

                entity.Property(e => e.FkCdCompra)
                    .HasColumnType("int(11)")
                    .HasColumnName("fk_cd_compra");

                entity.Property(e => e.NmProduto)
                    .HasMaxLength(40)
                    .HasColumnName("nm_produto");

                entity.Property(e => e.VlItemCompra)
                    .HasPrecision(8, 2)
                    .HasColumnName("vl_item_compra");

                entity.HasOne(d => d.FkCdCompraNavigation)
                    .WithMany(p => p.TbItemCompra)
                    .HasForeignKey(d => d.FkCdCompra)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("tb_item_compra_ibfk_1");
            });

            modelBuilder.Entity<TbItemFavorito>(entity =>
            {
                entity.HasKey(e => e.CdItemFavorito)
                    .HasName("PRIMARY");

                entity.ToTable("tb_item_favorito");

                entity.HasIndex(e => e.FkCdCliente, "fk_cd_cliente");

                entity.HasIndex(e => e.FkCdProduto, "fk_cd_produto");

                entity.Property(e => e.CdItemFavorito)
                    .HasColumnType("int(11)")
                    .HasColumnName("cd_item_favorito");

                entity.Property(e => e.FkCdCliente)
                    .HasColumnType("int(11)")
                    .HasColumnName("fk_cd_cliente");

                entity.Property(e => e.FkCdProduto)
                    .HasColumnType("int(11)")
                    .HasColumnName("fk_cd_produto");

                entity.HasOne(d => d.FkCdClienteNavigation)
                    .WithMany(p => p.TbItemFavorito)
                    .HasForeignKey(d => d.FkCdCliente)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("tb_item_favorito_ibfk_2");

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

                entity.Property(e => e.CdMercante)
                    .HasColumnType("int(11)")
                    .HasColumnName("cd_mercante");

                entity.Property(e => e.DsLoja).HasColumnName("ds_loja");

                entity.Property(e => e.FkCdCliente)
                    .HasColumnType("int(11)")
                    .HasColumnName("fk_cd_cliente");

                entity.Property(e => e.ImgLogo).HasColumnName("img_logo");

                entity.Property(e => e.ImgLogoLink)
                    .HasColumnType("mediumtext")
                    .HasColumnName("img_logo_link");

                entity.Property(e => e.NmEmail)
                    .HasMaxLength(100)
                    .HasColumnName("nm_email");

                entity.Property(e => e.NmLoja)
                    .HasMaxLength(100)
                    .HasColumnName("nm_loja");

                entity.Property(e => e.NrCnpj)
                    .HasMaxLength(14)
                    .HasColumnName("nr_cnpj")
                    .IsFixedLength();

                entity.Property(e => e.NrTelefoneCelular)
                    .HasMaxLength(11)
                    .HasColumnName("nr_telefone_celular")
                    .IsFixedLength();

                entity.Property(e => e.NrTelefoneFixo)
                    .HasMaxLength(10)
                    .HasColumnName("nr_telefone_fixo")
                    .IsFixedLength();

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

                entity.HasIndex(e => e.FkCdCategoria, "fk_cd_categoria");

                entity.HasIndex(e => e.FkCdMercante, "fk_cd_mercante");

                entity.Property(e => e.CdProduto)
                    .HasColumnType("int(11)")
                    .HasColumnName("cd_produto");

                entity.Property(e => e.DsProduto).HasColumnName("ds_produto");

                entity.Property(e => e.DtCriacao).HasColumnName("dt_criacao");

                entity.Property(e => e.FkCdCategoria)
                    .HasColumnType("int(11)")
                    .HasColumnName("fk_cd_categoria");

                entity.Property(e => e.FkCdMercante)
                    .HasColumnType("int(11)")
                    .HasColumnName("fk_cd_mercante");

                entity.Property(e => e.IdDisponibilidade)
                    .HasColumnType("bit(1)")
                    .HasColumnName("id_disponibilidade");

                entity.Property(e => e.NmProduto)
                    .HasMaxLength(40)
                    .HasColumnName("nm_produto");

                entity.Property(e => e.QtProduto)
                    .HasColumnType("int(11)")
                    .HasColumnName("qt_produto");

                entity.Property(e => e.VlFrete)
                    .HasPrecision(6, 2)
                    .HasColumnName("vl_frete");

                entity.Property(e => e.VlPeso)
                    .HasPrecision(6, 3)
                    .HasColumnName("vl_peso");

                entity.Property(e => e.VlProduto)
                    .HasPrecision(8, 2)
                    .HasColumnName("vl_produto");

                entity.Property(e => e.VlTamanho)
                    .HasColumnType("int(11)")
                    .HasColumnName("vl_tamanho");

                entity.HasOne(d => d.FkCdCategoriaNavigation)
                    .WithMany(p => p.TbProduto)
                    .HasForeignKey(d => d.FkCdCategoria)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("tb_produto_ibfk_2");

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

                entity.Property(e => e.CdProdutoImagem)
                    .HasColumnType("int(11)")
                    .HasColumnName("cd_produto_imagem");

                entity.Property(e => e.AltImagemProduto)
                    .HasColumnType("tinytext")
                    .HasColumnName("alt_imagem_produto");

                entity.Property(e => e.FkCdProduto)
                    .HasColumnType("int(11)")
                    .HasColumnName("fk_cd_produto");

                entity.Property(e => e.IdPrincipal)
                    .HasColumnType("bit(1)")
                    .HasColumnName("id_principal");

                entity.Property(e => e.ImgProduto).HasColumnName("img_produto");

                entity.Property(e => e.ImgProdutoLink)
                    .HasColumnType("mediumtext")
                    .HasColumnName("img_produto_link");

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
