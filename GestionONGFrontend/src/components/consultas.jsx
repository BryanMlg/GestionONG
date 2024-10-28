import { useContext } from "react";
import CustomCard from "./customCard";
import CustomRadio from "./customRadio";
import { ContentContext } from "../context/appContex";
const Consultas = () => {
  const { options } = useContext(ContentContext);
  return (
    <>
      <h2 className="fw-bold text-center mt-3">Consultas</h2>
      <div className="d-flex justify-content-center align-items-center mt-3">
        <CustomRadio options={options} />
      </div>

      <div className="row row-cols-lg-2 row-cols-md-2 row-cols-sm-2 row-cols-1 mb-5 mt-3">
        <div className="col">
          <CustomCard
            title="Porcentaje"
            description=""
            icon="bi bi-percent"
            size={50}
            link={'/porcentaje'}
          />
        </div>

        <div className="col">
          <CustomCard
            title="Disponibilidad"
            description=""
            icon="bi bi-question-lg"
            size={50}
            link={'/disponibilidad'}
          />
        </div>
      </div>
    </>
  );
};

export default Consultas;
