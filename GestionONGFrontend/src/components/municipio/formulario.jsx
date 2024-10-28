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
    departamentos,
    getDepartamentos,
  } = useContext(ContentContext);

  const [nombre, setNombre] = useState("");
  const [selectedDepartamento, setSelectedDepartamento] = useState("");

  const handleSubmit = (e) => {
    e.preventDefault();
    createUpdate({
      id: oneData?.id,
      nombre: nombre,
      idDepartamento: selectedDepartamento,
    });
    toggleModal(0);
  };

  useEffect(() => {
    getDepartamentos();
    if (opcion >= 1) {
      setNombre(oneData?.nombre);
      setSelectedDepartamento(oneData?.idDepartamento || "");
    } else {
      setNombre("");
      setSelectedDepartamento("");
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
          setSelectedDepartamento("");
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
              <h5 className="modal-title">Municipio</h5>
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
                    Nombre del Municipio <span className="text-danger">*</span>
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
                <div className="mb-3">
                  <label htmlFor="departamento" className="form-label">
                    Departamento <span className="text-danger">*</span>
                  </label>
                  <select
                    id="departamento"
                    className="form-select"
                    value={selectedDepartamento}
                    onChange={(e) => setSelectedDepartamento(e.target.value)}
                    disabled={opcion === 2}
                    required
                  >
                    <option value="">Seleccione un departamento</option>
                    {departamentos.map((dept) => (
                      <option key={dept.id} value={dept.id}>
                        {dept.nombre}
                      </option>
                    ))}
                  </select>
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
