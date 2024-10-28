import React from "react";

const Loading = () => (
  <div className="d-flex justify-content-center align-items-center my-5">
    <div className="spinner-border text-primary" role="status">
      <span className="visually-hidden">Cargando...</span>
    </div>
  </div>
);

export default Loading;
