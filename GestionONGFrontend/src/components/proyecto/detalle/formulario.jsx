import { useState, useContext } from "react";
import { ContentContext } from "./context";

const Formulario = () => {
  const { createUpdate, opcion, rubros } = useContext(ContentContext);

  const [selectedRubro, setSelectedRubro] = useState("");

  const handleSubmit = (e) => {
    e.preventDefault();
    createUpdate({
      idRubro: selectedRubro,
    });
  };

  return (
    <div className="container d-flex justify-content-center align-items-center">
      <form onSubmit={handleSubmit}>
        <div className="mb-3 row">
          <div className="col-sm-10 w-auto">
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
                <option key={rubro.id} value={rubro.id}>
                  {rubro.nombre}
                </option>
              ))}
            </select>
          </div>
          <div className="col-sm-2 d-flex align-items-end w-auto">
            <button type="submit" className="btn btn-primary">
              <i class="bi bi-plus-lg" />
            </button>
          </div>
        </div>
      </form>
    </div>
  );
};

export default Formulario;
