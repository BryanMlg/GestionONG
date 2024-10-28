import { useContext, useEffect, useState } from "react";
import { ContentContext } from "./context";
import Loading from "../../loading";
import { Link } from "react-router-dom";

const List = () => {
  const {
    loading,
    error,
    allData,
    setSelectedProyecto,
    selectedProyecto,
    proyectos,
  } = useContext(ContentContext);

  if (loading) return <Loading />;
  if (error) return <p className="text-danger text-center">Error: {error}</p>;

  return (
    <div className="container mt-5 mb-3">
      <Link to="/consultas">
        <button className="btn btn-light position-absolute top-0 start-0 ms-2 mt-2">
          <i className="bi bi-arrow-left" />
        </button>
      </Link>
      <h1 className="text-center mb-4 mt-5">Lista de Disponibilidad</h1>
      <div className="mb-4">
        <label htmlFor="proyectoSelect" className="form-label">
          Selecciona un Proyecto:
        </label>
        <select
          id="proyectoSelect"
          className="form-select w-auto"
          value={selectedProyecto}
          onChange={(e) => setSelectedProyecto(e.target.value)}
        >
          <option value="">-- Selecciona un Proyecto --</option>
          {proyectos.map((proyecto) => (
            <option key={proyecto.id} value={proyecto.id}>
              {proyecto.nombre}
            </option>
          ))}
        </select>
      </div>

      <div className="row">
        {allData.map((proyecto) => {
          const disponibilidadFondos = proyecto.TotalDonaciones - proyecto.TotalGastos;
          const porcentajeDisponibilidad = (disponibilidadFondos / proyecto.TotalDonaciones) * 100;

          return (
            <div key={proyecto.id} className="col-lg-4 col-md-6 col-sm-12 mb-4">
              <div className="card shadow-sm h-100">
                <div className="card-header text-white bg-primary d-flex justify-content-between align-items-center">
                  <span>{proyecto.NombreRubro}</span>
                  <i className="bi bi-building ms-2"></i>
                </div>
                <div className="card-body">
                  <h5 className="card-title text-center text-success">
                    <i className="bi bi-cash-stack me-2"></i>Fondos Recibidos: Q
                    {proyecto.TotalDonaciones.toLocaleString("es-ES", {
                      style: "currency",
                      currency: "GTQ",
                    }) || "Q" + 0.0}
                  </h5>
                  <p className="card-text text-danger">
                    <strong>
                      <i className="bi bi-exclamation-triangle me-2"></i>Total
                      Gastado:
                    </strong>{" "}
                    {proyecto.TotalGastos.toLocaleString("es-ES", {
                      style: "currency",
                      currency: "GTQ",
                    }) || "Q" + 0.0}
                  </p>
                  <p className="card-text">
                    <strong>
                      <i className="bi bi-percent me-2"></i>Disponible:
                    </strong>{" "}
                    {porcentajeDisponibilidad || 0}
                  </p>
                  <div className="progress mt-3" style={{ height: "15px" }}>
                    <div
                      className="progress-bar bg-success"
                      role="progressbar"
                      style={{ width: `${Math.max(0, porcentajeDisponibilidad)}%` }} // Asegúrate de que no sea negativo
                      aria-valuenow={porcentajeDisponibilidad}
                      aria-valuemin="0"
                      aria-valuemax="100"
                    >
                      {porcentajeDisponibilidad > 0 && Math.max(0, porcentajeDisponibilidad).toFixed(2) + '%'}
                    </div>
                  </div>
                </div>
              </div>
            </div>
          );
        })}
      </div>
    </div>
  );
};

export default List;
