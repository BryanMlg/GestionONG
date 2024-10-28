CREATE TABLE Departamento (
    id INT PRIMARY KEY IDENTITY(1,1), 
    Nombre VARCHAR(100) NOT NULL 
);


CREATE TABLE Municipio (
    id INT PRIMARY KEY IDENTITY(1,1), 
    Nombre VARCHAR(100) NOT NULL,
    idDepartamento INT, 
    FOREIGN KEY (idDepartamento) REFERENCES Departamento(id) ON DELETE CASCADE
);

CREATE TABLE Proyecto (
    id INT PRIMARY KEY IDENTITY(1,1),
    Codigo VARCHAR(15) NOT NULL, 
    Nombre VARCHAR(100) NOT NULL, 
    idMunicipio INT, 
    idDepartamento INT, 
    FechaInicio DATE NOT NULL, 
    FechaFin DATE NOT NULL,
    FOREIGN KEY (idMunicipio) REFERENCES Municipio(id) ON DELETE NO ACTION, 
    FOREIGN KEY (idDepartamento) REFERENCES Departamento(id) ON DELETE NO ACTION
);



CREATE TABLE Rubro (
    id INT IDENTITY(1,1) PRIMARY KEY,
    Nombre VARCHAR(50) NOT NULL
);

CREATE TABLE ProyectoRubro (
    id INT PRIMARY KEY IDENTITY(1,1),  
    idProyecto INT NOT NULL,
    idRubro INT NOT NULL,
    FOREIGN KEY (idProyecto) REFERENCES Proyecto(id) ON DELETE NO ACTION,
    FOREIGN KEY (idRubro) REFERENCES Rubro(id) ON DELETE NO ACTION
);


CREATE TABLE Donacion (
    id INT IDENTITY(1,1) PRIMARY KEY,
    idProyecto INT NOT NULL,
    idRubro INT NOT NULL,
    FechaDonacion DATE NOT NULL,
    NombreDonante VARCHAR(100) NOT NULL,
    Monto DECIMAL(18, 2) NOT NULL,
    CONSTRAINT FK_Donacion_Proyecto FOREIGN KEY (idProyecto) REFERENCES Proyecto(id),
    CONSTRAINT FK_Donacion_Rubro FOREIGN KEY (idRubro) REFERENCES Rubro(id)
);

CREATE TABLE OrdenCompra (
    id INT IDENTITY(1,1) PRIMARY KEY,
    idProyecto INT NOT NULL,
    Proveedor VARCHAR(100) NOT NULL,
    FechaOrden DATE NOT NULL,
    MontoTotal DECIMAL(18, 2) NOT NULL,
    CONSTRAINT FK_OrdenCompra_Proyecto FOREIGN KEY (idProyecto) REFERENCES Proyecto(id)
);

CREATE TABLE DetalleOrdenCompra (
    id INT IDENTITY(1,1) PRIMARY KEY,
    idOrdenCompra INT NOT NULL,
    idRubro INT NOT NULL,
    Monto DECIMAL(18, 2) NOT NULL,
    CONSTRAINT FK_DetalleOrdenCompra_OrdenCompra FOREIGN KEY (idOrdenCompra) REFERENCES OrdenCompra(id),
    CONSTRAINT FK_DetalleOrdenCompra_Rubro FOREIGN KEY (idRubro) REFERENCES Rubro(id)
);

ALTER TABLE DetalleOrdenCompra
ADD nombreProducto VARCHAR(100) NULL;


ALTER TABLE ProyectoRubro
ADD Presupuesto DECIMAL(18,2) NOT NULL DEFAULT 0;



SELECT 
    r.id AS RubroId,
    r.Nombre AS NombreRubro,
    COALESCE(SUM(d.Monto), 0) AS TotalDonaciones,
    COALESCE(SUM(oc.MontoTotal), 0) AS TotalGastos
FROM 
    Rubro r
LEFT JOIN 
    ProyectoRubro pr ON r.id = pr.idRubro
LEFT JOIN 
    Donacion d ON pr.idProyecto = d.idProyecto AND pr.idRubro = d.idRubro
LEFT JOIN 
    OrdenCompra oc ON pr.idProyecto = oc.idProyecto
WHERE 
    pr.idProyecto = 8  -- Filtra por el proyecto específico
GROUP BY 
    r.id, r.Nombre; 
	
	--Obtener metricas de rubros de un proyecto en especifico (disponibilidad)

	SELECT 
    r.id AS RubroId,
    r.Nombre AS NombreRubro,
    COALESCE(SUM(d.Monto), 0) AS TotalDonaciones,
    COALESCE(SUM(doc.Monto), 0) AS TotalGastos,
    (COALESCE(SUM(d.Monto), 0) - COALESCE(SUM(doc.Monto), 0)) AS DisponibilidadFondos
FROM 
    Rubro r
LEFT JOIN 
    ProyectoRubro pr ON r.id = pr.idRubro
LEFT JOIN 
    Donacion d ON pr.idProyecto = d.idProyecto AND pr.idRubro = d.idRubro
LEFT JOIN 
    OrdenCompra oc ON pr.idProyecto = oc.idProyecto
LEFT JOIN 
    DetalleOrdenCompra doc ON oc.id = doc.idOrdenCompra AND pr.idRubro = doc.idRubro 
WHERE 
    pr.idProyecto = 8  -- Reemplaza @idProyecto con el ID del proyecto específico
GROUP BY 
    r.id, r.Nombre; 

	--Obtener metricas de rubros de un proyecto en especifico (Porcentaje accion)

	SELECT 
    p.id AS IdProyecto,
    p.Nombre AS NombreProyecto,
    ISNULL(SUM(d.Monto), 0) AS TotalFondosRecibidos,
    ISNULL((SELECT SUM(doc.Monto) 
            FROM DetalleOrdenCompra doc
            JOIN OrdenCompra oc ON doc.IdOrdenCompra = oc.Id
            WHERE oc.IdProyecto = p.id), 0) AS TotalGastado,
    CASE 
        WHEN ISNULL(SUM(d.Monto), 0) > 0 THEN 
            (ISNULL((SELECT SUM(doc.Monto) 
                      FROM DetalleOrdenCompra doc
                      JOIN OrdenCompra oc ON doc.IdOrdenCompra = oc.Id
                      WHERE oc.IdProyecto = p.id), 0) * 100.0) / ISNULL(SUM(d.Monto), 0)
        ELSE 
            0 
    END AS PorcentajeEjecucion
FROM Proyecto p
LEFT JOIN Donacion d ON p.id = d.IdProyecto
GROUP BY p.id, p.Nombre;