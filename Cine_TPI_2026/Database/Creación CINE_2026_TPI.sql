-- =========================================================================
-- TPI Bases de Datos I / Programación II - Script de Creación (DDL)
-- =========================================================================

CREATE DATABASE CINE_2026_TPI;
GO
USE CINE_2026_TPI;
GO

-- 1. TABLAS SIN CLAVES FORÁNEAS

CREATE TABLE usuarios (
    id_usuario INT IDENTITY(1,1) NOT NULL,
    nombre_usuario VARCHAR(50) NOT NULL,
    password VARCHAR(100) NOT NULL,
    email VARCHAR(100) NULL,
    rol VARCHAR(20) NOT NULL DEFAULT 'Empleado',
    CONSTRAINT pk_usuarios PRIMARY KEY (id_usuario),
    CONSTRAINT uq_usuarios_nombre UNIQUE (nombre_usuario)
);

CREATE TABLE generos (
    id_genero INT IDENTITY(1,1) NOT NULL,
    descripcion VARCHAR(50) NOT NULL,
    CONSTRAINT pk_generos PRIMARY KEY (id_genero)
);

CREATE TABLE clasificaciones (
    id_clasificacion INT IDENTITY(1,1) NOT NULL,
    descripcion VARCHAR(3) NOT NULL,
    CONSTRAINT pk_clasificaciones PRIMARY KEY (id_clasificacion)
);

CREATE TABLE directores (
    id_director INT IDENTITY(1,1) NOT NULL,
    nombre VARCHAR(25) NOT NULL,
    apellido VARCHAR(25) NOT NULL,
    fec_nacimiento DATETIME,
    nacionalidad VARCHAR(20),
    CONSTRAINT pk_directores PRIMARY KEY (id_director)
);

CREATE TABLE actores (
    id_actor INT IDENTITY(1,1) NOT NULL,
    nombre VARCHAR(25) NOT NULL,
    apellido VARCHAR(25) NOT NULL,
    fec_nacimiento DATETIME,
    nacionalidad VARCHAR(20),
    CONSTRAINT pk_actores PRIMARY KEY (id_actor)
);

CREATE TABLE tipos_sala (
    id_tipo_sala INT IDENTITY(1,1) NOT NULL,
    descripcion VARCHAR(50) NOT NULL,
    precio_entrada DECIMAL(10,2) NOT NULL,
    CONSTRAINT pk_tipos_sala PRIMARY KEY (id_tipo_sala)
);

CREATE TABLE clientes (
    id_cliente INT IDENTITY(1,1) NOT NULL,
    nombre VARCHAR(50) NOT NULL,
    apellido VARCHAR(50) NOT NULL,
    dni BIGINT NOT NULL,
    e_mail VARCHAR(100),
    Activo BIT NOT NULL DEFAULT 1,
    CONSTRAINT pk_clientes PRIMARY KEY (id_cliente)
);

CREATE TABLE formas_pago (
    id_forma_pago INT IDENTITY(1,1) NOT NULL,
    descripcion VARCHAR(50) NOT NULL,
    CONSTRAINT pk_formas_pago PRIMARY KEY (id_forma_pago)
);

CREATE TABLE tipos_descuento (
    id_tipo_descuento INT IDENTITY(1,1) NOT NULL,
    descripcion DECIMAL(3,2) NOT NULL,
    CONSTRAINT pk_tipos_descuento PRIMARY KEY (id_tipo_descuento)
);

CREATE TABLE estados_reserva (
    id_estado_reserva INT IDENTITY(1,1) NOT NULL,
    descripcion VARCHAR(12) NOT NULL,
    CONSTRAINT pk_estados_reserva PRIMARY KEY (id_estado_reserva)
);

CREATE TABLE tipos_producto (
    id_tipo_producto INT IDENTITY(1,1) NOT NULL,
    descripcion VARCHAR(10) NOT NULL,
    CONSTRAINT pk_tipos_producto PRIMARY KEY (id_tipo_producto)
);

CREATE TABLE tamaños_producto (
    id_tamaño_producto INT IDENTITY(1,1) NOT NULL,
    descripcion VARCHAR(10) NOT NULL,
    CONSTRAINT pk_tamaños_producto PRIMARY KEY (id_tamaño_producto)
);

