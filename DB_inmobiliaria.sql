CREATE DATABASE inmobiliaria;
USE inmobiliaria;

CREATE TABLE Propietario (
    IdPropietario INT AUTO_INCREMENT PRIMARY KEY,
    Dni VARCHAR(20) UNIQUE NOT NULL,
    Nombre VARCHAR(100) NOT NULL,
    Apellido VARCHAR(100) NOT NULL,
    Email VARCHAR(150) UNIQUE,
    Telefono VARCHAR(30)
);

CREATE TABLE Inquilino (
    IdInquilino INT AUTO_INCREMENT PRIMARY KEY,
    Dni VARCHAR(20) UNIQUE NOT NULL,
    Nombre VARCHAR(100) NOT NULL,
    Apellido VARCHAR(100) NOT NULL,
    Email VARCHAR(150) UNIQUE,
    Telefono VARCHAR(30)
);

CREATE TABLE TipoInmueble (
    IdTipoInmueble INT AUTO_INCREMENT PRIMARY KEY,
    Nombre VARCHAR(100) NOT NULL
);

CREATE TABLE Inmueble (
    IdInmueble INT AUTO_INCREMENT PRIMARY KEY,
    Direccion VARCHAR(200) NOT NULL,
    CantidadAmbientes INT NOT NULL,
    Superficie DECIMAL(10,2) NOT NULL,
    PrecioPorDia DECIMAL(10,2) NOT NULL,
    ImagenPortada VARCHAR(300),
    IdPropietario INT NOT NULL,
    IdTipoInmueble INT NOT NULL,

    FOREIGN KEY (IdPropietario) REFERENCES Propietario(IdPropietario),
    FOREIGN KEY (IdTipoInmueble) REFERENCES TipoInmueble(IdTipoInmueble)
);

CREATE TABLE Reserva (
    IdReserva INT AUTO_INCREMENT PRIMARY KEY,
    IdInquilino INT NOT NULL,
    IdInmueble INT NOT NULL,
    MontoPorDia DECIMAL(10,2) NOT NULL,
    FechaDesde DATE NOT NULL,
    FechaHasta DATE NOT NULL,

    FOREIGN KEY (IdInquilino) REFERENCES Inquilino(IdInquilino),
    FOREIGN KEY (IdInmueble) REFERENCES Inmueble(IdInmueble)
);

