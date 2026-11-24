-- =========================================================================
-- TPI Bases de Datos I / Programación II - Script de Inserción (DML)
-- =========================================================================

USE CINE_2026_TPI;
GO

-- 1. USUARIOS Y CATÁLOGOS BÁSICOS

INSERT INTO usuarios (nombre_usuario, password, email, rol) VALUES
('admin', 'admin123', NULL, 'Administrador'),
('Duilio Diaz', 'duilio123', 'duiliodiaz@gmail.com', 'Empleado');

INSERT INTO generos (descripcion) VALUES 
('Acción'), ('Comedia'), ('Drama'), ('Terror'), ('Ciencia Ficción'),
('Aventura'), ('Animación'), ('Suspenso'), ('Romance'), ('Fantasía');

INSERT INTO clasificaciones (descripcion) VALUES 
('ATP'), ('+13'), ('+16'), ('+18');

INSERT INTO tipos_sala (descripcion, precio_entrada) VALUES 
('2D Tradicional', 10000.00), 
('3D', 13000.00), 
('4D', 16000.00);

INSERT INTO formas_pago (descripcion) VALUES 
('Efectivo'), ('Tarjeta de Débito'), ('Tarjeta de Crédito');

INSERT INTO tipos_descuento (descripcion) VALUES 
(0.10), (0.20), (0.50);

INSERT INTO estados_reserva (descripcion) VALUES 
('Pendiente'), ('Confirmada'), ('Vencida'), ('Cancelada');

INSERT INTO tipos_producto (descripcion) VALUES 
('Comida'), ('Bebida');

INSERT INTO tamaños_producto (descripcion) VALUES 
('Pequeño'), ('Mediano'), ('Grande');

-- 2. PERSONAS (20 Directores, 40 Actores, 30 Clientes)

INSERT INTO directores (nombre, apellido, fec_nacimiento, nacionalidad) VALUES 
-- Originales (1..10)
('Steven', 'Spielberg', '1946-12-18', 'Estadounidense'),
('Christopher', 'Nolan', '1970-07-30', 'Británico'),
('Quentin', 'Tarantino', '1963-03-27', 'Estadounidense'),
('Martin', 'Scorsese', '1942-11-17', 'Estadounidense'),
('James', 'Cameron', '1954-08-16', 'Canadiense'),
('Peter', 'Jackson', '1961-10-31', 'Neozelandés'),
('Alfonso', 'Cuarón', '1961-11-28', 'Mexicano'),
('Guillermo', 'del Toro', '1964-10-09', 'Mexicano'),
('Denis', 'Villeneuve', '1967-10-03', 'Canadiense'),
('Greta', 'Gerwig', '1983-08-04', 'Estadounidense'),
-- 10 Nuevos (11..20)
('Ridley', 'Scott', '1937-11-30', 'Británico'),
('David', 'Fincher', '1962-08-28', 'Estadounidense'),
('Hayao', 'Miyazaki', '1941-01-05', 'Japonés'),
('Bong', 'Joon-ho', '1969-09-14', 'Surcoreano'),
('Damien', 'Chazelle', '1985-01-19', 'Estadounidense'),
('Juan José', 'Campanella', '1959-07-19', 'Argentino'),
('Damián', 'Szifron', '1975-07-09', 'Argentino'),
('George', 'Miller', '1945-03-03', 'Australiano'),
('Tim', 'Burton', '1958-08-25', 'Estadounidense'),
('Wes', 'Anderson', '1969-05-01', 'Estadounidense');

