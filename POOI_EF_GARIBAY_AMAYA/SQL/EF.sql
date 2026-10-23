--CREAR Y USAR BD
CREATE DATABASE ExamenFinal

USE ExamenFinal

--CREAR TABLAS
Create table tb_paises(
idpais char(3) PRIMARY KEY)

INSERT INTO tb_paises (idpais)
VALUES ('PER')

CREATE TABLE tb_Consumidor(
dnicont char(8) PRIMARY KEY,
nomcont varchar(40) not null,
apecont varchar(40) not null,
emailcont varchar(250) UNIQUE,
idpais char(3) REFERENCES tb_paises)

select * from tb_Consumidor

--PROCEDIMIENTOS ALMACENADOS

-- usp_listar_consumidor, donde liste los registros de tb_consumidor.
CREATE OR ALTER PROCEDURE usp_listar_consumidor
AS
BEGIN
SELECT * FROM tb_Consumidor
END

--usp_agrega_consumidor donde inserte un registro a la tabla tb_consumidor.
--Evaluaciones: campo dnicont es tipo numérico y de 8 digitos; nomcont y apecont no deben estar vacíos,
--campo pais es tipo string. Su resultado de las operaciones se envía a través de un parámetro de salida.
CREATE OR ALTER PROCEDURE usp_agrega_consumidor
@dnicont char(8),
@nomcont varchar(40),
@apecont varchar(40),
@emailcont varchar(250),
@idpais char(3)
AS
BEGIN
INSERT INTO tb_Consumidor VALUES (@dnicont, @nomcont, @apecont, @emailcont, @idpais)
END

--usp_eliminar_consumidor, donde permita eliminar un registro de consumidor por su campo dnicont,
--verificar previamente que dicho valor se encuentra en la tabla.
CREATE OR ALTER PROCEDURE usp_eliminar_consumidor
@dnicont CHAR(8)
AS
BEGIN
    IF EXISTS (SELECT * FROM tb_Consumidor WHERE dnicont = @dnicont)
    BEGIN
        DELETE FROM tb_Consumidor WHERE dnicont = @dnicont
    END
END


