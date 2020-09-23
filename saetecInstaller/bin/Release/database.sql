-- 
-- ********************** MySQL ************************;
-- ********************** 5.7.23 ***********************;

-- *****************************************************;
-- **************** DATABASE: SAETEC *******************;
-- *****************************************************;
--           **** Final build: 21-11-2019 ****

CREATE DATABASE IF NOT EXISTS `saetec`;
USE saetec;
ALTER DATABASE saetec CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;

-- ********************************* `recurso`

CREATE TABLE IF NOT EXISTS `recurso`
(
 `recId`         int auto_increment NOT NULL,
 `nome`          varchar(50) NOT NULL ,
 `tipo_recurso`  varchar(50) NOT NULL ,
 `quantidade`    int NOT NULL ,
 `descricao`     longtext NULL ,
PRIMARY KEY (`recId`)
);

-- ************************************** `usuarios`

CREATE TABLE IF NOT EXISTS `usuarios`
(
 `id`                  int auto_increment,
 `nome`                varchar(50) NOT NULL ,
 `username`            varchar(255) NOT NULL ,
 `password`            varchar(255) NOT NULL ,
 `tipo`                varchar(45) NOT NULL ,
 `ativo`               tinyint(1) NOT NULL ,
 `pendente`			   tinyint(1) NOT NULL ,
 
PRIMARY KEY (`id`)
);

-- ************************************** `reservas`

CREATE TABLE IF NOT EXISTS `reservas`
(
 `id`        	int auto_increment,
 `recId`	 	int NOT NULL,
 `reserva`   	varchar(45) NOT NULL ,
 `usuario`   	int NOT NULL ,
 `cancelado` 	tinyint(1) NOT NULL ,
 `nome`		 	varchar(50) NOT NULL,
PRIMARY KEY (`id`),
CONSTRAINT FK_USUARIO FOREIGN KEY(usuario) REFERENCES usuarios(id),
CONSTRAINT FK_RECURSO FOREIGN KEY(recId) REFERENCES recurso(RecId)
);

-- ****** Testing Entries
-- ************************************************ RECURSOS
INSERT INTO `recurso` (`recId`, `nome`, `tipo_recurso`, `quantidade`, `descricao`) VALUES (NULL, 'Projetor Sony', 'Recurso / Aparelho', '1', 'Projetor Sony VPL');
INSERT INTO `recurso` (`recId`, `nome`, `tipo_recurso`, `quantidade`, `descricao`) VALUES (NULL, 'Mapa Mundi', 'Recurso / Aparelho', '1', '4:4 200x200');
INSERT INTO `recurso` (`recId`, `nome`, `tipo_recurso`, `quantidade`, `descricao`) VALUES (NULL, 'Esqueleto Humano', 'Recurso / Aparelho', '1', 'Modelo Anatomico');
INSERT INTO `recurso` (`recId`, `nome`, `tipo_recurso`, `quantidade`, `descricao`) VALUES (NULL, 'Auditorio', 'Sala / Laboratorio', '1', 'Auditorio');
INSERT INTO `recurso` (`recId`, `nome`, `tipo_recurso`, `quantidade`, `descricao`) VALUES (NULL, 'Radio', 'Recurso / Aparelho', '1', 'Radio Sony VHX5ES');
INSERT INTO `recurso` (`recId`, `nome`, `tipo_recurso`, `quantidade`, `descricao`) VALUES (NULL, 'Caixa de Som', 'Recurso / Aparelho', '1', 'Caixa de Som Panasonic');
INSERT INTO `recurso` (`recId`, `nome`, `tipo_recurso`, `quantidade`, `descricao`) VALUES (NULL, 'Caixa de Som Bluetooth', 'Recurso / Aparelho', '1', 'Bluetooth 3.1');
INSERT INTO `recurso` (`recId`, `nome`, `tipo_recurso`, `quantidade`, `descricao`) VALUES (NULL, 'Projetor Epson', 'Recurso / Aparelho', '1', 'Projetor Epson 2002');
INSERT INTO `recurso` (`recId`, `nome`, `tipo_recurso`, `quantidade`, `descricao`) VALUES (NULL, 'Caixa de Som', 'Recurso / Aparelho', '1', 'Caixa de Som Panasonic');

-- ****** Testing Entries
-- ************************************************ USUÁRIOS
INSERT INTO `usuarios` ( `nome`, `username`, `password`, `tipo`, `ativo`, `pendente`) VALUES ( 'Admin', 'admin', '21232f297a57a5a743894a0e4a801fc3', '1', '1', '0'); -- Administrator Account
INSERT INTO `usuarios` ( `nome`, `username`, `password`, `tipo`, `ativo`, `pendente`) VALUES ( 'Test', 'test', MD5('test'), '2', '1', '0'); -- Test Account
INSERT INTO `usuarios` ( `nome`, `username`, `password`, `tipo`, `ativo`, `pendente`) VALUES ( 'Teste', 'teste', MD5('teste'), '2', '1', '0'); -- Test Account
INSERT INTO `usuarios` ( `nome`, `username`, `password`, `tipo`, `ativo`, `pendente`) VALUES ( 'Chapolin', 'ch@po.lin', MD5('chapolimcolorado'), '2', '1', '0'); -- Normal Account
INSERT INTO `usuarios` ( `nome`, `username`, `password`, `tipo`, `ativo`, `pendente`) VALUES ( 'Teste 01', 'teste01@teste.com', MD5('123456789'), '2', '1', '0'); -- T1 Account
INSERT INTO `usuarios` ( `nome`, `username`, `password`, `tipo`, `ativo`, `pendente`) VALUES ( 'Teste 02', 'teste02@teste.com', MD5('123456789'), '2', '1', '0'); -- T2 Account
INSERT INTO `usuarios` ( `nome`, `username`, `password`, `tipo`, `ativo`, `pendente`) VALUES ( 'betovem', 'betovem@deezer.com', MD5('betovem123123'), '2', '1', '1'); -- Pending Account
INSERT INTO `usuarios` ( `nome`, `username`, `password`, `tipo`, `ativo`, `pendente`) VALUES ( 'Karl Marx', 'karl@marx.com', MD5('capitalismo'), '2', '0', '0'); -- Deactivated Account



/* Debug Purpose Only */
-- drop database saetec;