INSERT INTO actores (nombre, apellido, fec_nacimiento, nacionalidad) VALUES 
-- Originales (1..30)
('Leonardo', 'DiCaprio', '1974-11-11', 'Estadounidense'),
('Brad', 'Pitt', '1963-12-18', 'Estadounidense'),
('Tom', 'Hanks', '1956-07-09', 'Estadounidense'),
('Scarlett', 'Johansson', '1984-11-22', 'Estadounidense'),
('Natalie', 'Portman', '1981-06-09', 'Israelí'),
('Christian', 'Bale', '1974-01-30', 'Británico'),
('Cillian', 'Murphy', '1976-05-25', 'Irlandés'),
('Margot', 'Robbie', '1990-07-02', 'Australiana'),
('Ryan', 'Gosling', '1980-11-12', 'Canadiense'),
('Emma', 'Stone', '1988-11-06', 'Estadounidense'),
('Tom', 'Holland', '1996-06-01', 'Británico'),
('Zendaya', 'Coleman', '1996-09-01', 'Estadounidense'),
('Samuel L.', 'Jackson', '1948-12-21', 'Estadounidense'),
('Robert', 'De Niro', '1943-08-17', 'Estadounidense'),
('Al', 'Pacino', '1940-04-25', 'Estadounidense'),
('Kate', 'Winslet', '1975-10-05', 'Británica'),
('Zoe', 'Saldana', '1978-06-19', 'Estadounidense'),
('Sam', 'Worthington', '1976-08-02', 'Australiano'),
('Sigourney', 'Weaver', '1949-10-08', 'Estadounidense'),
('Keanu', 'Reeves', '1964-09-02', 'Canadiense'),
('Carrie-Anne', 'Moss', '1967-08-21', 'Canadiense'),
('Elijah', 'Wood', '1981-01-28', 'Estadounidense'),
('Ian', 'McKellen', '1939-05-25', 'Británico'),
('Viggo', 'Mortensen', '1958-10-20', 'Estadounidense'),
('Daniel', 'Radcliffe', '1989-07-23', 'Británico'),
('Emma', 'Watson', '1990-04-15', 'Británica'),
('Rupert', 'Grint', '1988-08-24', 'Británico'),
('Harrison', 'Ford', '1942-07-13', 'Estadounidense'),
('Mark', 'Hamill', '1951-09-25', 'Estadounidense'),
('Carrie', 'Fisher', '1956-10-21', 'Estadounidense'),
-- 10 Nuevos (31..40)
('Ricardo', 'Darín', '1957-01-16', 'Argentino'),
('Guillermo', 'Francella', '1955-02-14', 'Argentino'),
('Pedro', 'Pascal', '1975-04-02', 'Chileno'),
('Anya', 'Taylor-Joy', '1996-04-16', 'Estadounidense'),
('Timothée', 'Chalamet', '1995-12-27', 'Estadounidense'),
('Florence', 'Pugh', '1996-01-03', 'Británica'),
('Joaquin', 'Phoenix', '1974-10-28', 'Estadounidense'),
('Anne', 'Hathaway', '1982-11-12', 'Estadounidense'),
('Matt', 'Damon', '1970-10-08', 'Estadounidense'),
('Emily', 'Blunt', '1983-02-23', 'Británica');

INSERT INTO clientes (nombre, apellido, dni, e_mail, Activo) VALUES 
-- Originales (1..20)
('Juan', 'Pérez', 30123456, 'juan.perez@email.com', 1),
('María', 'Gómez', 31234567, 'maria.gomez@email.com', 1),
('Carlos', 'López', 32345678, 'carlos.lopez@email.com', 1),
('Ana', 'Martínez', 33456789, 'ana.martinez@email.com', 1),
('Luis', 'Rodríguez', 34567890, 'luis.rodriguez@email.com', 1),
('Laura', 'Fernández', 35678901, 'laura.fernandez@email.com', 1),
('Diego', 'García', 36789012, 'diego.garcia@email.com', 1),
('Sofía', 'Sánchez', 37890123, 'sofia.sanchez@email.com', 1),
('Martín', 'Romero', 38901234, 'martin.romero@email.com', 1),
('Lucía', 'Sosa', 39012345, 'lucia.sosa@email.com', 1),
('Pablo', 'Díaz', 40123456, 'pablo.diaz@email.com', 1),
('Valeria', 'Torres', 41234567, 'valeria.torres@email.com', 1),
('Javier', 'Ruiz', 42345678, 'javier.ruiz@email.com', 1),
('Micaela', 'Alonso', 43456789, 'micaela.alonso@email.com', 1),
('Facundo', 'Iglesias', 44567890, 'facundo.iglesias@email.com', 1),
('Camila', 'Giménez', 45678901, 'camila.gimenez@email.com', 1),
('Nicolás', 'Silva', 46789012, 'nicolas.silva@email.com', 1),
('Agustina', 'Vargas', 47890123, 'agustina.vargas@email.com', 1),
('Lucas', 'Castro', 48901234, 'lucas.castro@email.com', 1),
('Florencia', 'Molina', 49012345, 'florencia.molina@email.com', 1),
-- 10 Nuevos (21..30)
('Mateo', 'Benítez', 41555111, 'mateo.benitez@email.com', 1),
('Valentina', 'Acosta', 42666222, 'valentina.acosta@email.com', 1),
('Santiago', 'Medina', 33777333, 'santiago.medina@email.com', 1),
('Julieta', 'Herrera', 39888444, 'julieta.herrera@email.com', 1),
('Tomás', 'Aguirre', 44999555, 'tomas.aguirre@email.com', 1),
('Milagros', 'Pereyra', 45111666, 'milagros.pereyra@email.com', 1),
('Gonzalo', 'Rojas', 29222777, 'gonzalo.rojas@email.com', 1),
('Rocío', 'Ortiz', 37333888, 'rocio.ortiz@email.com', 1),
('Federico', 'Núñez', 31444999, 'federico.nunez@email.com', 1),
('Catalina', 'Ríos', 46555000, 'catalina.rios@email.com', 0);

-- 3. PELÍCULAS (20), SALAS (10), PROMOCIONES (3) Y PRODUCTOS (15)

