use bdbuyge;

insert into tb_cliente values (null, "Fulano", "da Silva", "2022-01-01", "13912345678", "login", "senha");
insert into tb_cliente values (null, "Cliclano", "da Silva", "2022-01-01", "13912345678", "login", "senha");
insert into tb_cliente values (null, "Beltrano", "da Silva", "2022-01-01", "13912345678", "login", "senha");
insert into tb_cliente values (null, "Joãozinho", "da Silva", "2022-01-01", "13912345678", "login", "senha");
insert into tb_cliente values (null, "Pedro", "da Silva", "2022-01-01", "13912345678", "login", "senha");

insert into tb_endereco values (null, "Avenida Um", 2200, "Itaóca", "11730000", "Mongaguá", "SP", 1);
insert into tb_endereco values (null, "Avenida Dois", 110, "Jardim Praia Grande", "11730000", "Mongaguá", "SP", 2);
insert into tb_endereco values (null, "Avenida Três", 200, "Santa Eugênia", "11730000", "Mongaguá", "SP", 3);
insert into tb_endereco values (null, "Avenida Quatro", 770, "Jussara", "11730000", "Mongaguá", "SP", 4);
insert into tb_endereco values (null, "Avenida Cinco", 511, "Vera Cruz", "11730000", "Mongaguá", "SP", 5);

insert into tb_mercante values (null, "Bichinho Fofinho", "Loja especializada em bicinhos de pelúcia", "https://images-shoptime.b2w.io/produtos/3256190274/imagens/bicho-de-pelucia-fofinho-unicornio-20-cm-importado-rosa-azul/3256190282_1_large.jpg", "03729624000132", 1);
insert into tb_mercante values (null, "Só Funko", "Loja especializada em Funkos", "https://cf.shopee.com.br/file/81f234e9edc6c1d077b369575629153a", "02829706000196", 2);
insert into tb_mercante values (null, "Ação em Miniatura", "Loja especializada em Action Figures", "https://storage.geralgeek.com.br/images/venda/Levi-Ackerman---Attack-on-Titan---PVC-Figure-Anime-Action-Figure-6101354bd5588.jpg", "04343875000147", 3);
insert into tb_mercante values (null, "Arte Emoldurada", "Loja especializada quadros e desenhos por encomenda", "https://imgs.casasbahia.com.br/1515032040/1xg.jpg?imwidth=500", "77539591000102", 4);
insert into tb_mercante values (null, "Capinha Decorada", "Loja especializada em capinhas personalizadas para celular", "https://ae01.alicdn.com/kf/H52785214f0e6415f8d43bcacc0b0c3efL/Jap-o-anime-jujutsu-kaisen-caso-de-telefone-para-o-iphone-13-12-11-pro-xs.jpg_Q90.jpg_.webp", "60216457000160", 5);

insert into tb_categoria values (null, "Action Figures", "Bonecos de Ação");
insert into tb_categoria values (null, "Quadros", "Quadros");
insert into tb_categoria values (null, "Bottons", "Bottons");
insert into tb_categoria values (null, "Capinhas", "Capinhas para celular");
insert into tb_categoria values (null, "Acessorios", "Pulseiras, colares");
insert into tb_categoria values (null, "Gamer", "Produtos relacionados a jogos");
insert into tb_categoria values (null, "Pelúcias", "Bichinhos de Pelúcia");

insert into tb_produto values (null, "Totoro em pelúcia", "Pelúcia do Totoro de A Viagem de Chihiro.", 38.99, 100, 1, 7);
insert into tb_produto values (null, "Funko Mando Child Mandalorian", "Os colecionáveis Funko Pop! dão vida aos seus personagens favoritos com um design estilizado exclusivo.", 144.30, 100, 2, 1);
insert into tb_produto values (null, "Hashira Rengoku Action Figure", "Desafio você a me falar uma pessoa que não se emociona ao ver a história desse personagem, e duvido encontrar um oni que não se arrepia só de ouvir seu nome. Esse Action Figure do personagem Rengoku do anime Demon Slayer mede aproximadamente 16cm e mesclaria muito bem com outros Hashiras não acha? Então que tal ver nossa coleção lendária do anime e se divertir com o que encontrar ? *As cores podem vir em tons minimamente diferentes", 119.00, 12, 3, 1);
insert into tb_produto values (null, "Quadro One Punch Man Saitama", "Quadro decocaritvo do Saitama", 289.90, 10, 4, 2);
insert into tb_produto values (null, "Capinha Astronauta iPhone", "Com a Capinha Anti Choque Astronauta Matte iPhone Anova® de material maleável de alta qualidade seu iPhone fica mais protegido contra quedas, riscos na câmera e fica ainda muito bonito.  ", 87.00, 30, 5, 4);

insert into tb_produto_imagem values (null, "https://ae01.alicdn.com/kf/Sf994c2e1e9d04d6bb55fe7a7d5a7549bi/Totoro-brinquedo-de-pel-cia-bonito-gato-de-pel-cia-anime-japon-s-figura-boneca-de.jpg_Q90.jpg_.webp", "Foto da pelucia do Totoro", 1);
insert into tb_produto_imagem values (null, "https://m.media-amazon.com/images/I/61tnjPwUpSL._AC_SX679_.jpg", "Foto do Funko do Mandaloriano", 2);
insert into tb_produto_imagem values (null, "https://cdn.shopify.com/s/files/1/0595/2526/7508/products/H3ea0e07fa6e54981bb83c89b4282b6f3J.jpg?v=1654730473", "Foto do Action Figure do Rengoku", 3);
insert into tb_produto_imagem values (null, "https://cdn.iset.io/assets/55268/produtos/20086/thumb_750-750-quadro-decorativo-anime-one-punch-man-saitama-_02.jpg", "Foto do quadro do Saitama", 4);
insert into tb_produto_imagem values (null, "https://cdn.shopify.com/s/files/1/0592/5391/5853/products/S6a1c20ccdb124dfa8306a5962a45dedbd_500x.jpg?v=1644099295", "Capinha para iPhone Astronauta", 5);