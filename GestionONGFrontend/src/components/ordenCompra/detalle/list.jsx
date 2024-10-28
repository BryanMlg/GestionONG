import { useContext, useState } from "react";
import { ContentContext } from "./context";
import Loading from "../../loading";
import Formulario from "./formulario";

const List = () => {
  const {
    loading,
    error,
    allData,
    nextPage,
    previousPage,
    totalRecords,
    currentPage,
    totalPages,
    setPageSize,
    dropItem,
    setEditar,
    getOneData,
  } = useContext(ContentContext);

  const [pageSize, setPageSizeLocal] = useState(10);
  const [isAccordionOpen, setIsAccordionOpen] = useState(false);

  const handlePageSizeChange = (e) => {
    const newSize = parseInt(e.target.value);
    setPageSizeLocal(newSize);
    setPageSize(newSize);
  };

  const handleEditClick = async (proyectoRubro) => {
    await getOneData(proyectoRubro);
    setEditar(true);
    setIsAccordionOpen(true); // Abre el acordeón al hacer clic en "Editar"
  };

  if (loading) return <Loading />;
  if (error) return <p className="text-danger text-center">Error: {error}</p>;

  return (
    <div className="container mb-3">
      <div className="accordion mb-3" id="accordionFormulario">
        <div className="accordion-item">
          <h2 className="accordion-header" id="headingFormulario">
            <button
              className={`accordion-button ${isAccordionOpen ? "" : "collapsed"}`}
              type="button"
              data-bs-toggle="collapse"
              data-bs-target="#collapseFormulario"
              aria-expanded={isAccordionOpen}
              aria-controls="collapseFormulario"
              onClick={() => setIsAccordionOpen(!isAccordionOpen)}
            >
              Agregar Nuevo Producto
            </button>
          </h2>
          <div
            id="collapseFormulario"
            className={`accordion-collapse collapse ${isAccordionOpen ? "show" : ""}`}
            aria-labelledby="headingFormulario"
            data-bs-parent="#accordionFormulario"
          >
            <div className="accordion-body">
              <Formulario />
            </div>
          </div>
        </div>
      </div>

      <hr />

      <div className="row mb-3">
        <div className="col-md-6 mb-2">
          <label className="form-label">Items por página:</label>
          <select
            id="pageSize"
            className="form-select w-auto"
            value={pageSize}
            onChange={handlePageSizeChange}
          >
            <option value={5}>5</option>
            <option value={10}>10</option>
            <option value={20}>20</option>
            <option value={50}>50</option>
          </select>
        </div>

        <div className="col-md-6 text-md-end text-start">
          <span>
            Total de registros: <strong>{totalRecords}</strong>
          </span>
        </div>
      </div>

      <div className="row mb-3">
        <div className="col-12 text-center text-md-start">
          <span>
            Página <strong>{currentPage}</strong> de{" "}
            <strong>{totalPages}</strong>
          </span>
        </div>
      </div>

      <div className="row">
        {allData.map((proyectoRubro) => (
          <div
            key={proyectoRubro.idRubro}
            className="col-lg-6 col-md-6 col-sm-12 mb-3"
          >
            <div className="card shadow-sm">
              <div className="card-body text-center">
                <h5 className="card-title mb-3">{proyectoRubro.rubro}</h5>
                <p className="text-muted mb-1">
                  <i
                    className="bi bi-box"
                    style={{ fontSize: "1rem", color: "#6c757d" }}
                  ></i>{" "}
                  <strong>Producto:</strong> {proyectoRubro.nombreProducto}
                </p>
                <p
                  className="text-primary mb-1"
                  style={{ fontSize: "1.25rem" }}
                >
                  <i className="bi bi-cash-coin"></i>{" "}
                  {proyectoRubro.monto.toLocaleString("es-ES", {
                    style: "currency",
                    currency: "GTQ",
                  })}
                </p>
                <hr />
                <div className="d-flex">
                  <button
                    className="btn btn-outline-danger btn-sm w-100"
                    onClick={() => dropItem(proyectoRubro)}
                    title="Eliminar"
                  >
                    <i className="bi bi-trash-fill"></i> Eliminar
                  </button>
                  <button
                    className="btn btn-outline-warning btn-sm w-100 ms-2"
                    onClick={() => handleEditClick(proyectoRubro)}
                    title="Editar"
                  >
                    <i className="bi bi-pencil-square" /> Editar
                  </button>
                </div>
              </div>
            </div>
          </div>
        ))}
      </div>

      <div className="d-flex justify-content-between align-items-center mt-4">
        <button
          className="btn btn-secondary me-2"
          onClick={previousPage}
          disabled={currentPage === 1}
        >
          <i className="bi bi-arrow-left-circle-fill me-1"></i> Anterior
        </button>
        <button
          className="btn btn-secondary"
          onClick={nextPage}
          disabled={currentPage === totalPages}
        >
          Siguiente <i className="bi bi-arrow-right-circle-fill ms-1"></i>
        </button>
      </div>
    </div>
  );
};

export default List;