INSERT INTO peliculas (titulo, id_genero, duracion, fecha_estreno, id_clasificacion, sinopsis, id_director) VALUES 
-- Originales (1..10)
('Spider-Man: El Origen', 1, 120, '2023-05-10', 2, 'Un joven es picado por una araña y obtiene poderes.', 1),
('La Odisea del Espacio', 5, 145, '2023-06-15', 1, 'Viaje intergaláctico para salvar la humanidad.', 2),
('El Misterio de la Casa', 4, 105, '2023-10-31', 3, 'Una familia se muda a una casa encantada.', 8),
('Amor en París', 9, 110, '2023-02-14', 2, 'Dos extraños se conocen en la ciudad del amor.', 10),
('Aventuras en la Jungla', 6, 130, '2023-07-20', 1, 'Exploradores buscan un tesoro perdido.', 5),
('Guerra de Clanes', 1, 150, '2023-11-05', 4, 'Mafias rivales se enfrentan por el control.', 3),
('Dinosaurios Animados', 7, 95, '2024-01-10', 1, 'Divertida historia para toda la familia.', 6),
('El Juicio Final', 3, 160, '2023-09-12', 3, 'Drama legal intenso basado en hechos reales.', 4),
('Más Allá del Tiempo', 5, 135, '2024-03-01', 2, 'Viajes en el tiempo alteran la realidad.', 9),
('Magia y Espadas', 10, 140, '2023-12-25', 2, 'Un reino de fantasía en peligro.', 6),
-- 10 Nuevas (11..20)
('Gladiador: El Regreso', 1, 155, '2025-03-15', 3, 'Un guerrero busca justicia en la arena de Roma.', 11),
('La Sombra del Detective', 8, 138, '2025-06-20', 3, 'Un investigador sigue pistas de un caso sin resolver.', 12),
('El Bosque de los Espíritus', 7, 118, '2025-08-10', 1, 'Una niña descubre un mundo mágico habitado por criaturas.', 13),
('El Secreto de Buenos Aires', 3, 129, '2025-10-05', 2, 'Un fiscal reabre un expediente oculto durante décadas.', 16),
('Furia en el Desierto', 1, 142, '2026-02-12', 3, 'Supervivientes cruzan el páramo en vehículos blindados.', 18),
('Sinfonía Nocturna', 3, 124, '2026-05-14', 2, 'Un joven pianista arriesga todo por su debut internacional.', 15),
('Crónicas de Dune: Horizonte', 5, 162, '2026-09-18', 2, 'La batalla por el control del planeta desértico se intensifica.', 9),
('Relatos del Sur', 2, 115, '2026-11-05', 3, 'Cuatro historias cruzadas sobre situaciones límite.', 17),
('La Mansión Gótica', 4, 112, '2026-11-19', 3, 'Un excéntrico inventor recibe visitas inesperadas en Halloween.', 19),
('El Gran Hotel Alpino', 2, 104, '2026-12-10', 1, 'Un conserje y su aprendiz protegen una pintura invaluable.', 20);

INSERT INTO salas (descripcion, id_tipo_sala, capacidad) VALUES 
-- Originales (1..5)
('Sala 1 - Principal', 1, 30),
('Sala 2 - Clásica', 1, 30),
('Sala 3 - 3D Experiencia', 2, 20),
('Sala 4 - 3D Premium', 2, 20),
('Sala 5 - 4D Inmersiva', 3, 10),
-- 5 Nuevas (6..10)
('Sala 6 - Confort 2D', 1, 30),
('Sala 7 - Familiar 2D', 1, 30),
('Sala 8 - 3D RealD', 2, 20),
('Sala 9 - 3DIMAX', 2, 20),
('Sala 10 - 4D E-Motion', 3, 10);

INSERT INTO promociones (dia, hora_inicio, hora_fin, id_tipo_descuento) VALUES 
('Lunes', NULL, NULL, 1),
('Miércoles', '14:00:00', '19:00:00', 2),
('Viernes', '11:00:00', '14:00:00', 3);

-- Productos: mismos 15 registros, con stock y stock_minimo variados
INSERT INTO productos (descripcion, pre_unitario, id_tipo_producto, id_tamaño_producto, stock, stock_minimo, Activo) VALUES 
('Agua Mineral sin gas', 1500.00, 2, 2, 85, 20, 1),
('Jugo de Naranja', 1800.00, 2, 2, 42, 15, 1),
('Jugo de Manzana', 1800.00, 2, 2, 12, 15, 1), -- Stock bajo
('Jugo Multifruta', 1800.00, 2, 2, 36, 12, 1),
('Jugo de Durazno', 1800.00, 2, 2, 28, 10, 1),
('Coca Cola', 2000.00, 2, 1, 95, 25, 1),
('Coca Cola', 2500.00, 2, 2, 110, 30, 1),
('Coca Cola', 3000.00, 2, 3, 64, 20, 1),
('Sprite', 2500.00, 2, 2, 50, 18, 1),
('Fanta', 2500.00, 2, 2, 14, 15, 1), -- Stock bajo
('Pochoclo Salado', 3500.00, 1, 1, 70, 20, 1),
('Pochoclo Salado', 4500.00, 1, 2, 55, 15, 1),
('Pochoclo Dulce', 4500.00, 1, 2, 48, 15, 1),
('Nachos con Queso', 5000.00, 1, 2, 9, 12, 1),  -- Stock bajo
('Pancho Clásico', 3000.00, 1, 2, 40, 10, 1);

