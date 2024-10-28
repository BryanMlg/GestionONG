import "./css/main.css";
import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import { useEffect } from "react";
import { useState } from "react";
import Gestiones from "./components/gestiones";
import Consultas from "./components/consultas";
import Departamento from "./components/departamento/index";
import Municipio from "./components/municipio/index";
import Rubro from "./components/rubro/index";
import Proyecto from "./components/proyecto/index";
import Donacion from "./components/donacion/index";
import OrdenCompra from "./components/ordenCompra/index";
import Porcentaje from "./components/consultas/porcentaje/index";
import Disponibilidad from "./components/consultas/disponibilidad/index";
import Notification from "./components/common/notification";

function App() {
  const [dark, setDark] = useState(false);
  const toggleTheme = () => {
    const currentTheme = document.documentElement.getAttribute("data-bs-theme");
    document.documentElement.setAttribute(
      "data-bs-theme",
      currentTheme === "dark" ? "light" : "dark"
    );
    setDark(!dark);
  };

  useEffect(() => {
    if (dark) {
      document.body.classList.add("dark-theme");
    } else {
      document.body.classList.remove("dark-theme");
    }
  }, [dark]);

  return (
    <Router>
      <section className="main-container d-flex justify-content-center align-items-center">
        <button
          className={`btn btn-${
            dark ? "dark" : "light"
          } position-absolute top-0 mt-5`}
          onClick={toggleTheme}
        >
          <i className={`bi ${dark ? "bi-brightness-high" : "bi-moon"}`}></i>
        </button>
        <Notification />
        <div className="card card-custom">
          <Routes>
            <Route path="/" element={<Gestiones />} />
            <Route path="/gestiones" element={<Gestiones />} />
            <Route path="/consultas" element={<Consultas />} />
            <Route path="/departamento" element={<Departamento />} />
            <Route path="/municipio" element={<Municipio />} />
            <Route path="/rubro" element={<Rubro />} />
            <Route path="/proyecto" element={<Proyecto />} />
            <Route path="/donacion" element={<Donacion />} />
            <Route path="/orden-compra" element={<OrdenCompra />} />
            <Route path="/porcentaje" element={<Porcentaje />} />
            <Route path="/disponibilidad" element={<Disponibilidad />} />
          </Routes>
        </div>
      </section>
    </Router>
  );
}

export default App;
