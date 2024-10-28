import React, { createContext, useEffect, useState, useContext } from "react";
import { request } from "../../../service/api";
import { ContentContext as PrincipalContext } from "../context";
import { ContentContext as mainContext } from "../../../context/appContex";
export const ContentContext = createContext();

export const ContentProvider = ({ children }) => {
  const { oneData: oneDataPrincipal } = useContext(PrincipalContext);
  const { showNotification } = useContext(mainContext);
  const [allData, setAllData] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [currentPage, setCurrentPage] = useState(1);
  const [totalRecords, setTotalRecords] = useState(0);
  const [totalPages, setTotalPages] = useState(0);
  const [pageSize, setPageSize] = useState(10);
  const [rubros, setRubros] = useState([]);
  const [oneData, setOneData] = useState(null);
  const getAllData = async (page) => {
    setLoading(true);
    setError(null);
    try {
      const result = await request("POST", "ProyectoRubro/all", {
        pageNumber: page,
        pageSize: pageSize,
        id: oneDataPrincipal?.id,
      });
      setAllData(result?.data?.ProyectoRubros);
      setTotalRecords(result?.data?.TotalRegistros);
      setTotalPages(result?.data?.TotalPaginas);
    } catch (error) {
      console.error("Error fetching ProyectoRubros:", error.message);
    } finally {
      setLoading(false);
    }
  };

  const createUpdate = async (data) => {
    setLoading(true);
    setError(null);
    try {
      const result = await request(
        `${oneData?.id ? "PUT" : "POST"}`,
        `ProyectoRubro/${oneData?.id ? "update" : "create"}`,
        {
          idRubro: data?.idRubro,
          idProyecto: oneDataPrincipal?.id,
        }
      );
      showNotification(
        `Rubro ${
          oneDataPrincipal?.id ? "actualizado" : "agregado"
        } exitosamente.`,
        "success"
      );
    } catch (error) {
      setError(error.message);
      showNotification(`${error?.message}`, "error");
    } finally {
      getAllData(currentPage);
      setLoading(false);
    }
  };

  const getRubros = async () => {
    setLoading(true);
    setError(null);
    try {
      const result = await request("GET", "Rubro/label");
      setRubros(result);
    } catch (error) {
    } finally {
      setLoading(false);
    }
  };

  const dropItem = async (data) => {
    setLoading(true);
    setError(null);
    try {
      const result = await request(`DELETE`, `ProyectoRubro/delete`, {
        id: data?.id,
      });
      showNotification(`Rubro eliminado exitosamente.`, "success");
    } catch (error) {
      console.error("Error fetching ProyectoRubros:", error.message);
      setError(error.message);
      showNotification(`${error?.message}`, "error");
    } finally {
      getAllData(currentPage);
      setLoading(false);
    }
  };

  useEffect(() => {
    getAllData(currentPage);
    getRubros();
  }, [currentPage, pageSize]);

  const nextPage = () => {
    if (currentPage < totalPages) {
      setCurrentPage((prev) => prev + 1);
    }
  };

  const previousPage = () => {
    if (currentPage > 1) {
      setCurrentPage((prev) => prev - 1);
    }
  };

  const value = {
    allData,
    loading,
    error,
    totalRecords,
    totalPages,
    currentPage,
    nextPage,
    previousPage,
    setPageSize,
    pageSize,
    dropItem,
    rubros,
    createUpdate,
  };

  return (
    <ContentContext.Provider value={value}>{children}</ContentContext.Provider>
  );
};