-- 4. DETALLES PELÍCULA-ACTOR Y BUTACAS (220 butacas en total)

INSERT INTO detalles_pelicula_actor (id_pelicula, id_actor) VALUES 
-- Películas 1..10
(1, 11), (1, 12), (2, 20), (2, 21), (3, 7), 
(4, 9), (4, 10), (5, 17), (5, 18), (6, 1), 
(6, 2), (6, 3), (7, 4), (8, 14), (8, 15), 
(9, 8), (10, 22), (10, 23), (10, 24),
-- Películas 11..20
(11, 33), (11, 6), (12, 2), (12, 39), (13, 26),
(14, 31), (14, 32), (15, 34), (15, 6), (16, 10),
(16, 35), (17, 35), (17, 12), (17, 36), (18, 31),
(18, 32), (19, 37), (19, 38), (20, 40), (20, 28);

-- Butacas Salas 1..5 (IDs 1..110)
INSERT INTO butacas (fila, columna, id_sala) VALUES 
-- Sala 1 (IDs 1..30)
('A',1,1), ('A',2,1), ('A',3,1), ('A',4,1), ('A',5,1), ('A',6,1), ('A',7,1), ('A',8,1), ('A',9,1), ('A',10,1),
('B',1,1), ('B',2,1), ('B',3,1), ('B',4,1), ('B',5,1), ('B',6,1), ('B',7,1), ('B',8,1), ('B',9,1), ('B',10,1),
('C',1,1), ('C',2,1), ('C',3,1), ('C',4,1), ('C',5,1), ('C',6,1), ('C',7,1), ('C',8,1), ('C',9,1), ('C',10,1),
-- Sala 2 (IDs 31..60)
('A',1,2), ('A',2,2), ('A',3,2), ('A',4,2), ('A',5,2), ('A',6,2), ('A',7,2), ('A',8,2), ('A',9,2), ('A',10,2),
('B',1,2), ('B',2,2), ('B',3,2), ('B',4,2), ('B',5,2), ('B',6,2), ('B',7,2), ('B',8,2), ('B',9,2), ('B',10,2),
('C',1,2), ('C',2,2), ('C',3,2), ('C',4,2), ('C',5,2), ('C',6,2), ('C',7,2), ('C',8,2), ('C',9,2), ('C',10,2),
-- Sala 3 (IDs 61..80)
('A',1,3), ('A',2,3), ('A',3,3), ('A',4,3), ('A',5,3), ('A',6,3), ('A',7,3), ('A',8,3), ('A',9,3), ('A',10,3),
('B',1,3), ('B',2,3), ('B',3,3), ('B',4,3), ('B',5,3), ('B',6,3), ('B',7,3), ('B',8,3), ('B',9,3), ('B',10,3),
-- Sala 4 (IDs 81..100)
('A',1,4), ('A',2,4), ('A',3,4), ('A',4,4), ('A',5,4), ('A',6,4), ('A',7,4), ('A',8,4), ('A',9,4), ('A',10,4),
('B',1,4), ('B',2,4), ('B',3,4), ('B',4,4), ('B',5,4), ('B',6,4), ('B',7,4), ('B',8,4), ('B',9,4), ('B',10,4),
-- Sala 5 (IDs 101..110)
('A',1,5), ('A',2,5), ('A',3,5), ('A',4,5), ('A',5,5), ('A',6,5), ('A',7,5), ('A',8,5), ('A',9,5), ('A',10,5);

-- Butacas Salas 6..10 (IDs 111..220)
INSERT INTO butacas (fila, columna, id_sala) VALUES 
-- Sala 6 (IDs 111..140)
('A',1,6), ('A',2,6), ('A',3,6), ('A',4,6), ('A',5,6), ('A',6,6), ('A',7,6), ('A',8,6), ('A',9,6), ('A',10,6),
('B',1,6), ('B',2,6), ('B',3,6), ('B',4,6), ('B',5,6), ('B',6,6), ('B',7,6), ('B',8,6), ('B',9,6), ('B',10,6),
('C',1,6), ('C',2,6), ('C',3,6), ('C',4,6), ('C',5,6), ('C',6,6), ('C',7,6), ('C',8,6), ('C',9,6), ('C',10,6),
-- Sala 7 (IDs 141..170)
('A',1,7), ('A',2,7), ('A',3,7), ('A',4,7), ('A',5,7), ('A',6,7), ('A',7,7), ('A',8,7), ('A',9,7), ('A',10,7),
('B',1,7), ('B',2,7), ('B',3,7), ('B',4,7), ('B',5,7), ('B',6,7), ('B',7,7), ('B',8,7), ('B',9,7), ('B',10,7),
('C',1,7), ('C',2,7), ('C',3,7), ('C',4,7), ('C',5,7), ('C',6,7), ('C',7,7), ('C',8,7), ('C',9,7), ('C',10,7),
-- Sala 8 (IDs 171..190)
('A',1,8), ('A',2,8), ('A',3,8), ('A',4,8), ('A',5,8), ('A',6,8), ('A',7,8), ('A',8,8), ('A',9,8), ('A',10,8),
('B',1,8), ('B',2,8), ('B',3,8), ('B',4,8), ('B',5,8), ('B',6,8), ('B',7,8), ('B',8,8), ('B',9,8), ('B',10,8),
-- Sala 9 (IDs 191..210)
('A',1,9), ('A',2,9), ('A',3,9), ('A',4,9), ('A',5,9), ('A',6,9), ('A',7,9), ('A',8,9), ('A',9,9), ('A',10,9),
('B',1,9), ('B',2,9), ('B',3,9), ('B',4,9), ('B',5,9), ('B',6,9), ('B',7,9), ('B',8,9), ('B',9,9), ('B',10,9),
-- Sala 10 (IDs 211..220)
('A',1,10), ('A',2,10), ('A',3,10), ('A',4,10), ('A',5,10), ('A',6,10), ('A',7,10), ('A',8,10), ('A',9,10), ('A',10,10);

