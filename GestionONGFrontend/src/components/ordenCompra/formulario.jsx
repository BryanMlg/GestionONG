import { useState, useContext, useEffect } from "react";
import { ContentContext } from "./context";
import { Link } from "react-router-dom";
import Detalle from "./detalle/index";
const Formulario = () => {
  const {
    toggleModal,
    showModal,
    proyectos,
    createUpdate,
    opcion,
    oneData,
    getAllData,
  } = useContext(ContentContext);
  const [selectedProyecto, setSelectedProyecto] = useState("");
  const [proveedor, setProveedor] = useState("");
  const [fechaOrden, setFechaOrden] = useState("");

  const handleSubmit = (e) => {
    e.preventDefault();
    createUpdate({
      idProyecto: selectedProyecto,
      proveedor: proveedor,
      fechaOrden: fechaOrden,
      montoTotal: 0,
    });
    toggleModal(0);
  };

  useEffect(() => {
    if (opcion >= 1) {
      setSelectedProyecto(oneData?.idProyecto);
      setProveedor(oneData?.proveedor);
      setFechaOrden(oneData?.fechaOrden);
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
          setSelectedProyecto("");
          setProveedor("");
          setFechaOrden("");
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
        <div
          className={`modal-dialog modal-dialog-centered ${
            opcion === 3 ? "modal-lg" : ""
          }`}
          role="document"
        >
          <div className="modal-content">
            <div className="modal-header">
              <h5 className="modal-title">Orden de Compra</h5>
              <button
                className="btn btn-light position-absolute top-0 end-0"
                onClick={() => {
                  toggleModal(0);
                  getAllData(1);
                }}
              >
                <i className="bi bi-x" style={{ fontSize: "20px" }} />
              </button>
            </div>
            <div className="modal-body">
              {opcion != 3 ? (
                <form onSubmit={handleSubmit}>
                  <div className="mb-3">
                    <label htmlFor="proyecto" className="form-label">
                      Proyecto <span className="text-danger">*</span>
                    </label> 
                    <select
                      id="proyecto"
                      className="form-select"
                      value={selectedProyecto}
                      onChange={(e) => setSelectedProyecto(e.target.value)}
                      required
                    >
                      <option value="">Seleccione un proyecto</option>
                      {proyectos.map((proj) => (
                        <option key={proj.id} value={proj.id}>
                          {proj.nombre}
                        </option>
                      ))}
                    </select>
                  </div>

                  <div className="mb-3">
                    <label htmlFor="proveedor" className="form-label">
                      Proveedor <span className="text-danger">*</span>
                    </label>
                    <input
                      type="text"
                      className="form-control"
                      id="proveedor"
                      value={proveedor}
                      onChange={(e) => setProveedor(e.target.value)}
                      required
                    />
                  </div>

                  <div className="mb-3">
                    <label htmlFor="fechaOrden" className="form-label">
                      Fecha de Orden <span className="text-danger">*</span>
                    </label>
                    <input
                      type="date"
                      className="form-control"
                      id="fechaOrden"
                      value={fechaOrden}
                      onChange={(e) => setFechaOrden(e.target.value)}
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
              ) : (
                <Detalle />
              )}
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
