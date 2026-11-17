-- Tabla usuarios (requisito obligatorio del enunciado) + usuario inicial
USE CINE_2026_TPI_01;
GO

IF OBJECT_ID('dbo.usuarios', 'U') IS NULL
BEGIN
    CREATE TABLE usuarios (
        id_usuario     INT IDENTITY(1,1) NOT NULL,
        nombre_usuario VARCHAR(50)  NOT NULL,
        password       VARCHAR(100) NOT NULL,
        email          VARCHAR(100) NULL,
        rol            VARCHAR(20)  NOT NULL CONSTRAINT df_usuarios_rol DEFAULT 'Empleado',
        CONSTRAINT pk_usuarios PRIMARY KEY (id_usuario),
        CONSTRAINT uq_usuarios_nombre UNIQUE (nombre_usuario)
    );
END
GO

IF NOT EXISTS (SELECT 1 FROM usuarios WHERE nombre_usuario = 'admin')
    INSERT INTO usuarios (nombre_usuario, password, email, rol)
    VALUES ('admin', 'admin123', 'admin@cine.com', 'Admin');
GO