-- 5. FUNCIONES (25 en total: 10 de 2024 + 5 de 2025 + 5 cercanas a hoy Nov 2026 + 5 futuras Dic 2026 / Ene 2027)

INSERT INTO funciones (fecha, id_pelicula, id_sala) VALUES 
-- Originales 2024 (IDs 1..10)
('2024-05-13 16:00:00', 1, 1),  -- 1: Lunes (Promo 1)
('2024-05-15 15:00:00', 2, 2),  -- 2: Miércoles tarde (Promo 2)
('2024-05-17 12:30:00', 3, 3),  -- 3: Viernes mediodía (Promo 3)
('2024-05-18 20:00:00', 1, 5),  -- 4: Sábado
('2024-05-18 22:00:00', 6, 4),  -- 5: Sábado
('2024-05-19 14:00:00', 7, 1),  -- 6: Domingo
('2024-05-20 18:00:00', 8, 2),  -- 7: Lunes (Promo 1)
('2024-05-22 21:00:00', 5, 3),  -- 8: Miércoles noche
('2024-05-24 13:00:00', 9, 4),  -- 9: Viernes mediodía (Promo 3)
('2024-05-25 19:30:00', 10, 5), -- 10: Sábado
-- 5 Funciones 2025 (IDs 11..15)
('2025-04-14 19:00:00', 11, 8), -- 11: Lunes (Promo 1, Sala 8 3D)
('2025-07-09 16:30:00', 12, 6), -- 12: Miércoles tarde (Promo 2, Sala 6 2D)
('2025-08-22 12:00:00', 13, 7), -- 13: Viernes mediodía (Promo 3, Sala 7 2D)
('2025-10-18 21:30:00', 14, 1), -- 14: Sábado (Sala 1 2D)
('2025-11-29 22:00:00', 11, 10),-- 15: Sábado (Sala 10 4D)
-- 5 Funciones 2026 cercanas a hoy 24/11/2026 (IDs 16..20)
('2026-11-18 17:00:00', 17, 9), -- 16: Miércoles tarde (Promo 2, Sala 9 3D)
('2026-11-20 13:00:00', 18, 6), -- 17: Viernes mediodía (Promo 3, Sala 6 2D)
('2026-11-23 20:00:00', 19, 8), -- 18: Lunes (Promo 1, Sala 8 3D)
('2026-11-24 19:30:00', 17, 10),-- 19: Hoy Martes 24/11 (Sala 10 4D)
('2026-11-25 16:00:00', 19, 7), -- 20: Mañana Miércoles 25/11 tarde (Promo 2, Sala 7 2D)
-- 5 Funciones posteriores a hoy: Dic 2026 y Ene 2027 (IDs 21..25)
('2026-12-11 12:30:00', 20, 6), -- 21: Viernes mediodía (Promo 3, Sala 6 2D)
('2026-12-14 21:00:00', 20, 9), -- 22: Lunes (Promo 1, Sala 9 3D)
('2026-12-20 18:00:00', 17, 10),-- 23: Domingo (Sala 10 4D)
('2027-01-06 15:30:00', 20, 8), -- 24: Miércoles tarde (Promo 2, Sala 8 3D)
('2027-01-16 22:00:00', 15, 5); -- 25: Sábado (Sala 5 4D)

-- 6. COMPROBANTES (30 en total: 6 originales + 24 nuevos)