-- 2. TABLAS CON CLAVES FORÁNEAS DE PRIMER NIVEL

CREATE TABLE peliculas (
    id_pelicula INT IDENTITY(1,1) NOT NULL,
    titulo VARCHAR(100) NOT NULL,
    id_genero INT NOT NULL,
    duracion INT NOT NULL,
    fecha_estreno DATETIME,
    id_clasificacion INT NOT NULL,
    sinopsis VARCHAR(500),
    id_director INT NOT NULL,
    CONSTRAINT pk_peliculas PRIMARY KEY (id_pelicula),
    CONSTRAINT fk_peliculas_generos FOREIGN KEY (id_genero) REFERENCES generos(id_genero),
    CONSTRAINT fk_peliculas_clasificaciones FOREIGN KEY (id_clasificacion) REFERENCES clasificaciones(id_clasificacion),
    CONSTRAINT fk_peliculas_directores FOREIGN KEY (id_director) REFERENCES directores(id_director)
);

CREATE TABLE salas (
    id_sala INT IDENTITY(1,1) NOT NULL,
    descripcion VARCHAR(50) NOT NULL,
    id_tipo_sala INT NOT NULL,
    capacidad INT NOT NULL,
    CONSTRAINT pk_salas PRIMARY KEY (id_sala),
    CONSTRAINT fk_salas_tipos FOREIGN KEY (id_tipo_sala) REFERENCES tipos_sala(id_tipo_sala)
);

CREATE TABLE promociones (
    id_promocion INT IDENTITY(1,1) NOT NULL,
    dia VARCHAR(50) NOT NULL,
    hora_inicio TIME NULL, 
    hora_fin TIME NULL,    
    id_tipo_descuento INT NOT NULL,
    CONSTRAINT pk_promociones PRIMARY KEY (id_promocion),
    CONSTRAINT fk_promociones_tipos_desc FOREIGN KEY (id_tipo_descuento) REFERENCES tipos_descuento(id_tipo_descuento)
);

CREATE TABLE comprobantes (
    id_comprobante INT IDENTITY(1,1) NOT NULL,
    id_cliente INT NOT NULL,
    fecha DATETIME NOT NULL,
    id_forma_pago INT NOT NULL,
    Activo BIT NOT NULL DEFAULT 1,
    CONSTRAINT pk_comprobantes PRIMARY KEY (id_comprobante),
    CONSTRAINT fk_comprobantes_clientes FOREIGN KEY (id_cliente) REFERENCES clientes(id_cliente),
    CONSTRAINT fk_comprobantes_formas_pago FOREIGN KEY (id_forma_pago) REFERENCES formas_pago(id_forma_pago)
);

CREATE TABLE reservas (
    id_reserva INT IDENTITY(1,1) NOT NULL,
    id_cliente INT NOT NULL,
    fecha_reserva DATETIME NOT NULL,
    id_estado INT NOT NULL,
    CONSTRAINT pk_reservas PRIMARY KEY (id_reserva),
    CONSTRAINT fk_reservas_clientes FOREIGN KEY (id_cliente) REFERENCES clientes(id_cliente),
    CONSTRAINT fk_reservas_estados FOREIGN KEY (id_estado) REFERENCES estados_reserva(id_estado_reserva)
);

CREATE TABLE productos (
    id_producto INT IDENTITY(1,1) NOT NULL,
    descripcion VARCHAR(50) NOT NULL,
    pre_unitario DECIMAL(10,2) NOT NULL,
    id_tipo_producto INT NOT NULL,
    id_tamaño_producto INT NOT NULL,
    stock INT NOT NULL DEFAULT 30,
    stock_minimo INT NOT NULL DEFAULT 10,
    Activo BIT NOT NULL DEFAULT 1,
    CONSTRAINT pk_productos PRIMARY KEY (id_producto),
    CONSTRAINT fk_productos_tipos FOREIGN KEY (id_tipo_producto) REFERENCES tipos_producto(id_tipo_producto),
    CONSTRAINT fk_productos_tamaños FOREIGN KEY (id_tamaño_producto) REFERENCES tamaños_producto(id_tamaño_producto)
);

