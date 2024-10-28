const API_URL = "http://localhost:3000/api";

export const request = async (method, endpoint, body = null) => {
  const options = {
    method: method,
    headers: {
      "Content-Type": "application/json",
    },
  };

  if (body) {
    options.body = JSON.stringify(body);
  }

  const response = await fetch(`${API_URL}/${endpoint}`, options);

  if (!response.ok) {
    const errorResponse = await response.json();
    throw new Error(errorResponse.message || "Error en la solicitud");
  }

  return await response.json();
};