INSERT INTO comprobantes (id_cliente, fecha, id_forma_pago, Activo) VALUES 
-- Originales (IDs 1..6)
(1, '2024-05-13 15:50:00', 1, 1),
(2, '2024-05-15 14:45:00', 2, 1),
(3, '2024-05-16 10:00:00', 3, 1),
(3, '2024-05-17 12:20:00', 1, 1),
(4, '2024-05-18 19:50:00', 2, 1),
(5, '2024-05-19 13:40:00', 1, 1),
-- 24 Nuevos (IDs 7..30)
(8,  '2025-04-14 18:40:00', 2, 1), -- 7
(9,  '2025-04-14 18:50:00', 3, 1), -- 8
(10, '2025-07-09 16:15:00', 1, 1), -- 9
(11, '2025-08-22 11:45:00', 2, 1), -- 10
(12, '2025-10-18 21:10:00', 3, 1), -- 11
(13, '2025-11-29 21:40:00', 1, 1), -- 12
(14, '2025-11-29 21:45:00', 2, 0), -- 13 (Anulado - borrado lógico)
(15, '2026-11-18 16:40:00', 3, 1), -- 14
(16, '2026-11-18 16:50:00', 1, 1), -- 15 (Solo productos)
(17, '2026-11-20 12:40:00', 2, 1), -- 16
(18, '2026-11-20 12:50:00', 3, 1), -- 17
(19, '2026-11-23 19:30:00', 1, 1), -- 18
(20, '2026-11-23 19:45:00', 2, 1), -- 19
(21, '2026-11-24 09:15:00', 3, 1), -- 20 (Hoy)
(22, '2026-11-24 09:40:00', 2, 1), -- 21 (Hoy)
(23, '2026-11-24 10:05:00', 1, 1), -- 22 (Hoy - Solo productos)
(24, '2026-11-24 10:15:00', 3, 1), -- 23 (Hoy - Compra anticipada para mañana)
(25, '2026-11-24 10:20:00', 2, 0), -- 24 (Anulado - borrado lógico)
(26, '2026-11-25 15:30:00', 1, 1), -- 25
(27, '2026-12-11 12:10:00', 3, 1), -- 26
(28, '2026-12-14 20:30:00', 2, 1), -- 27
(29, '2026-12-20 17:40:00', 3, 1), -- 28
(1,  '2027-01-06 15:10:00', 1, 1), -- 29
(4,  '2027-01-16 21:30:00', 2, 1); -- 30

-- 7. RESERVAS (30 en total: 3 originales + 27 nuevos)
-- id_estado: 1:Pendiente, 2:Confirmada, 3:Vencida, 4:Cancelada

INSERT INTO reservas (id_cliente, fecha_reserva, id_estado) VALUES 
-- Originales (IDs 1..3)
(3,  '2024-05-14 09:00:00', 2), -- 1: Confirmada
(6,  '2024-05-18 10:00:00', 1), -- 2: Pendiente
(7,  '2024-05-16 14:00:00', 3), -- 3: Vencida
-- 27 Nuevas (IDs 4..30)
(8,  '2025-04-12 10:00:00', 2), -- 4: Confirmada (Comprobante 7)
(9,  '2025-04-13 15:00:00', 3), -- 5: Vencida
(10, '2025-07-08 11:30:00', 2), -- 6: Confirmada (Comprobante 9)
(11, '2025-08-20 18:00:00', 4), -- 7: Cancelada
(12, '2025-10-17 09:45:00', 2), -- 8: Confirmada (Comprobante 11)
(13, '2025-11-28 16:20:00', 3), -- 9: Vencida
(14, '2025-11-28 20:00:00', 4), -- 10: Cancelada
(15, '2026-11-17 14:00:00', 2), -- 11: Confirmada (Comprobante 14)
(16, '2026-11-17 19:10:00', 3), -- 12: Vencida
(17, '2026-11-19 10:15:00', 2), -- 13: Confirmada (Comprobante 16)
(18, '2026-11-19 21:00:00', 4), -- 14: Cancelada
(19, '2026-11-22 12:30:00', 2), -- 15: Confirmada (Comprobante 18)
(20, '2026-11-23 11:00:00', 3), -- 16: Vencida
(21, '2026-11-23 18:45:00', 2), -- 17: Confirmada (Comprobante 20)
(22, '2026-11-24 08:30:00', 1), -- 18: Pendiente (Función de hoy a las 19:30)
(23, '2026-11-24 09:00:00', 1), -- 19: Pendiente (Función de hoy a las 19:30)
(24, '2026-11-24 09:30:00', 2), -- 20: Confirmada (Comprobante 23 - Función de mañana)
(25, '2026-11-24 10:00:00', 1), -- 21: Pendiente (Función de mañana)
(26, '2026-11-24 10:10:00', 4), -- 22: Cancelada
(27, '2026-12-09 14:00:00', 2), -- 23: Confirmada (Comprobante 26)
(28, '2026-12-10 17:00:00', 1), -- 24: Pendiente
(29, '2026-12-12 11:20:00', 2), -- 25: Confirmada (Comprobante 27)
(1,  '2026-12-13 19:00:00', 1), -- 26: Pendiente
(2,  '2026-12-18 15:10:00', 2), -- 27: Confirmada (Comprobante 28)
(5,  '2027-01-04 10:30:00', 1), -- 28: Pendiente
(10, '2027-01-05 16:00:00', 2), -- 29: Confirmada (Comprobante 29)
(15, '2027-01-14 13:45:00', 1); -- 30: Pendiente

-- 8. ENTRADAS
-- Precios base según tipos_sala: 2D = 10000 | 3D = 13000 | 4D = 16000
-- Descuentos: Promo 1 (Lunes) = -10% | Promo 2 (Miércoles 14-19h) = -20% | Promo 3 (Viernes 11-14h) = -50%

