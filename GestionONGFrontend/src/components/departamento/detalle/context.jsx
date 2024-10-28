import React, { createContext, useEffect, useState, useContext } from "react";
import { request } from "../../../service/api";
import { ContentContext as PrincipalContext } from "../context";
export const ContentContext = createContext();

export const ContentProvider = ({ children }) => {
  const { oneData } = useContext(PrincipalContext);
  const [allData, setAllData] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [currentPage, setCurrentPage] = useState(1);
  const [totalRecords, setTotalRecords] = useState(0);
  const [totalPages, setTotalPages] = useState(0);
  const [pageSize, setPageSize] = useState(10);

  const getAllData = async (page) => {
    setLoading(true);
    setError(null);
    try {
      const result = await request("POST", "Departamento/municipios", {
        pageNumber: page,
        pageSize: pageSize,
        id: oneData?.id,
      });
      setAllData(result?.data?.Municipios);
      setTotalRecords(result?.data?.TotalRegistros);
      setTotalPages(result?.data?.TotalPaginas);
    } catch (error) {
      console.error("Error fetching departamentos:", error.message);
      setError(error.message);
    } finally {
      setLoading(false);
    }
  };

  const dropItem = async (data) => {
    setLoading(true);
    setError(null);
    try {
      const result = await request(`DELETE`, `Municipio/delete`, {
        id: data?.id,
      });
    } catch (error) {
      console.error("Error fetching departamentos:", error.message);
      setError(error.message);
    } finally {
      getAllData(currentPage);
      setLoading(false);
    }
  };

  useEffect(() => {
    getAllData(currentPage);
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
  };

  return (
    <ContentContext.Provider value={value}>{children}</ContentContext.Provider>
  );
};
