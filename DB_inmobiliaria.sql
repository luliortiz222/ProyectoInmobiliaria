CREATE DATABASE inmobiliaria;
USE inmobiliaria;


CREATE TABLE Propietario (
    IdPropietario INT AUTO_INCREMENT PRIMARY KEY,
    Dni VARCHAR(20) NOT NULL,
    Nombre VARCHAR(100) NOT NULL,
    Apellido VARCHAR(100) NOT NULL,
    Email VARCHAR(150),
    Telefono VARCHAR(30)
);

CREATE TABLE Inquilino (
    IdInquilino INT AUTO_INCREMENT PRIMARY KEY,
    Dni VARCHAR(20) NOT NULL,
    Nombre VARCHAR(100) NOT NULL,
    Apellido VARCHAR(100) NOT NULL,
    Email VARCHAR(150),
    Telefono VARCHAR(30)
);