INSERT INTO entradas (id_funcion, id_butaca, edad, precio_cobrado, id_comprobante, id_promocion) VALUES 
-- Originales (IDs 1..11)
(1, 1,   25, 4500.00, 1, 1),
(1, 2,   28, 4500.00, 1, 1),
(2, 31,  18, 4000.00, 2, 2),
(3, 61,  30, 3250.00, 3, 3),
(3, 62,  30, 3250.00, 3, 3),
(4, 101, 45, 8000.00, 5, NULL),
(4, 102, 12, 8000.00, 5, NULL),
(6, 3,   22, 5000.00, 6, NULL),
(5, 81,  20, 6500.00, NULL, NULL),
(5, 82,  20, 6500.00, NULL, NULL),
(8, 63,  35, 6500.00, NULL, NULL),

-- Nuevas Entradas 2025 (IDs 12..23)
(11, 171, 29, 11700.00, 7, 1),     -- Func 11 (Sala 8 3D: 13000 - 10%) | Comp 7 (Reserva 4)
(11, 172, 27, 11700.00, 7, 1),     -- Func 11 | Comp 7 (Reserva 4)
(11, 173, 34, 11700.00, 8, 1),     -- Func 11 | Comp 8
(11, 174, 40, 11700.00, NULL, 1),  -- Func 11 | Reserva 5 (Vencida)
(12, 111, 24, 8000.00,  9, 2),     -- Func 12 (Sala 6 2D: 10000 - 20%) | Comp 9 (Reserva 6)
(12, 112, 26, 8000.00,  9, 2),     -- Func 12 | Comp 9 (Reserva 6)
(13, 141, 10, 5000.00,  10, 3),    -- Func 13 (Sala 7 2D: 10000 - 50%) | Comp 10
(13, 142, 38, 5000.00,  10, 3),    -- Func 13 | Comp 10
(13, 143, 19, 5000.00,  NULL, 3),  -- Func 13 | Reserva 7 (Cancelada)
(14, 4,   31, 10000.00, 11, NULL), -- Func 14 (Sala 1 2D: 10000) | Comp 11 (Reserva 8)
(15, 211, 23, 16000.00, 12, NULL), -- Func 15 (Sala 10 4D: 16000) | Comp 12
(15, 212, 25, 16000.00, NULL, NULL),-- Func 15 | Reserva 9 (Vencida)

-- Nuevas Entradas Noviembre 2026 (IDs 24..43)
(16, 191, 28, 10400.00, 14, 2),    -- Func 16 (Sala 9 3D: 13000 - 20%) | Comp 14 (Reserva 11)
(16, 192, 29, 10400.00, 14, 2),    -- Func 16 | Comp 14 (Reserva 11)
(16, 193, 33, 10400.00, NULL, 2),  -- Func 16 | Reserva 12 (Vencida)
(17, 113, 42, 5000.00,  16, 3),    -- Func 17 (Sala 6 2D: 10000 - 50%) | Comp 16 (Reserva 13)
(17, 114, 41, 5000.00,  17, 3),    -- Func 17 | Comp 17
(17, 115, 21, 5000.00,  NULL, 3),  -- Func 17 | Reserva 14 (Cancelada)
(18, 175, 27, 11700.00, 18, 1),    -- Func 18 (Sala 8 3D: 13000 - 10%) | Comp 18 (Reserva 15)
(18, 176, 30, 11700.00, 19, 1),    -- Func 18 | Comp 19
(18, 177, 19, 11700.00, NULL, 1),  -- Func 18 | Reserva 16 (Vencida)
(19, 213, 32, 16000.00, 20, NULL), -- Func 19 Hoy (Sala 10 4D: 16000) | Comp 20 (Reserva 17)
(19, 214, 31, 16000.00, 20, NULL), -- Func 19 Hoy | Comp 20 (Reserva 17)
(19, 215, 24, 16000.00, 21, NULL), -- Func 19 Hoy | Comp 21
(19, 216, 22, 16000.00, NULL, NULL),-- Func 19 Hoy | Reserva 18 (Pendiente)
(19, 217, 29, 16000.00, NULL, NULL),-- Func 19 Hoy | Reserva 19 (Pendiente)
(20, 144, 17, 8000.00,  23, 2),    -- Func 20 Mañana (Sala 7 2D: 10000 - 20%) | Comp 23 (Reserva 20)
(20, 145, 18, 8000.00,  23, 2),    -- Func 20 Mañana | Comp 23 (Reserva 20)
(20, 146, 26, 8000.00,  25, 2),    -- Func 20 Mañana | Comp 25
(20, 147, 35, 8000.00,  NULL, 2),  -- Func 20 Mañana | Reserva 21 (Pendiente)
(20, 148, 44, 8000.00,  NULL, 2),  -- Func 20 Mañana | Reserva 22 (Cancelada)

