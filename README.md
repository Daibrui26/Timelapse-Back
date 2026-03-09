# TimelapseCompleto

docker-compose up -d --build

docker compose down

USUARIO DE PRUEBA:

ana.perez@email.com
contraseña123

Puerto de la web: http://localhost:5173/

Al hacer el dockercompose up

ejecutar en heidi sql (o cualquier otro) con puerto 7983 Tipo de server: Microsoft SQL SERVER (TCP/IP) Libreria: SQLOLEDB usuario=sa y contraseña:yourStrong(!)Password
esto:

-----------------------------------------------------------------
CREATE DATABASE TimelapseDB;
USE TimelapseDB;
-----------------------------------------------------------------

CREATE TABLE Usuario (
    Id_Usuario INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(100),
    Email NVARCHAR(150) NOT NULL UNIQUE,
    Contraseña NVARCHAR(255) NOT NULL,
    Rol NVARCHAR(20) NOT NULL DEFAULT 'usuario',
    foto_perfil NVARCHAR(500) NULL,
    foto_perfil_public_id NVARCHAR(200) NULL;
    CONSTRAINT CHK_Usuario_Rol CHECK (Rol IN ('admin', 'usuario'))
);

-- Tabla Capsula
CREATE TABLE Capsula (
    Id_Capsula INT IDENTITY(1,1) PRIMARY KEY,
    Titulo NVARCHAR(150),
    Descripcion NVARCHAR(MAX),
    Fecha_Creacion DATE,
    Fecha_Apertura DATE,
    Estado NVARCHAR(20),
    Visibilidad NVARCHAR(20)
);

-- Tabla Usuario_Capsula
CREATE TABLE Usuario_Capsula (
    Id_Usuario_Capsula INT IDENTITY(1,1) PRIMARY KEY,
    Id_Usuario INT,
    Id_Capsula INT,
    Rol NVARCHAR(30),
    CONSTRAINT FK_Usuario_Capsula_Usuario FOREIGN KEY (Id_Usuario) REFERENCES Usuario(Id_Usuario),
    CONSTRAINT FK_Usuario_Capsula_Capsula FOREIGN KEY (Id_Capsula) REFERENCES Capsula(Id_Capsula)
);

-- Tabla Contenido
CREATE TABLE Contenido (
    Id_Contenido INT IDENTITY(1,1) PRIMARY KEY,
    Tipo NVARCHAR(30),
    Contenido NVARCHAR(MAX) NULL,
    Fecha_Subida DATE,
    url_archivo NVARCHAR(500) NULL,
    public_id   NVARCHAR(200) NULL,
    Id_Capsula INT,
    CONSTRAINT FK_Contenido_Capsula FOREIGN KEY (Id_Capsula) REFERENCES Capsula(Id_Capsula)
);

-- Tabla Comentario
CREATE TABLE Comentario (
    Id_Comentario INT IDENTITY(1,1) PRIMARY KEY,
    Texto NVARCHAR(MAX),
    Fecha_Comentario DATE,
    Id_Usuario INT,
    Id_Capsula INT,
    CONSTRAINT FK_Comentario_Usuario FOREIGN KEY (Id_Usuario) REFERENCES Usuario(Id_Usuario),
    CONSTRAINT FK_Comentario_Capsula FOREIGN KEY (Id_Capsula) REFERENCES Capsula(Id_Capsula)
);

-- Tabla Notificacion
CREATE TABLE Notificacion (
    Id_Notificacion INT IDENTITY(1,1) PRIMARY KEY,
    Tipo NVARCHAR(50),
    Mensaje NVARCHAR(MAX),
    Fecha_Creacion DATE,
    Leida BIT,
    Id_Usuario INT,
    Id_Capsula INT NULL,
    CONSTRAINT FK_Notificacion_Usuario FOREIGN KEY (Id_Usuario) REFERENCES Usuario(Id_Usuario),
    CONSTRAINT FK_Notificacion_Capsula FOREIGN KEY (Id_Capsula) REFERENCES Capsula(Id_Capsula)
);

-- Tabla Amistad
CREATE TABLE Amistad (
    Id_Amistad INT IDENTITY(1,1) PRIMARY KEY,
    Id_Usuario1 INT,
    Id_Usuario2 INT,
    Estado NVARCHAR(20),
    CONSTRAINT FK_Amistad_Usuario1 FOREIGN KEY (Id_Usuario1) REFERENCES Usuario(Id_Usuario),
    CONSTRAINT FK_Amistad_Usuario2 FOREIGN KEY (Id_Usuario2) REFERENCES Usuario(Id_Usuario)
);

