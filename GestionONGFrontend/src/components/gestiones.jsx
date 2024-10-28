import { useContext } from "react";
import CustomCard from "./customCard";
import CustomRadio from "./customRadio";
import { ContentContext } from "../context/appContex";
const Gestiones = () => {
  const { options } = useContext(ContentContext);
  return (
    <>
      <h2 className="fw-bold text-center mt-3">Gestiones</h2>
      <div className="d-flex justify-content-center align-items-center mt-3">
        <CustomRadio options={options} />
      </div>
      <div className="row row-cols-lg-3 row-cols-md-3 row-cols-sm-2 row-cols-1 g-4 mb-5 mt-3">
        <div className="col">
          <CustomCard
            title="Departamento"
            description=""
            icon="bi bi-map-fill"
            size={75}
            link="/departamento" 
          />
        </div>
        <div className="col">
          <CustomCard
            title="Municipio"
            description=""
            icon="bi bi-map-fill"
            size={75}
            link="/municipio" 
          />
        </div>
        <div className="col">
          <CustomCard
            title="Proyecto"
            description=""
            icon="bi bi-box-fill"
            size={75}
            link="/proyecto" 
          />
        </div>
        <div className="col">
          <CustomCard
            title="Rubro"
            description=""
            icon="bi bi-collection"
            size={75}
            link="/rubro" 
          />
        </div>
        <div className="col">
          <CustomCard
            title="Donación"
            description=""
            icon="bi bi-patch-plus-fill"
            size={75}
            link="/donacion" 
          />
        </div>
        <div className="col">
          <CustomCard
            title="Orden Compra"
            description=""
            icon="bi bi-cart-plus-fill"
            size={75}
            link="/orden-compra"
          />
        </div>
      </div>
    </>
  );
};

export default Gestiones;
