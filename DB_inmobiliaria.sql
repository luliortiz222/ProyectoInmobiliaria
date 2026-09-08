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

CREATE TABLE Usuario (
    IdUsuario INT AUTO_INCREMENT PRIMARY KEY,
    Email VARCHAR(150) UNIQUE NOT NULL,
    Password VARCHAR(255) NOT NULL,
    Nombre VARCHAR(100) NOT NULL,
    Apellido VARCHAR(100) NOT NULL,
    Avatar VARCHAR(300),
    Rol VARCHAR(20) NOT NULL
);

CREATE TABLE Inmueble (
    IdInmueble INT AUTO_INCREMENT PRIMARY KEY,
    Direccion VARCHAR(200) NOT NULL,
    Cupo INT NOT NULL,
    Coordenadas VARCHAR(200) NOT NULL,
    PrecioPorDia DECIMAL(10,2) NOT NULL,
    ImagenPortada VARCHAR(300),
    Estado BOOLEAN NOT NULL DEFAULT TRUE, 
    IdPropietario INT NOT NULL,
    IdTipoInmueble INT NOT NULL,
    PorcentajeReserva DECIMAL(5,2) NOT NULL DEFAULT 0,

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
    IdUsuarioCreador INT NOT NULL,
    IdUsuarioFinalizador INT NULL,
    FechaFinalizacion DATE NULL,

    FOREIGN KEY (IdInquilino) REFERENCES Inquilino(IdInquilino),
    FOREIGN KEY (IdInmueble) REFERENCES Inmueble(IdInmueble),
    FOREIGN KEY (IdUsuarioCreador) REFERENCES Usuario(IdUsuario),
    FOREIGN KEY (IdUsuarioFinalizador) REFERENCES Usuario(IdUsuario)

);

CREATE TABLE Pago (
    IdPago INT AUTO_INCREMENT PRIMARY KEY,
    IdReserva INT NOT NULL,
    Concepto VARCHAR(200) NOT NULL,
    FechaPago DATE NOT NULL,
    Importe DECIMAL(10,2) NOT NULL,
    Estado BOOLEAN NOT NULL DEFAULT TRUE,
    IdUsuarioCreador INT NOT NULL,
    IdUsuarioAnulador INT NULL,

    FOREIGN KEY (IdReserva) REFERENCES Reserva(IdReserva),
    FOREIGN KEY (IdUsuarioCreador) REFERENCES Usuario(IdUsuario),
    FOREIGN KEY (IdUsuarioAnulador) REFERENCES Usuario(IdUsuario)
);

