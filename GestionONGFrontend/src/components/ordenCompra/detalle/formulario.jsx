import { useState, useContext, useEffect } from "react";
import { ContentContext } from "./context";

const Formulario = () => {
  const { createUpdate, rubros, oneData, editar, setEditar, getRubros, oneDataPrincipal } =
    useContext(ContentContext);

  const [selectedRubro, setSelectedRubro] = useState("");
  const [nombreProducto, setNombreProducto] = useState("");
  const [monto, setMonto] = useState("");

  const handleSubmit = (e) => {
    e.preventDefault();
    createUpdate({
      idRubro: selectedRubro,
      nombreProducto,
      monto: parseFloat(monto),
    });
    setEditar(false);
    setSelectedRubro("");
    setNombreProducto("");
    setMonto("");
  };

  useEffect(() => {
    if (editar) {
      setSelectedRubro(oneData?.idRubro);
      setNombreProducto(oneData?.nombreProducto);
      setMonto(oneData?.monto);
    } else {
      setSelectedRubro("");
      setNombreProducto("");
      setMonto("");
    }
  }, [editar]);



  return (
    <div className="container d-flex justify-content-center align-items-center">
      <form onSubmit={handleSubmit}>
        <div className="mb-3 row">
          <div className="col-sm-12 col-md-6 mt-3">
            <label htmlFor="rubro" className="form-label text-center">
              Rubro <span className="text-danger">*</span>
            </label>
            <select
              id="rubro"
              className="form-select"
              value={selectedRubro}
              onChange={(e) => setSelectedRubro(e.target.value)}
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

          <div className="col-sm-12 col-md-6 mt-3">
            <label htmlFor="nombreProducto" className="form-label text-center">
              Nombre del Producto <span className="text-danger">*</span>
            </label>
            <input
              id="nombreProducto"
              className="form-control"
              type="text"
              value={nombreProducto}
              onChange={(e) => setNombreProducto(e.target.value)}
              required
            />
          </div>

          <div className="col-sm-12 mt-3">
            <label htmlFor="monto" className="form-label text-center">
              Monto <span className="text-danger">*</span>
            </label>
            <input
              id="monto"
              className="form-control"
              type="number"
              min="0"
              step="0.01"
              value={monto}
              onChange={(e) => setMonto(e.target.value)}
              required
            />
          </div>
        </div>
        <div className="d-flex justify-content-center">
          <button
            type="submit"
            className={`btn btn-${editar ? "warning" : "primary"}`}
          >
            {editar ? (
              <i className="bi bi-pencil-square" />
            ) : (
              <i className="bi bi-plus-lg" />
            )}
          </button>
          {editar && (
            <button
              type="button"
              className="btn btn-danger ms-3"
              onClick={() => setEditar(false)}
            >
              X
            </button>
          )}
        </div>
      </form>
    </div>
  );
};

export default Formulario;
