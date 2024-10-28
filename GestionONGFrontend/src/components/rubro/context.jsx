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
      const result = await request("POST", "Rubro/all", {
        pageNumber: page,
        pageSize: pageSize,
      });
      setAllData(result.data.Rubros);
      setTotalRecords(result.data.TotalRegistros);
      setTotalPages(result.data.TotalPaginas);
    } catch (error) {
      setError(error.message);
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
        `Rubro/${oneData?.id ? "update" : "create"}`,
        {
          id: data?.id,
          nombre: data?.nombre,
        }
      );
      showNotification(
        `Rubro ${oneData?.id ? "actualizado" : "creado"} exitosamente.`,
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
      const result = await request(`DELETE`, `Rubro/delete`, {
        id: data?.id,
      });
      showNotification(`Rubro eliminado exitosamente.`, "success");
    } catch (error) {
      setError(error.message);
      showNotification("Error al realizar la operación.", "error");
    } finally {
      getAllData(currentPage);
      setLoading(false);
    }
  };

  const getOneData = async (data, opcion) => {
    setLoading(true);
    setError(null);
    try {
      const result = await request(`POST`, "Rubro/one", {
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
  }, [currentPage, pageSize]); // Agregar pageSize a la dependencia

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
  };

  return (
    <ContentContext.Provider value={value}>{children}</ContentContext.Provider>
  );
};
