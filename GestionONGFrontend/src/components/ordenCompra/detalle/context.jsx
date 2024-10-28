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
  const [proyectos, setProyectos] = useState([]);
  const [oneData, setOneData] = useState(null);
  const [editar, setEditar] = useState(false);
  const getAllData = async (page) => {
    setLoading(true);
    setError(null);
    try {
      const result = await request("POST", "DetalleOrdenCompra/all", {
        pageNumber: page,
        pageSize: pageSize,
        id: oneDataPrincipal?.id,
      });
      setAllData(result?.data?.Detalles);
      setTotalRecords(result?.data?.TotalRegistros);
      setTotalPages(result?.data?.TotalPaginas);
    } catch (error) {
      console.error("Error fetching Detalles:", error.message);
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
        `DetalleOrdenCompra/${oneData?.id ? "update" : "create"}`,
        {
          id: oneData?.id,
          idRubro: data?.idRubro,
          idOrdenCompra: oneDataPrincipal?.id,
          nombreProducto: data?.nombreProducto,
          monto: data?.monto,
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

  const getOneData = async (data) => {
    setLoading(true);
    setError(null);
    try {
      const result = await request(`POST`, "DetalleOrdenCompra/one", {
        id: data?.id,
      });
      setOneData(result.data);
    } catch (error) {
      setError(error.message);
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

  const getRubros = async (data) => {
    setLoading(true);
    setError(null);
    try {
      const result = await request(
        "POST",
        "ProyectoRubro/labelProyectoRubros",
        data
      );
      setRubros(result?.data?.ProyectoRubros);
    } catch (error) {
    } finally {
      setLoading(false);
    }
  };

  const dropItem = async (data) => {
    setLoading(true);
    setError(null);
    try {
      const result = await request(`DELETE`, `DetalleOrdenCompra/delete`, {
        id: data?.id,
      });
      showNotification(`Rubro eliminado exitosamente.`, "success");
    } catch (error) {
      console.error("Error fetching Detalles:", error.message);
      setError(error.message);
      showNotification(`${error?.message}`, "error");
    } finally {
      getAllData(currentPage);
      setLoading(false);
    }
  };

  useEffect(() => {
    getAllData(currentPage);
    getProyectos();
    getRubros(oneDataPrincipal?.idProyecto);
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
    setEditar,
    getOneData,
    oneData,
    editar,
    proyectos,
    getRubros,
    oneDataPrincipal,
  };

  return (
    <ContentContext.Provider value={value}>{children}</ContentContext.Provider>
  );
};
