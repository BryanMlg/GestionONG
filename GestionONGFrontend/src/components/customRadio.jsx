import React, { useContext } from "react";
import { useNavigate } from "react-router-dom"; 
import "../css/customRadio.css";
import { ContentContext } from "../context/appContex";

const CustomRadio = ({ options }) => {
  const { selectedOption, handleChange } = useContext(ContentContext);
  const navigate = useNavigate(); 

  const handleRadioChange = (e) => {
    handleChange(e);
    navigate(`/${e.target.value}`); 
  };

  return (
    <div className="radio-inputs">
      {options.map((option) => (
        <label className="radio" key={option.value}>
          <input
            type="radio"
            name="radio"
            value={option.value}
            checked={selectedOption === option.value}
            onChange={handleRadioChange}
          />
          <span className="name">{option.label}</span>
        </label>
      ))}
    </div>
  );
};

export default CustomRadio;
