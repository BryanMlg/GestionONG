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
  } = useContext(ContentContext);

  const [pageSize, setPageSizeLocal] = useState(10);

  const handlePageSizeChange = (e) => {
    const newSize = parseInt(e.target.value);
    setPageSizeLocal(newSize);
    setPageSize(newSize);
  };

  if (loading) return <Loading />;
  if (error) return <p className="text-danger text-center">Error: {error}</p>;

  return (
    <div className="container mb-3">
      <Formulario />
      <hr />
      <div className="mb-3 w-25">
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

      <div className="d-flex justify-content-between mb-3">
        <span>
          Total de registros: <strong>{totalRecords}</strong>
        </span>
        <span>
          Página <strong>{currentPage}</strong> de <strong>{totalPages}</strong>
        </span>
      </div>

      <div className="row">
        {allData.map((proyectoRubro) => (
          <div key={proyectoRubro.idRubro} className="col-md-6 col-sm-12 mb-3">
            <div className="card shadow-sm">
              <div className="card-body d-flex flex-column">
                <h5 className="card-title d-flex align-items-center justify-content-between">
                  <span>
                    <i className="bi bi-building-fill me-2"></i>
                    {proyectoRubro.nombreRubro}
                  </span>
                  <span className="text-muted">
                    <i className="bi bi-cash-coin me-1"></i>
                    {proyectoRubro.presupuesto.toLocaleString("es-ES", {
                      style: "currency",
                      currency: "GTQ",
                    })}
                  </span>
                </h5>
                <hr />
                <div className="d-flex justify-content-end">
                  <button
                    className="btn btn-outline-danger btn-sm"
                    onClick={() => dropItem(proyectoRubro)}
                    title="Eliminar"
                  >
                    <i className="bi bi-trash-fill"></i> Eliminar
                  </button>
                </div>
              </div>
            </div>
          </div>
        ))}
      </div>

      <div className="d-flex justify-content-end mt-4">
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
