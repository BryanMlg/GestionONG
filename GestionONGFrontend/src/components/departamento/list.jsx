import React, { useContext, useState } from "react";
import { ContentContext } from "./context";
import Loading from "../loading";
import Confirmacion from "../common/confirmacion";

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
  const [showModal, setShowModal] = useState(false);
  const [itemToDelete, setItemToDelete] = useState(null);

  const handlePageSizeChange = (e) => {
    const newSize = parseInt(e.target.value);
    setPageSizeLocal(newSize);
    setPageSize(newSize);
  };

  const handleDeleteClick = (departamento) => {
    setItemToDelete(departamento);
    setShowModal(true);
  };

  const handleConfirmDelete = () => {
    if (itemToDelete) {
      dropItem(itemToDelete);
    }
    setShowModal(false);
    setItemToDelete(null);
  };

  if (loading) return <Loading />;
  if (error) return <p className="text-danger text-center">Error: {error}</p>;

  return (
    <div className="container mt-5 mb-3">
      <h1 className="text-center mb-4 mt-5">Lista de Departamentos</h1>
      <div className="mb-3">
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
        {allData.map((departamento) => (
          <div key={departamento.id} className="col-md-4 col-sm-6 mb-3">
            <div
              className="card clickable"
              onClick={(e) => {
                e.stopPropagation();
                getOneData(departamento, 3);
              }}
              title="Visualizar Detalles"
              style={{ cursor: "pointer" }}
            >
              <div className="card-body d-flex justify-content-between align-items-center">
                <i className="bi bi-arrow-up-short icon-list" />
                <span>{departamento.nombre}</span>
                <div className="dropdown">
                  <button
                    className="btn btn-light"
                    type="button"
                    id={`dropdownMenuButton-${departamento.id}`}
                    data-bs-toggle="dropdown"
                    aria-expanded="false"
                    onClick={(e) => e.stopPropagation()}
                  >
                    <i className="bi bi-three-dots-vertical"></i>
                  </button>
                  <ul
                    className="dropdown-menu"
                    aria-labelledby={`dropdownMenuButton-${departamento.id}`}
                  >
                    <li>
                      <button
                        className="dropdown-item text-primary"
                        onClick={(e) => {
                          e.stopPropagation();
                          getOneData(departamento, 2);
                        }}
                      >
                        <i className="bi bi-eye me-2"></i> Ver
                      </button>
                    </li>
                    <li>
                      <button
                        className="dropdown-item text-warning"
                        onClick={(e) => {
                          e.stopPropagation();
                          getOneData(departamento, 1);
                        }}
                      >
                        <i className="bi bi-pencil me-2"></i> Editar
                      </button>
                    </li>
                    <li>
                      <button
                        className="dropdown-item text-danger"
                        onClick={(e) => {
                          e.stopPropagation();
                          handleDeleteClick(departamento);
                        }}
                      >
                        <i className="bi bi-trash me-2"></i> Eliminar
                      </button>
                    </li>
                  </ul>
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
      <Confirmacion
        show={showModal}
        handleClose={() => setShowModal(false)}
        handleConfirm={handleConfirmDelete}
        itemName={itemToDelete ? itemToDelete.nombre : ""}
      />
    </div>
  );
};

export default List;
