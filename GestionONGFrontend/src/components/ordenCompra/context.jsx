import React, { createContext, useEffect, useState, useContext } from "react";
import { request } from "../../service/api";
import { ContentContext as mainContext } from "../../context/appContex";
export const ContentContext = createContext();

export const ContentProvider = ({ children }) => {
  const { showNotification } = useContext(mainContext);
  const [allData, setAllData] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [currentPage, setCurrentPage] = useState(1);
  const [totalRecords, setTotalRecords] = useState(0);
  const [totalPages, setTotalPages] = useState(0);
  const [pageSize, setPageSize] = useState(10);
  const [showModal, setShowModal] = useState(false);
  const [opcion, setOpcion] = useState(0);
  const [oneData, setOneData] = useState(null);
  const [proyectos, setProyectos] = useState([]);
  const toggleModal = (opcion) => {
    if (opcion === 0) {
      setOneData("");
    }
    setOpcion(opcion);
    setShowModal(!showModal);
  };

  const getAllData = async (page) => {
    setLoading(true);
    setError(null);
    try {
      const result = await request("POST", "OrdenCompra/all", {
        pageNumber: page,
        pageSize: pageSize,
      });
      setAllData(result.data.OrdenesCompras);
      setTotalRecords(result.data.TotalRegistros);
      setTotalPages(result.data.TotalPaginas);
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
      showNotification(`${error?.message}`, "error");
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
        `OrdenCompra/${oneData?.id ? "update" : "create"}`,
        {
          id: oneData?.id,
          idProyecto: data?.idProyecto,
          proveedor: data?.proveedor,
          fechaOrden: data?.fechaOrden,
          montoTotal: data?.montoTotal,
        }
      );
      showNotification(
        `OrdenCompra ${oneData?.id ? "actualizado" : "creado"} exitosamente.`,
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

  const dropItem = async (data) => {
    setLoading(true);
    setError(null);
    try {
      const result = await request(`DELETE`, `OrdenCompra/delete`, {
        id: data?.id,
      });
      showNotification(`OrdenCompra eliminado exitosamente.`, "success");
    } catch (error) {
      setError(error.message);
      showNotification(`${error?.message}`, "error");
    } finally {
      getAllData(currentPage);
      setLoading(false);
    }
  };

  const getOneData = async (data, opcion) => {
    setLoading(true);
    setError(null);
    try {
      const result = await request(`POST`, "OrdenCompra/one", {
        id: data?.id,
      });
      setOneData(result.data);
    } catch (error) {
      setError(error.message);
    } finally {
      toggleModal(opcion);
      setLoading(false);
    }
  };

  useEffect(() => {
    getAllData(currentPage);
    getProyectos();
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
    createUpdate,
    toggleModal,
    opcion,
    showModal,
    getOneData,
    oneData,
    dropItem,
    proyectos,
    getAllData
  };

  return (
    <ContentContext.Provider value={value}>{children}</ContentContext.Provider>
  );
};
