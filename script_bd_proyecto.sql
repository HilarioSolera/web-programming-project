-- Script SQL para crear la base de datos Proyecto y sus tablas
CREATE DATABASE Proyecto;
GO

USE Proyecto;
GO

-- Tabla Clientes
CREATE TABLE Clientes (
    Id INT PRIMARY KEY IDENTITY,
    Identificacion NVARCHAR(20) NOT NULL,
    NombreCompleto NVARCHAR(100) NOT NULL,
    Provincia NVARCHAR(50) NOT NULL,
    Canton NVARCHAR(50) NOT NULL,
    Distrito NVARCHAR(50) NOT NULL,
    DireccionExacta NVARCHAR(200) NOT NULL,
    Telefono NVARCHAR(20) NOT NULL,
    Correo NVARCHAR(100) NOT NULL,
    PreferenciaLavado NVARCHAR(50) NOT NULL
);

-- Tabla Empleados
CREATE TABLE Empleados (
    Id INT PRIMARY KEY IDENTITY,
    Identificacion NVARCHAR(20) NOT NULL,
    NombreCompleto NVARCHAR(100) NOT NULL,
    Telefono NVARCHAR(20) NOT NULL,
    Correo NVARCHAR(100) NOT NULL,
    Puesto NVARCHAR(50) NOT NULL,
    FechaNacimiento DATE NOT NULL,
    FechaIngreso DATE NOT NULL,
    SalarioPorDia DECIMAL(10,2) NOT NULL,
    DiasVacaciones INT NOT NULL,
    FechaRetiro DATE NULL,
    MontoLiquidacion DECIMAL(18,4) NULL
);

-- Tabla Vehiculos
CREATE TABLE Vehiculos (
    Id INT PRIMARY KEY IDENTITY,
    Placa NVARCHAR(15) NOT NULL,
    Marca NVARCHAR(30) NOT NULL,
    Modelo NVARCHAR(30) NOT NULL,
    Color NVARCHAR(20) NOT NULL,
    Traccion NVARCHAR(20) NOT NULL,
    TratamientoNanoCeramico BIT NOT NULL,
    Anio INT NOT NULL,
    UltimaAtencion DATE NOT NULL,
    ClienteId INT NOT NULL,
    CONSTRAINT FK_Vehiculo_Cliente FOREIGN KEY (ClienteId)
        REFERENCES Clientes(Id) ON DELETE CASCADE
);

-- Tabla Lavados
CREATE TABLE Lavados (
    Id INT PRIMARY KEY IDENTITY,
    Fecha DATE NOT NULL,
    TipoLavado NVARCHAR(50) NOT NULL,
    Precio DECIMAL(10,2) NOT NULL,
    Estado NVARCHAR(20) NOT NULL,
    ClienteId INT NOT NULL,
    VehiculoId INT NOT NULL,
    EmpleadoId INT NULL,
    CONSTRAINT FK_Lavado_Cliente FOREIGN KEY (ClienteId)
        REFERENCES Clientes(Id) ON DELETE NO ACTION,
    CONSTRAINT FK_Lavado_Vehiculo FOREIGN KEY (VehiculoId)
        REFERENCES Vehiculos(Id) ON DELETE NO ACTION,
    CONSTRAINT FK_Lavado_Empleado FOREIGN KEY (EmpleadoId)
        REFERENCES Empleados(Id) ON DELETE SET NULL
);

-- Vista opcional (si aplica)
-- CREATE VIEW ClientesSinLavadoReciente AS
-- SELECT ...
