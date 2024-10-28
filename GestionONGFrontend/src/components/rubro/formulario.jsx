import { useState, useContext, useEffect } from "react";
import { Link } from "react-router-dom";
import { ContentContext } from "./context";

const Formulario = () => {
  const { toggleModal, showModal, createUpdate, oneData, opcion } =
    useContext(ContentContext);

  const [nombre, setNombre] = useState("");

  const handleSubmit = (e) => {
    e.preventDefault();
    createUpdate({
      id: oneData?.id,
      nombre: nombre,
    });
    toggleModal(0);
  };

  useEffect(() => {
    if (opcion >= 1) {
      setNombre(oneData?.nombre);
    }
  }, [opcion, oneData]);

  return (
    <>
      <Link to="/">
        <button
          className="btn btn-light position-absolute top-0 start-0 ms-2 mt-2"
          onClick={() => toggleModal(0)}
        >
          <i className="bi bi-arrow-left" />
        </button>
      </Link>
      <button
        className="btn btn-primary position-absolute top-0 end-0 me-2 mt-2"
        onClick={() => {
          toggleModal(0);
          setNombre("");
        }}
      >
        <i className="bi bi-plus" />
      </button>

      <div
        className={`modal fade ${showModal ? "show" : ""}`}
        style={{ display: showModal ? "block" : "none" }}
        tabIndex="-1"
        role="dialog"
      >
        <div className="modal-dialog modal-dialog-centered" role="document">
          <div className="modal-content">
            <div className="modal-header">
              <h5 className="modal-title">Rubro</h5>
              <button
                className="btn btn-light position-absolute top-0 end-0"
                onClick={() => toggleModal(0)}
              >
                <i className="bi bi-x" style={{ fontSize: "20px" }} />
              </button>
            </div>
            <div className="modal-body">
              <form onSubmit={handleSubmit}>
                <div className="mb-3">
                  <label htmlFor="nombre" className="form-label">
                    Nombre del Rubro <span className="text-danger">*</span>
                  </label>
                  <input
                    type="text"
                    className="form-control"
                    id="nombre"
                    value={nombre}
                    onChange={(e) => setNombre(e.target.value)}
                    disabled={opcion === 2}
                    required
                  />
                </div>

                {opcion <= 1 && (
                  <button
                    type="submit"
                    className={`btn btn-${
                      opcion === 0 ? "success" : "warning"
                    }`}
                  >
                    {opcion === 0 ? "Crear" : "Actualizar"}
                  </button>
                )}
              </form>
            </div>
          </div>
        </div>
      </div>
      {showModal && (
        <div className="modal-backdrop fade show" onClick={toggleModal}></div>
      )}
    </>
  );
};

export default Formulario;