-- Nuevas Entradas Dic 2026 / Ene 2027 (IDs 44..56)
(21, 116, 14, 5000.00,  26, 3),    -- Func 21 (Sala 6 2D: 10000 - 50%) | Comp 26 (Reserva 23)
(21, 117, 46, 5000.00,  26, 3),    -- Func 21 | Comp 26 (Reserva 23)
(21, 118, 28, 5000.00,  NULL, 3),  -- Func 21 | Reserva 24 (Pendiente)
(22, 194, 33, 11700.00, 27, 1),    -- Func 22 (Sala 9 3D: 13000 - 10%) | Comp 27 (Reserva 25)
(22, 195, 25, 11700.00, NULL, 1),  -- Func 22 | Reserva 26 (Pendiente)
(23, 218, 37, 16000.00, 28, NULL), -- Func 23 (Sala 10 4D: 16000) | Comp 28 (Reserva 27)
(23, 219, 36, 16000.00, 28, NULL), -- Func 23 | Comp 28 (Reserva 27)
(24, 178, 21, 10400.00, 29, 2),    -- Func 24 (Sala 8 3D: 13000 - 20%) | Comp 29 (Reserva 29)
(24, 179, 20, 10400.00, 29, 2),    -- Func 24 | Comp 29 (Reserva 29)
(24, 180, 29, 10400.00, NULL, 2),  -- Func 24 | Reserva 28 (Pendiente)
(25, 103, 39, 16000.00, 30, NULL), -- Func 25 (Sala 5 4D: 16000) | Comp 30
(25, 104, 41, 16000.00, 30, NULL), -- Func 25 | Comp 30
(25, 105, 24, 16000.00, NULL, NULL);-- Func 25 | Reserva 30 (Pendiente)

-- 9. DETALLES RESERVA - ENTRADA (Asocia las 30 reservas con sus entradas)

INSERT INTO detalles_reserva_entrada (id_reserva, id_entrada) VALUES 
-- Originales (Reservas 1..3)
(1, 4), (1, 5),
(2, 9), (2, 10),
(3, 11),
-- Nuevas (Reservas 4..30)
(4, 12), (4, 13),
(5, 15),
(6, 16), (6, 17),
(7, 20),
(8, 21),
(9, 23),
(10, 22),
(11, 24), (11, 25),
(12, 26),
(13, 27),
(14, 29),
(15, 30),
(16, 32),
(17, 33), (17, 34),
(18, 36),
(19, 37),
(20, 38), (20, 39),
(21, 41),
(22, 42),
(23, 44), (23, 45),
(24, 46),
(25, 47),
(26, 48),
(27, 49), (27, 50),
(28, 53),
(29, 51), (29, 52),
(30, 56);

-- 10. DETALLES COMPROBANTE - PRODUCTOS

INSERT INTO detalles_comprobante_productos (id_comprobante, id_producto, cantidad_productos, precio_unitario) VALUES 
-- Originales
(4, 12, 1, 4500.00),
(4, 7,  2, 2500.00),
(5, 14, 1, 5000.00),
(5, 9,  1, 2500.00),
(6, 11, 2, 3500.00),
(6, 1,  1, 1500.00),
-- Nuevos detalles de productos en comprobantes
(7,  13, 1, 4500.00), -- Pochoclo Dulce M
(7,  8,  2, 3000.00), -- 2 Coca Cola G
(9,  15, 2, 3000.00), -- 2 Pancho Clásico
(9,  2,  2, 1800.00), -- 2 Jugo de Naranja
(10, 11, 2, 3500.00), -- 2 Pochoclo Salado P
(10, 6,  2, 2000.00), -- 2 Coca Cola P
(12, 14, 1, 5000.00), -- Nachos con Queso
(12, 10, 1, 2500.00), -- Fanta M
(14, 12, 2, 4500.00), -- 2 Pochoclo Salado M
(14, 7,  2, 2500.00), -- 2 Coca Cola M
(15, 13, 1, 4500.00), -- Pochoclo Dulce M (Compra solo candy)
(15, 1,  1, 1500.00), -- Agua Mineral
(16, 15, 1, 3000.00), -- Pancho Clásico
(16, 4,  1, 1800.00), -- Jugo Multifruta
(18, 8,  1, 3000.00), -- Coca Cola G
(20, 14, 2, 5000.00), -- 2 Nachos con Queso
(20, 8,  2, 3000.00), -- 2 Coca Cola G
(21, 12, 1, 4500.00), -- Pochoclo Salado M
(21, 9,  1, 2500.00), -- Sprite M
(22, 11, 3, 3500.00), -- 3 Pochoclo Salado P (Compra solo candy)
(22, 5,  3, 1800.00), -- 3 Jugo de Durazno
(25, 13, 1, 4500.00), -- Pochoclo Dulce M
(27, 14, 1, 5000.00), -- Nachos con Queso
(28, 12, 2, 4500.00), -- 2 Pochoclo Salado M
(28, 7,  2, 2500.00), -- 2 Coca Cola M
(30, 15, 2, 3000.00), -- 2 Pancho Clásico
(30, 8,  2, 3000.00); -- 2 Coca Cola G
GO