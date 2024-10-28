import React, { createContext, useEffect, useState } from "react";
import { request } from "../../../service/api";
export const ContentContext = createContext();

export const ContentProvider = ({ children }) => {
  const [allData, setAllData] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [proyectos, setProyectos] = useState([]);
  const [selectedProyecto, setSelectedProyecto] = useState("");

  const getAllData = async () => {
    setLoading(true);
    setError(null);
    try {
      const result = await request(
        "POST",
        "Proyecto/Disponibilidad",
        selectedProyecto
      );
      setAllData(result.data);
    } catch (error) {
    } finally {
      setLoading(false);
    }
  };

  const getProyectos = async () => {
    setLoading(true);
    setError(null);
    try {
      const result = await request("GET", "Proyecto/label");
      setProyectos(result);
    } catch (error) {
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    getAllData();
  }, [selectedProyecto]);
  
  useEffect(() => {
    getProyectos();
  }, []);

  const value = {
    allData,
    loading,
    error,
    setSelectedProyecto,
    selectedProyecto,
    proyectos,
  };

  return (
    <ContentContext.Provider value={value}>{children}</ContentContext.Provider>
  );
};
