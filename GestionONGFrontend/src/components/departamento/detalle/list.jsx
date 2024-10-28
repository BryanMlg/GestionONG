import { useContext, useState } from "react";
import { ContentContext } from "./context";
import Loading from "../../loading";
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
    getOneData,
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
        {allData.map((municipio) => (
          <div key={municipio.id} className="col-md-6 col-sm-6 mb-3">
            <div className="card">
              <div className="card-body d-flex justify-content-between align-items-center">
                <span>{municipio.nombre}</span>
                <div>
                  <i
                    className="bi bi-trash text-danger"
                    style={{ cursor: "pointer" }}
                    onClick={() => dropItem(municipio)}
                    title="Eliminar"
                  ></i>
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
          Anterior
        </button>
        <button
          className="btn btn-secondary"
          onClick={nextPage}
          disabled={currentPage === totalPages}
        >
          Siguiente
        </button>
      </div>
    </div>
  );
};

export default List;
