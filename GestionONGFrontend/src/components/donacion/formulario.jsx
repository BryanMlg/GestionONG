import { useState, useContext, useEffect } from "react";
import { Link } from "react-router-dom";
import { ContentContext } from "./context";

const Formulario = () => {
  const {
    toggleModal,
    showModal,
    createUpdate,
    oneData,
    opcion,
    proyectos,
    getProyectos,
    getRubros,
    rubros,
  } = useContext(ContentContext);

  const [idProyecto, setIdProyecto] = useState("");
  const [idRubro, setIdRubro] = useState("");
  const [fechaDonacion, setFechaDonacion] = useState("");
  const [nombreDonante, setNombreDonante] = useState("");
  const [monto, setMonto] = useState("");

  const handleSubmit = (e) => {
    e.preventDefault();
    createUpdate({
      id: oneData?.id,
      idProyecto,
      idRubro,
      fechaDonacion,
      nombreDonante,
      monto: parseFloat(monto),
    });
    toggleModal(0);
  };

  useEffect(() => {
    getProyectos();
    if (opcion >= 1) {
      setIdProyecto(oneData?.idProyecto);
      setIdRubro(oneData?.idRubro);
      setFechaDonacion(oneData?.fechaDonacion);
      setNombreDonante(oneData?.nombreDonante);
      setMonto(oneData?.monto);
    }
  }, [opcion, oneData]);

  useEffect(() => {
    getRubros(idProyecto);
  }, [idProyecto]);

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
          setIdProyecto("");
          setIdRubro("");
          setFechaDonacion("");
          setNombreDonante("");
          setMonto("");
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
              <h5 className="modal-title">Registrar Donación</h5>
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
                  <label htmlFor="idProyecto" className="form-label">
                    Proyecto <span className="text-danger">*</span>
                  </label>
                  <select
                    id="idProyecto"
                    className="form-select"
                    value={idProyecto}
                    onChange={(e) => setIdProyecto(e.target.value)}
                    required
                  >
                    <option value="">Seleccione un proyecto</option>
                    {proyectos.map((proyecto) => (
                      <option key={proyecto.id} value={proyecto.id}>
                        {proyecto.nombre}
                      </option>
                    ))}
                  </select>
                </div>

                <div className="mb-3">
                  <label htmlFor="idRubro" className="form-label">
                    Rubro <span className="text-danger">*</span>
                  </label>
                  <select
                    id="idRubro"
                    className="form-select"
                    value={idRubro}
                    onChange={(e) => setIdRubro(e.target.value)}
                    required
                  >
                    <option value="">Seleccione un rubro</option>
                    {rubros.map((rubro) => (
                      <option key={rubro.idRubro} value={rubro.idRubro}>
                        {rubro.nombreRubro}
                      </option>
                    ))}
                  </select>
                </div>

                <div className="mb-3">
                  <label htmlFor="fechaDonacion" className="form-label">
                    Fecha de Donación <span className="text-danger">*</span>
                  </label>
                  <input
                    type="date"
                    className="form-control"
                    id="fechaDonacion"
                    value={fechaDonacion}
                    onChange={(e) => setFechaDonacion(e.target.value)}
                    required
                  />
                </div>

                <div className="mb-3">
                  <label htmlFor="nombreDonante" className="form-label">
                    Nombre del Donante <span className="text-danger">*</span>
                  </label>
                  <input
                    type="text"
                    className="form-control"
                    id="nombreDonante"
                    value={nombreDonante}
                    onChange={(e) => setNombreDonante(e.target.value)}
                    required
                  />
                </div>

                <div className="mb-3">
                  <label htmlFor="monto" className="form-label">
                    Monto <span className="text-danger">*</span>
                  </label>
                  <input
                    type="number"
                    className="form-control"
                    id="monto"
                    value={monto}
                    onChange={(e) => setMonto(e.target.value)}
                    required
                    step="0.01"
                  />
                </div>

                <button
                  type="submit"
                  className={`btn btn-${opcion === 0 ? "success" : "warning"}`}
                >
                  {opcion === 0 ? "Registrar" : "Actualizar"}
                </button>
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
