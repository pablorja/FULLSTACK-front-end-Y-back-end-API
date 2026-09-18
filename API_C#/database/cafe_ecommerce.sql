CREATE DATABASE IF NOT EXISTS cafe_ecommerce
  CHARACTER SET utf8mb4
  COLLATE utf8mb4_unicode_ci;

USE cafe_ecommerce;

CREATE TABLE IF NOT EXISTS marcas (
    id INT NOT NULL AUTO_INCREMENT,
    nombre VARCHAR(100) NOT NULL,
    descripcion VARCHAR(255) NOT NULL DEFAULT '',
    PRIMARY KEY (id),
    UNIQUE KEY uq_marcas_nombre (nombre)
) ENGINE = InnoDB;

CREATE TABLE IF NOT EXISTS cafes (
    id INT NOT NULL AUTO_INCREMENT,
    marca_id INT NOT NULL,
    nombre VARCHAR(150) NOT NULL,
    origen VARCHAR(120) NOT NULL DEFAULT '',
    stock INT NOT NULL DEFAULT 0,
    precio DECIMAL(10, 2) NOT NULL DEFAULT 0.00,
    PRIMARY KEY (id),
    CONSTRAINT chk_cafes_stock CHECK (stock >= 0),
    CONSTRAINT chk_cafes_precio CHECK (precio >= 0),
    CONSTRAINT fk_cafes_marcas FOREIGN KEY (marca_id)
        REFERENCES marcas (id)
        ON UPDATE CASCADE
        ON DELETE RESTRICT
) ENGINE = InnoDB;

INSERT INTO marcas (id, nombre, descripcion) VALUES
    (1, 'Premium', 'Cafés de calidad superior'),
    (2, 'Especial', 'Cafés de origen seleccionado'),
    (3, 'Orgánico', 'Cafés cultivados de forma responsable')
ON DUPLICATE KEY UPDATE
    nombre = VALUES(nombre),
    descripcion = VALUES(descripcion);

INSERT INTO cafes (marca_id, nombre, origen, stock, precio) VALUES
    (1, 'Café Castillo', 'Colombia', 25, 10.07),
    (2, 'Café Pergamino', 'Colombia', 10, 20.08)
ON DUPLICATE KEY UPDATE
    nombre = VALUES(nombre),
    origen = VALUES(origen),
    stock = VALUES(stock),
    precio = VALUES(precio);

SELECT c.id, c.nombre, m.nombre AS marca, c.origen, c.stock, c.precio
FROM cafes c
INNER JOIN marcas m ON m.id = c.marca_id
ORDER BY c.id;
