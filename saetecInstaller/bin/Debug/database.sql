-- 
-- ********************** MySQL ************************;
-- ********************** 5.7.23 ***********************;

-- *****************************************************;
-- **************** DATABASE: SAETEC *******************;
-- *****************************************************;
--          **** Final build: 11-11-2019 ****

CREATE DATABASE IF NOT EXISTS `saetec`;
USE saetec;
ALTER DATABASE saetec CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;


-- ************************************** `reservas`

CREATE TABLE IF NOT EXISTS `reservas`
(
 `id`        	int auto_increment,
 `recId`	 	int NOT NULL,
 `reserva`   	varchar(45) NOT NULL ,
 `usuario`   	int NOT NULL ,
 `cancelado` 	tinyint(1) NOT NULL ,
 `nome`		 	varchar(50) NOT NULL,
PRIMARY KEY (`id`)
);

-- ************************************** `recurso`

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
 
PRIMARY KEY (`id`)
);


-- ************************************************ RECURSOS
INSERT INTO `recurso` (`recId`, `nome`, `tipo_recurso`, `quantidade`, `descricao`) VALUES (NULL, 'Projetor Sony VPL-HW45ES Full HD', 'Recurso / Aparelho', '1', 'Projetor Sony VPL-HW45ES Full HD');
INSERT INTO `recurso` (`recId`, `nome`, `tipo_recurso`, `quantidade`, `descricao`) VALUES (NULL, 'Mapa Mundi 200x200 4:4', 'Recurso / Aparelho', '1', 'Mapa Mundi - 200x200 4:4');
INSERT INTO `recurso` (`recId`, `nome`, `tipo_recurso`, `quantidade`, `descricao`) VALUES (NULL, 'Esqueleto Humano', 'Recurso / Aparelho', '1', 'Modelo anatomico de esqueleto humano de fibra de carbono');
INSERT INTO `recurso` (`recId`, `nome`, `tipo_recurso`, `quantidade`, `descricao`) VALUES (NULL, 'Auditorio', 'Sala / Laboratorio', '1', 'Auditorio');
INSERT INTO `recurso` (`recId`, `nome`, `tipo_recurso`, `quantidade`, `descricao`) VALUES (NULL, 'Radio', 'Recurso / Aparelho', '1', 'Radio Sony VHX5ES');
INSERT INTO `recurso` (`recId`, `nome`, `tipo_recurso`, `quantidade`, `descricao`) VALUES (NULL, 'Caixa de Som', 'Recurso / Aparelho', '1', 'Caixa de Som Panasonic');
INSERT INTO `recurso` (`recId`, `nome`, `tipo_recurso`, `quantidade`, `descricao`) VALUES (NULL, 'Caixa de Som Bluetooth', 'Recurso / Aparelho', '1', 'Caixa de Som Bluetooth 3.1');
INSERT INTO `recurso` (`recId`, `nome`, `tipo_recurso`, `quantidade`, `descricao`) VALUES (NULL, 'Projetor Epson 2002', 'Recurso / Aparelho', '1', 'Projetor da Epson 2002 1080p HD');
INSERT INTO `recurso` (`recId`, `nome`, `tipo_recurso`, `quantidade`, `descricao`) VALUES (NULL, 'Caixa de Som', 'Recurso / Aparelho', '1', 'Caixa de Som Panasonic');



-- ************************************************ USUÁRIOS
INSERT INTO `usuarios` ( `nome`, `username`, `password`, `tipo`, `ativo`) VALUES ( 'Admin', 'admin', '21232f297a57a5a743894a0e4a801fc3', '1', '1'); -- Administrator Account
INSERT INTO `usuarios` ( `nome`, `username`, `password`, `tipo`, `ativo`) VALUES ( 'Joao', 'joao', MD5('joao12345'), '2', '1');
INSERT INTO `usuarios` ( `nome`, `username`, `password`, `tipo`, `ativo`) VALUES ( 'Nathan Bryan Santos', 'nathanbryansantos-87@commscope.com', MD5('770642710'), '2', '1');
INSERT INTO `usuarios` ( `nome`, `username`, `password`, `tipo`, `ativo`) VALUES ( 'Marli Rosangela da Cruz', 'marliros@gmail.com', MD5('minhasenha123'), '2', '1');
INSERT INTO `usuarios` ( `nome`, `username`, `password`, `tipo`, `ativo`) VALUES ( 'Marco Aurelio Thompson', 'marco@antonieto.biz', MD5('marco10111975'), '2', '1');
INSERT INTO `usuarios` ( `nome`, `username`, `password`, `tipo`, `ativo`) VALUES ( 'Julia Castro', 'juliacastro.12@gmail.com', MD5('julinhacastro123'), '2', '1');
INSERT INTO `usuarios` ( `nome`, `username`, `password`, `tipo`, `ativo`) VALUES ( 'Ronaldo Passaro', 'passarovermelho4@gmail.com', MD5('umgrandepassarobranco'), '2', '1');
INSERT INTO `usuarios` ( `nome`, `username`, `password`, `tipo`, `ativo`) VALUES ( 'Testing', 'test@test.com', MD5('test@123'), '2', '1');
INSERT INTO `usuarios` ( `nome`, `username`, `password`, `tipo`, `ativo`) VALUES ( 'Testador', 'teste@teste.com', MD5('teste@123'), '2', '1');
INSERT INTO `usuarios` ( `nome`, `username`, `password`, `tipo`, `ativo`) VALUES ( 'Matheus Bitolo', 'matheusbitolo@ig.com', MD5('betonera5012'), '2', '0'); -- Deactivated Account



/* Testing Query */
-- drop database saetec;