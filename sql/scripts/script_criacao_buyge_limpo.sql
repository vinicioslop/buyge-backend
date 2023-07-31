DROP DATABASE bdbuyge;
CREATE DATABASE bdbuyge;
USE bdbuyge;

CREATE TABLE
  tb_cliente (
    cd_cliente INT NOT NULL AUTO_INCREMENT,
    nm_cliente VARCHAR(50) NOT NULL,
    nm_sobrenome VARCHAR(50) NULL,
    nr_cpf CHAR(11) NULL,
    dt_nascimento DATE NOT NULL,
    nr_telefone CHAR(11) NULL,
    nm_email VARCHAR(100) NOT NULL UNIQUE,
    nm_senha VARCHAR(16) NOT NULL,
    nm_tipo_conta VARCHAR(12) NOT NULL,
    PRIMARY KEY (cd_cliente)
  );

CREATE TABLE
  tb_endereco (
    cd_endereco INT NOT NULL AUTO_INCREMENT,
    nm_logradouro VARCHAR(30) NOT NULL,
    nr_endereco BIGINT NOT NULL,
    nm_bairro VARCHAR(30) NOT NULL,
    nr_cep CHAR(8) NOT NULL,
    nm_cidade VARCHAR(30) NOT NULL,
    sg_estado CHAR(2) NOT NULL,
    nm_titulo_endereco VARCHAR(50) NOT NULL,
    nm_tipo_endereco VARCHAR(45) NOT NULL,
    id_principal BIT NOT NULL,
    fk_cd_cliente INT NOT NULL,
    PRIMARY KEY (cd_endereco),
    FOREIGN KEY (fk_cd_cliente) REFERENCES tb_cliente (cd_cliente)
  );

CREATE TABLE
  tb_mercante (
    cd_mercante INT NOT NULL AUTO_INCREMENT,
    nm_loja VARCHAR(100) NOT NULL,
    ds_loja LONGTEXT NOT NULL,
    nm_email VARCHAR(100) NOT NULL,
    img_logo_link MEDIUMTEXT NULL,
    img_logo LONGBLOB NULL,
    nr_cnpj CHAR(14) NULL,
    nr_telefone_fixo CHAR(10) NULL,
    nr_telefone_celular CHAR(11) NULL,
    fk_cd_cliente INT NOT NULL,
    PRIMARY KEY (cd_mercante),
    FOREIGN KEY (fk_cd_cliente) REFERENCES tb_cliente (cd_cliente)
  );

CREATE TABLE
  tb_categoria (
    cd_categoria INT NOT NULL AUTO_INCREMENT,
    nm_categoria VARCHAR(30) NOT NULL,
    ds_categoria LONGTEXT NOT NULL,
    PRIMARY KEY (cd_categoria)
  );

CREATE TABLE
  tb_produto (
    cd_produto INT NOT NULL AUTO_INCREMENT,
    nm_produto VARCHAR(40) NOT NULL,
    ds_produto LONGTEXT NOT NULL,
    vl_produto DECIMAL(8, 2) NOT NULL,
    qt_produto INT NOT NULL,
    vl_peso DECIMAL(6, 3) NULL,
    vl_tamanho INT NULL,
    vl_frete DECIMAL(6, 2) NULL,
    id_disponibilidade BIT NOT NULL,
    dt_criacao DATE NOT NULL,
    fk_cd_mercante INT NOT NULL,
    fk_cd_categoria INT NOT NULL,
    PRIMARY KEY (cd_produto),
    FOREIGN KEY (fk_cd_mercante) REFERENCES tb_mercante (cd_mercante),
    FOREIGN KEY (fk_cd_categoria) REFERENCES tb_categoria (cd_categoria)
  );

CREATE TABLE
  tb_produto_imagem (
    cd_produto_imagem INT NOT NULL AUTO_INCREMENT,
    img_produto_link MEDIUMTEXT NULL,
    img_produto LONGBLOB NULL,
    alt_imagem_produto TINYTEXT NOT NULL,
    id_principal BIT NOT NULL,
    fk_cd_produto INT NOT NULL,
    PRIMARY KEY (cd_produto_imagem),
    FOREIGN KEY (fk_cd_produto) REFERENCES tb_produto (cd_produto)
  );

CREATE TABLE
  tb_item_favorito (
    cd_item_favorito INT NOT NULL AUTO_INCREMENT,
    fk_cd_produto INT NOT NULL,
    fk_cd_cliente INT NOT NULL,
    PRIMARY KEY (cd_item_favorito),
    FOREIGN KEY (fk_cd_produto) REFERENCES tb_produto (cd_produto),
    FOREIGN KEY (fk_cd_cliente) REFERENCES tb_cliente (cd_cliente)
  );

CREATE TABLE
  tb_compra (
    cd_compra INT NOT NULL AUTO_INCREMENT,
    vl_total_compra DECIMAL(8, 2) NOT NULL,
    vl_total_frete DECIMAL(6, 2) NULL,
    vl_total_desconto DECIMAL(6, 2) NULL,
    id_preferencia VARCHAR(100) NOT NULL,
    nm_collection_status VARCHAR(20) NOT NULL,
    nm_collection_id VARCHAR(30) NOT NULL,
    nm_payment_id VARCHAR(30) NOT NULL,
    nm_status VARCHAR(30) NOT NULL,
    nm_payment_type VARCHAR(30) NOT NULL,
    nm_merchant_order_id VARCHAR(30) NOT NULL,
    fk_cd_cliente INT NOT NULL,
    PRIMARY KEY (cd_compra),
    FOREIGN KEY (fk_cd_cliente) REFERENCES tb_cliente (cd_cliente)
  );

CREATE TABLE
  tb_item_compra (
    cd_item_compra INT NOT NULL AUTO_INCREMENT,
    nm_produto VARCHAR(40) NOT NULL,
    vl_item_compra DECIMAL(8, 2) NOT NULL,
    ds_produto LONGTEXT NOT NULL,
    qt_item_compra INT NOT NULL,
    fk_cd_compra INT NOT NULL,
    PRIMARY KEY (cd_item_compra),
    FOREIGN KEY (fk_cd_compra) REFERENCES tb_compra (cd_compra)
  );

CREATE TABLE tb_endereco_loja (
    cd_endereco INT NOT NULL AUTO_INCREMENT,
    nm_logradouro VARCHAR(30) NOT NULL,
    nr_endereco BIGINT NOT NULL,
    nm_bairro VARCHAR(30) NOT NULL,
    nr_cep CHAR(8) NOT NULL,
    nm_cidade VARCHAR(30) NOT NULL,
    sg_estado CHAR(2) NOT NULL,
    fk_cd_mercante INT NOT NULL,
    PRIMARY KEY (cd_endereco),
    FOREIGN KEY (fk_cd_mercante)
        REFERENCES tb_mercante (cd_mercante)
);

CREATE TABLE
  tb_item_carrinho (
    cd_item_carrinho INT NOT NULL AUTO_INCREMENT,
    qt_item_carrinho INT NOT NULL,
    fk_cd_produto INT NOT NULL,
    fk_cd_cliente INT NOT NULL,
    PRIMARY KEY (cd_item_carrinho),
    FOREIGN KEY (fk_cd_produto) REFERENCES tb_produto (cd_produto),
    FOREIGN KEY (fk_cd_cliente) REFERENCES tb_cliente (cd_cliente)
  );