CREATE DATABASE bd_buyge;

USE bd_buyge;

CREATE TABLE tb_cliente (
  cd_cliente INT NOT NULL AUTO_INCREMENT,
  nm_cliente VARCHAR(40) NOT NULL,
  nm_sobrenome VARCHAR(60) NOT NULL,
  dt_nascimento DATE NOT NULL,
  nr_telefone CHAR(11) NOT NULL,
  nm_login VARCHAR(30) NOT NULL,
  nm_senha VARCHAR(16) NOT NULL,
  PRIMARY KEY (cd_cliente)
);

CREATE TABLE tb_endereco (
  cd_endereco INT NOT NULL AUTO_INCREMENT,
  nm_logradouro VARCHAR(30) NOT NULL,
  nr_endereco BIGINT NOT NULL,
  nm_bairro VARCHAR(30) NOT NULL,
  nr_cep CHAR(8) NOT NULL,
  nm_cidade VARCHAR(30) NOT NULL,
  sg_estado CHAR(2) NOT NULL,
  fk_cd_cliente INT NOT NULL,
  PRIMARY KEY (cd_endereco),
  FOREIGN KEY (fk_cd_cliente) REFERENCES tb_cliente (cd_cliente)
);

CREATE TABLE tb_mercante (
  cd_mercante INT NOT NULL AUTO_INCREMENT,
  nm_loja VARCHAR(30) NOT NULL,
  ds_loja LONGTEXT NOT NULL,
  img_logo BLOB NOT NULL,
  fk_cd_cliente INT NOT NULL,
  PRIMARY KEY (cd_mercante),
  FOREIGN KEY (fk_cd_cliente) REFERENCES tb_cliente (cd_cliente)
);

CREATE TABLE tb_carrinho (
  cd_carrinho INT NOT NULL AUTO_INCREMENT,
  fk_cd_cliente INT NOT NULL,
  PRIMARY KEY (cd_carrinho, fk_cd_cliente),
  FOREIGN KEY (fk_cd_cliente) REFERENCES tb_cliente (cd_cliente)
);

CREATE TABLE tb_favorito (
  cd_favorito INT NOT NULL AUTO_INCREMENT,
  fk_cd_cliente INT NOT NULL,
  PRIMARY KEY (cd_favorito, fk_cd_cliente),
  FOREIGN KEY (fk_cd_cliente) REFERENCES tb_cliente (cd_cliente)
);

CREATE TABLE tb_produto (
  cd_produto INT NOT NULL AUTO_INCREMENT,
  nm_produto VARCHAR(30) NOT NULL,
  ds_produto LONGTEXT NOT NULL,
  vl_produto DECIMAL(8, 2) NOT NULL,
  qt_estoque INT NOT NULL,
  fk_cd_mercante INT NOT NULL,
  PRIMARY KEY (cd_produto),
  FOREIGN KEY (fk_cd_mercante) REFERENCES tb_mercante (cd_mercante)
);

CREATE TABLE tb_categoria (
  cd_categoria INT NOT NULL AUTO_INCREMENT,
  nm_categoria VARCHAR(30) NOT NULL,
  ds_categoria LONGTEXT NOT NULL,
  PRIMARY KEY (cd_categoria)
);

CREATE TABLE tb_produto_imagem (
  cd_produto_imagem INT NOT NULL AUTO_INCREMENT,
  img_produto BLOB NOT NULL,
  ds_imagem_produto VARCHAR(80) NOT NULL,
  fk_cd_produto INT NOT NULL,
  PRIMARY KEY (cd_produto_imagem),
  FOREIGN KEY (fk_cd_produto) REFERENCES tb_produto (cd_produto)
);

CREATE TABLE tb_produto_categoria (
  fk_cd_categoria INT NOT NULL,
  fk_cd_produto INT NOT NULL,
  PRIMARY KEY (fk_cd_categoria, fk_cd_produto),
  FOREIGN KEY (fk_cd_categoria) REFERENCES tb_categoria (cd_categoria),
  FOREIGN KEY (fk_cd_produto) REFERENCES tb_produto (cd_produto)
);

CREATE TABLE tb_item_favorito (
  fk_cd_produto INT NOT NULL,
  fk_cd_favorito INT NOT NULL,
  PRIMARY KEY (fk_cd_produto, fk_cd_favorito),
  FOREIGN KEY (fk_cd_produto) REFERENCES tb_produto (cd_produto),
  FOREIGN KEY (fk_cd_favorito) REFERENCES tb_favorito (cd_favorito)
);

CREATE TABLE tb_item_carrinho (
  fk_cd_produto INT NOT NULL,
  fk_cd_carrinho INT NOT NULL,
  PRIMARY KEY (fk_cd_produto, fk_cd_carrinho),
  FOREIGN KEY (fk_cd_produto) REFERENCES tb_produto (cd_produto),
  FOREIGN KEY (fk_cd_carrinho) REFERENCES tb_carrinho (cd_carrinho)
);

CREATE TABLE tb_compra (
  cd_compra INT NOT NULL AUTO_INCREMENT,
  vl_total_compra VARCHAR(45) NOT NULL,
  tb_cliente_cd_cliente INT NOT NULL,
  PRIMARY KEY (cd_compra),
  FOREIGN KEY (tb_cliente_cd_cliente) REFERENCES tb_cliente (cd_cliente)
);

CREATE TABLE tb_item_compra (
  fk_cd_produto INT NOT NULL,
  fk_cd_compra INT NOT NULL,
  vl_item_compra DECIMAL(8, 2) NOT NULL,
  PRIMARY KEY (fk_cd_produto, fk_cd_compra),
  FOREIGN KEY (fk_cd_produto) REFERENCES tb_produto (cd_produto),
  FOREIGN KEY (fk_cd_compra) REFERENCES tb_compra (cd_compra)
);