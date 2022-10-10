using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace buyge_backend.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8");

            migrationBuilder.CreateTable(
                name: "tb_categoria",
                columns: table => new
                {
                    cd_categoria = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    nm_categoria = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    ds_categoria = table.Column<string>(type: "longtext", nullable: false, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.cd_categoria);
                })
                .Annotation("MySql:CharSet", "utf8")
                .Annotation("Relational:Collation", "utf8_general_ci");

            migrationBuilder.CreateTable(
                name: "tb_cliente",
                columns: table => new
                {
                    cd_cliente = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    nm_cliente = table.Column<string>(type: "varchar(40)", maxLength: 40, nullable: false, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    nm_sobrenome = table.Column<string>(type: "varchar(60)", maxLength: 60, nullable: false, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    dt_nascimento = table.Column<DateTime>(type: "date", nullable: false),
                    nr_telefone = table.Column<string>(type: "char(11)", fixedLength: true, maxLength: 11, nullable: false, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    nm_login = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    nm_senha = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: false, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.cd_cliente);
                })
                .Annotation("MySql:CharSet", "utf8")
                .Annotation("Relational:Collation", "utf8_general_ci");

            migrationBuilder.CreateTable(
                name: "tb_item_carrinho",
                columns: table => new
                {
                    cd_item_carrinho = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    fk_cd_produto = table.Column<int>(type: "int", nullable: false),
                    fk_cd_carrinho = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.cd_item_carrinho);
                })
                .Annotation("MySql:CharSet", "utf8")
                .Annotation("Relational:Collation", "utf8_general_ci");

            migrationBuilder.CreateTable(
                name: "tb_item_favorito",
                columns: table => new
                {
                    cd_item_favorito = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    fk_cd_produto = table.Column<int>(type: "int", nullable: false),
                    fk_cd_favorito = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.cd_item_favorito);
                })
                .Annotation("MySql:CharSet", "utf8")
                .Annotation("Relational:Collation", "utf8_general_ci");

            migrationBuilder.CreateTable(
                name: "tb_produto_imagem",
                columns: table => new
                {
                    cd_produto_imagem = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    img_produto = table.Column<byte[]>(type: "blob", nullable: false),
                    ds_imagem_produto = table.Column<string>(type: "varchar(80)", maxLength: 80, nullable: false, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    fk_cd_produto = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.cd_produto_imagem);
                })
                .Annotation("MySql:CharSet", "utf8")
                .Annotation("Relational:Collation", "utf8_general_ci");

            migrationBuilder.CreateTable(
                name: "tb_carrinho",
                columns: table => new
                {
                    cd_carrinho = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    fk_cd_cliente = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => new { x.cd_carrinho, x.fk_cd_cliente })
                        .Annotation("MySql:IndexPrefixLength", new[] { 0, 0 });
                    table.ForeignKey(
                        name: "fk_tb_carrinho_tb_cliente1",
                        column: x => x.fk_cd_cliente,
                        principalTable: "tb_cliente",
                        principalColumn: "cd_cliente");
                })
                .Annotation("MySql:CharSet", "utf8")
                .Annotation("Relational:Collation", "utf8_general_ci");

            migrationBuilder.CreateTable(
                name: "tb_compra",
                columns: table => new
                {
                    cd_compra = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    vl_total_compra = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: false, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    tb_cliente_cd_cliente = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.cd_compra);
                    table.ForeignKey(
                        name: "fk_tb_compra_tb_cliente1",
                        column: x => x.tb_cliente_cd_cliente,
                        principalTable: "tb_cliente",
                        principalColumn: "cd_cliente");
                })
                .Annotation("MySql:CharSet", "utf8")
                .Annotation("Relational:Collation", "utf8_general_ci");

            migrationBuilder.CreateTable(
                name: "tb_endereco",
                columns: table => new
                {
                    cd_endereco = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    nm_logradouro = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    nr_endereco = table.Column<long>(type: "bigint", nullable: false),
                    nm_bairro = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    nr_cep = table.Column<string>(type: "char(8)", fixedLength: true, maxLength: 8, nullable: false, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    nm_cidade = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    sg_estado = table.Column<string>(type: "char(2)", fixedLength: true, maxLength: 2, nullable: false, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    fk_cd_cliente = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.cd_endereco);
                    table.ForeignKey(
                        name: "fk_tb_endereco_tb_cliente1",
                        column: x => x.fk_cd_cliente,
                        principalTable: "tb_cliente",
                        principalColumn: "cd_cliente");
                })
                .Annotation("MySql:CharSet", "utf8")
                .Annotation("Relational:Collation", "utf8_general_ci");

            migrationBuilder.CreateTable(
                name: "tb_favorito",
                columns: table => new
                {
                    cd_favorito = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    fk_cd_cliente = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => new { x.cd_favorito, x.fk_cd_cliente })
                        .Annotation("MySql:IndexPrefixLength", new[] { 0, 0 });
                    table.ForeignKey(
                        name: "fk_tb_favorito_tb_cliente1",
                        column: x => x.fk_cd_cliente,
                        principalTable: "tb_cliente",
                        principalColumn: "cd_cliente");
                })
                .Annotation("MySql:CharSet", "utf8")
                .Annotation("Relational:Collation", "utf8_general_ci");

            migrationBuilder.CreateTable(
                name: "tb_mercante",
                columns: table => new
                {
                    cd_mercante = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    nm_loja = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    ds_loja = table.Column<string>(type: "longtext", nullable: false, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    img_logo = table.Column<byte[]>(type: "blob", nullable: false),
                    fk_cd_cliente = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.cd_mercante);
                    table.ForeignKey(
                        name: "fk_tb_mercante_tb_cliente1",
                        column: x => x.fk_cd_cliente,
                        principalTable: "tb_cliente",
                        principalColumn: "cd_cliente");
                })
                .Annotation("MySql:CharSet", "utf8")
                .Annotation("Relational:Collation", "utf8_general_ci");

            migrationBuilder.CreateTable(
                name: "tb_item_compra",
                columns: table => new
                {
                    cd_item_compra = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    fk_cd_produto = table.Column<int>(type: "int", nullable: false),
                    fk_cd_compra = table.Column<int>(type: "int", nullable: false),
                    vl_item_compra = table.Column<decimal>(type: "decimal(8,2)", precision: 8, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.cd_item_compra);
                    table.ForeignKey(
                        name: "fk_tb_produto_has_tb_compra_tb_compra1",
                        column: x => x.fk_cd_compra,
                        principalTable: "tb_compra",
                        principalColumn: "cd_compra");
                })
                .Annotation("MySql:CharSet", "utf8")
                .Annotation("Relational:Collation", "utf8_general_ci");

            migrationBuilder.CreateTable(
                name: "tb_produto",
                columns: table => new
                {
                    cd_produto = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    fk_cd_categoria = table.Column<int>(type: "int", nullable: false),
                    nm_produto = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    ds_produto = table.Column<string>(type: "longtext", nullable: false, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    vl_produto = table.Column<decimal>(type: "decimal(8,2)", precision: 8, scale: 2, nullable: false),
                    qt_estoque = table.Column<int>(type: "int", nullable: false),
                    fk_cd_mercante = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => new { x.cd_produto, x.fk_cd_categoria })
                        .Annotation("MySql:IndexPrefixLength", new[] { 0, 0 });
                    table.ForeignKey(
                        name: "fk_tb_produto_tb_categoria1",
                        column: x => x.fk_cd_categoria,
                        principalTable: "tb_categoria",
                        principalColumn: "cd_categoria");
                    table.ForeignKey(
                        name: "fk_tb_produto_tb_mercante1",
                        column: x => x.fk_cd_mercante,
                        principalTable: "tb_mercante",
                        principalColumn: "cd_mercante");
                })
                .Annotation("MySql:CharSet", "utf8")
                .Annotation("Relational:Collation", "utf8_general_ci");

            migrationBuilder.CreateIndex(
                name: "fk_tb_carrinho_tb_cliente1_idx",
                table: "tb_carrinho",
                column: "fk_cd_cliente");

            migrationBuilder.CreateIndex(
                name: "fk_tb_compra_tb_cliente1_idx",
                table: "tb_compra",
                column: "tb_cliente_cd_cliente");

            migrationBuilder.CreateIndex(
                name: "fk_tb_endereco_tb_cliente1_idx",
                table: "tb_endereco",
                column: "fk_cd_cliente");

            migrationBuilder.CreateIndex(
                name: "fk_tb_favorito_tb_cliente1_idx",
                table: "tb_favorito",
                column: "fk_cd_cliente");

            migrationBuilder.CreateIndex(
                name: "fk_tb_produto_has_tb_carrinho_tb_carrinho1_idx",
                table: "tb_item_carrinho",
                column: "fk_cd_carrinho");

            migrationBuilder.CreateIndex(
                name: "fk_tb_produto_has_tb_carrinho_tb_produto1_idx",
                table: "tb_item_carrinho",
                column: "fk_cd_produto");

            migrationBuilder.CreateIndex(
                name: "fk_tb_produto_has_tb_compra_tb_compra1_idx",
                table: "tb_item_compra",
                column: "fk_cd_compra");

            migrationBuilder.CreateIndex(
                name: "fk_tb_produto_has_tb_compra_tb_produto1_idx",
                table: "tb_item_compra",
                column: "fk_cd_produto");

            migrationBuilder.CreateIndex(
                name: "fk_tb_produto_has_tb_favorito_tb_favorito1_idx",
                table: "tb_item_favorito",
                column: "fk_cd_favorito");

            migrationBuilder.CreateIndex(
                name: "fk_tb_produto_has_tb_favorito_tb_produto1_idx",
                table: "tb_item_favorito",
                column: "fk_cd_produto");

            migrationBuilder.CreateIndex(
                name: "fk_tb_mercante_tb_cliente1_idx",
                table: "tb_mercante",
                column: "fk_cd_cliente");

            migrationBuilder.CreateIndex(
                name: "fk_tb_produto_tb_categoria1_idx",
                table: "tb_produto",
                column: "fk_cd_categoria");

            migrationBuilder.CreateIndex(
                name: "fk_tb_produto_tb_mercante1_idx",
                table: "tb_produto",
                column: "fk_cd_mercante");

            migrationBuilder.CreateIndex(
                name: "fk_tb_produto_imagem_tb_produto1_idx",
                table: "tb_produto_imagem",
                column: "fk_cd_produto");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tb_carrinho");

            migrationBuilder.DropTable(
                name: "tb_endereco");

            migrationBuilder.DropTable(
                name: "tb_favorito");

            migrationBuilder.DropTable(
                name: "tb_item_carrinho");

            migrationBuilder.DropTable(
                name: "tb_item_compra");

            migrationBuilder.DropTable(
                name: "tb_item_favorito");

            migrationBuilder.DropTable(
                name: "tb_produto");

            migrationBuilder.DropTable(
                name: "tb_produto_imagem");

            migrationBuilder.DropTable(
                name: "tb_compra");

            migrationBuilder.DropTable(
                name: "tb_categoria");

            migrationBuilder.DropTable(
                name: "tb_mercante");

            migrationBuilder.DropTable(
                name: "tb_cliente");
        }
    }
}
