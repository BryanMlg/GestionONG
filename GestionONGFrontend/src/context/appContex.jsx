import React, { createContext, useState } from "react";

export const ContentContext = createContext();

export const ContentProvider = ({ children }) => {
  const options = [
    { value: "gestiones", label: "Gestiones" },
    { value: "consultas", label: "Consultas" },
  ];
  const [selectedOption, setSelectedOption] = useState(options[0].value);
  const [menuOption, setMenuOption] = useState(null);
  const [notification, setNotification] = useState({ message: "", type: "" });
  const [darkMode, setDarkMode] = useState(false);
  const handleChange = (e) => {
    setSelectedOption(e.target.value);
  };

  const showNotification = (message, type) => {
    setNotification({ message, type });
  };

  const clearNotification = () => {
    setNotification({ message: "", type: "" });
  };

  const value = {
    handleChange,
    selectedOption,
    options,
    menuOption,
    setMenuOption,
    notification,
    showNotification,
    clearNotification,
    setSelectedOption,
    darkMode,
    setDarkMode,
  };

  return (
    <ContentContext.Provider value={value}>{children}</ContentContext.Provider>
  );
};