-- Tabla Usuario
INSERT INTO Usuario (Nombre, Email, Contraseña, Rol) VALUES
('Ana Pérez', 'ana.perez@email.com', 'contraseña123', 'admin'),
('Luis Gómez', 'luis.gomez@email.com', 'pass456', 'usuario'),
('María López', 'maria.lopez@email.com', 'abc123', 'usuario'),
('Carlos Sánchez', 'carlos.sanchez@email.com', 'clave789', 'admin');

-- Tabla Capsula
INSERT INTO Capsula (Titulo, Descripcion, Fecha_Creacion, Fecha_Apertura, Estado, Visibilidad) VALUES
('Capsula del Tiempo 2023', 'Capsula con recuerdos del 2023', '2023-01-01', '2024-01-01', 'abierta', 'privada'),
('Capsula Escolar', 'Capsula con trabajos del colegio', '2022-09-01', '2025-09-01', 'abierta', 'publica'),
('Capsula Familiar', 'Recuerdos familiares', '2021-05-10', '2031-05-10', 'cerrada', 'privada');

-- Tabla Usuario_Capsula
INSERT INTO Usuario_Capsula (Id_Usuario, Id_Capsula, Rol) VALUES
(1, 1, 'creador'),
(2, 1, 'participante'),
(3, 2, 'creador'),
(4, 3, 'creador'),
(1, 3, 'participante');

-- Tabla Contenido
INSERT INTO Contenido (Tipo, Contenido, Fecha_Subida, url_archivo, public_id, Id_Capsula)
VALUES
('Imagen', NULL, '2026-02-21', 'https://res.cloudinary.com/dg5m7m0pe/image/upload/v1772397277/timelapse/imagenes/ptbmuuekvwdh0f9khxmn.jpg', NULL, 1),
('Imagen', NULL, '2026-02-21', 'https://res.cloudinary.com/dg5m7m0pe/image/upload/v1772397385/timelapse/imagenes/ytxosyuqa84mkkggsf9s.webp', NULL, 2),
('Imagen', NULL, '2026-02-22', 'https://res.cloudinary.com/dg5m7m0pe/image/upload/v1772397277/timelapse/imagenes/ptbmuuekvwdh0f9khxmn.jpg', NULL, 2),
('Texto', 'Ejemplo práctico paso a paso del ejercicio propuesto.', '2026-02-23', NULL, NULL, 3),
('Video', NULL, '2026-02-24', 'https://res.cloudinary.com/dg5m7m0pe/image/upload/v1772397567/timelapse/imagenes/nsk4fmzokeocy3hecmyw.gif', NULL, 3);
-- Tabla Comentario
INSERT INTO Comentario (Texto, Fecha_Comentario, Id_Usuario, Id_Capsula) VALUES
('Qué recuerdos tan bonitos!', '2023-01-02', 2, 1),
('Me encantó esta idea', '2022-09-06', 3, 2),
('No puedo esperar a abrirla', '2021-05-16', 1, 3);

-- Tabla Notificacion
INSERT INTO Notificacion (Tipo, Mensaje, Fecha_Creacion, Leida, Id_Usuario, Id_Capsula) VALUES
('info', 'Has sido agregado a la capsula del tiempo 2023', '2023-01-01', 0, 2, 1),
('alerta', 'Tu capsula escolar se abrirá pronto', '2025-08-30', 0, 3, 2),
('info', 'Nuevo contenido en la capsula familiar', '2021-05-15', 1, 1, 3);

-- Tabla Amistad
INSERT INTO Amistad (Id_Usuario1, Id_Usuario2, Estado) VALUES
(1, 2, 'aceptada'),
(1, 3, 'pendiente'),
(2, 4, 'aceptada'),
(3, 4, 'aceptada');

-----------------------------------------------------------------
DROPTABLES:

DROP TABLE IF EXISTS Amistad;
DROP TABLE IF EXISTS Notificacion;
DROP TABLE IF EXISTS Comentario;
DROP TABLE IF EXISTS Contenido;
DROP TABLE IF EXISTS Usuario_Capsula;
DROP TABLE IF EXISTS Capsula;
DROP TABLE IF EXISTS Usuario;