-- 3. TABLAS DE RELACIONES Y DETALLES

CREATE TABLE detalles_pelicula_actor (
    id_pelicula_actor INT IDENTITY(1,1) NOT NULL,
    id_pelicula INT NOT NULL,
    id_actor INT NOT NULL,
    CONSTRAINT pk_detalles_pelicula_actor PRIMARY KEY (id_pelicula_actor),
    CONSTRAINT fk_detalle_pa_peliculas FOREIGN KEY (id_pelicula) REFERENCES peliculas(id_pelicula),
    CONSTRAINT fk_detalle_pa_actores FOREIGN KEY (id_actor) REFERENCES actores(id_actor)
);

CREATE TABLE butacas (
    id_butaca INT IDENTITY(1,1) NOT NULL,
    fila VARCHAR(2) NOT NULL,
    columna INT NOT NULL,
    id_sala INT NOT NULL,
    CONSTRAINT pk_butacas PRIMARY KEY (id_butaca),
    CONSTRAINT fk_butacas_salas FOREIGN KEY (id_sala) REFERENCES salas(id_sala)
);

CREATE TABLE funciones (
    id_funcion INT IDENTITY(1,1) NOT NULL,
    fecha DATETIME NOT NULL,
    id_pelicula INT NOT NULL,
    id_sala INT NOT NULL,
    CONSTRAINT pk_funciones PRIMARY KEY (id_funcion),
    CONSTRAINT fk_funciones_peliculas FOREIGN KEY (id_pelicula) REFERENCES peliculas(id_pelicula),
    CONSTRAINT fk_funciones_salas FOREIGN KEY (id_sala) REFERENCES salas(id_sala)
);

CREATE TABLE detalles_comprobante_productos (
    id_detalle INT IDENTITY(1,1) NOT NULL,
    id_comprobante INT NOT NULL,
    id_producto INT NOT NULL,
    cantidad_productos INT NOT NULL,
    precio_unitario DECIMAL(10,2) NOT NULL,
    CONSTRAINT pk_detalles_comprobante_prod PRIMARY KEY (id_detalle),
    CONSTRAINT fk_detalle_cp_comprobantes FOREIGN KEY (id_comprobante) REFERENCES comprobantes(id_comprobante),
    CONSTRAINT fk_detalle_cp_productos FOREIGN KEY (id_producto) REFERENCES productos(id_producto)
);

-- 4. TABLAS DE ÚLTIMO NIVEL

CREATE TABLE entradas (
    id_entrada INT IDENTITY(1,1) NOT NULL,
    id_funcion INT NOT NULL,
    id_butaca INT NOT NULL,
    edad INT NOT NULL,
    precio_cobrado DECIMAL(10,2) NOT NULL,
    id_comprobante INT NULL,
    id_promocion INT NULL,
    CONSTRAINT pk_entradas PRIMARY KEY (id_entrada),
    CONSTRAINT fk_entradas_funciones FOREIGN KEY (id_funcion) REFERENCES funciones(id_funcion),
    CONSTRAINT fk_entradas_butacas FOREIGN KEY (id_butaca) REFERENCES butacas(id_butaca),
    CONSTRAINT fk_entradas_comprobantes FOREIGN KEY (id_comprobante) REFERENCES comprobantes(id_comprobante),
    CONSTRAINT fk_entradas_promociones FOREIGN KEY (id_promocion) REFERENCES promociones(id_promocion)
);

CREATE TABLE detalles_reserva_entrada (
    id_detalle_reservas INT IDENTITY(1,1) NOT NULL,
    id_reserva INT NOT NULL,
    id_entrada INT NOT NULL,
    CONSTRAINT pk_detalles_reserva_entrada PRIMARY KEY (id_detalle_reservas),
    CONSTRAINT fk_detalle_re_reservas FOREIGN KEY (id_reserva) REFERENCES reservas(id_reserva),
    CONSTRAINT fk_detalle_re_entradas FOREIGN KEY (id_entrada) REFERENCES entradas(id_entrada)
);
GO