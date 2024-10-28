import React from "react";
import { Link } from "react-router-dom";

const CustomCard = ({ title, description, icon, size, link }) => {
  return (
    <Link to={link} style={{ textDecoration: "none", color: "inherit" }}>
      <div
        className={`card w-${size} mx-auto shadow-lg`}
        style={{ cursor: "pointer" }}
      >
        <i
          className={`${icon} text-center mt-3`}
          style={{ fontSize: "2rem" }}
        />
        <div className="card-body d-flex flex-column">
          <h5 className="card-title fw-semibold text-center">{title}</h5>
          <p className="card-text text-center">{description}</p>
        </div>
      </div>
    </Link>
  );
};

export default CustomCard;
