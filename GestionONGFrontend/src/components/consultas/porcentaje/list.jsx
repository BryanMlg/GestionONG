import { useContext } from "react";
import { ContentContext } from "./context";
import Loading from "../../loading";
import { Link } from "react-router-dom";
const List = () => {
  const { loading, error, allData } = useContext(ContentContext);

  if (loading) return <Loading />;
  if (error) return <p className="text-danger text-center">Error: {error}</p>;

  return (
    <div className="container mt-5 mb-3">
      <Link to="/consultas">
        <button className="btn btn-light position-absolute top-0 start-0 ms-2 mt-2">
          <i className="bi bi-arrow-left" />
        </button>
      </Link>
      <h1 className="text-center mb-4 mt-5">Lista de Proyectos</h1>
      <div className="row">
        {allData.map((proyecto) => (
          <div key={proyecto.id} className="col-lg-4 col-md-6 col-sm-12 mb-4">
            <div className="card shadow-sm h-100">
              <div className="card-header text-white bg-primary d-flex justify-content-between align-items-center">
                <span>{proyecto.NombreProyecto}</span>
                <i className="bi bi-building ms-2"></i>
              </div>
              <div className="card-body">
                <h5 className="card-title text-center text-success">
                  <i className="bi bi-cash-stack me-2"></i>Fondos Recibidos: Q
                  {proyecto.TotalFondosRecibidos.toLocaleString("es-ES", {
                    style: "currency",
                    currency: "GTQ",
                  }) || "Q" + 0.0}
                </h5>
                <p className="card-text text-danger">
                  <strong>
                    <i className="bi bi-exclamation-triangle me-2"></i>Total
                    Gastado:
                  </strong>{" "}
                  {proyecto.TotalGastado.toLocaleString("es-ES", {
                    style: "currency",
                    currency: "GTQ",
                  }) || "Q" + 0.0}
                </p>
                <p className="card-text">
                  <strong>
                    <i className="bi bi-percent me-2"></i>Porcentaje de
                    Ejecución:
                  </strong>{" "}
                  {proyecto.PorcentajeEjecucion.toFixed(2)}%
                </p>
                <div className="progress mt-3" style={{ height: "15px" }}>
                  <div
                    className="progress-bar bg-success"
                    role="progressbar"
                    style={{ width: `${proyecto.PorcentajeEjecucion}%` }}
                    aria-valuenow={proyecto.PorcentajeEjecucion}
                    aria-valuemin="0"
                    aria-valuemax="100"
                  >
                    {proyecto.PorcentajeEjecucion.toFixed(2)}%
                  </div>
                </div>
              </div>
            </div>
          </div>
        ))}
      </div>
    </div>
  );
};

export default List;
