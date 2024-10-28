import React from "react";

const Confirmacion = ({ show, handleClose, handleConfirm, itemName }) => {
  return (
    <>
      {show && (
        <div
          className="modal-backdrop fade show"
          style={{ zIndex: 1040 }}
        ></div>
      )}
      <div
        className={`modal ${show ? "show" : ""}`}
        style={{ display: show ? "block" : "none", zIndex: 1050 }}
        tabIndex="-1"
        role="dialog"
      >
        <div className="modal-dialog modal-dialog-centered" role="document">
          <div className="modal-content">
            <div className="modal-header">
              <h5 className="modal-title">Confirmar Eliminación</h5>
            </div>
            <div className="modal-body">
              <p className="text-center">
                ¿Estás seguro de que deseas eliminar "
                <strong>{itemName}</strong>"?
              </p>
              <p className="text-center">
                Esta accion eliminara todos los municipios relacionados a este
                registro.
              </p>
            </div>
            <div className="modal-footer">
              <button
                type="button"
                className="btn btn-secondary"
                onClick={handleClose}
              >
                Cancelar
              </button>
              <button
                type="button"
                className="btn btn-danger"
                onClick={handleConfirm}
              >
                Eliminar
              </button>
            </div>
          </div>
        </div>
      </div>
    </>
  );
};

export default Confirmacion;
