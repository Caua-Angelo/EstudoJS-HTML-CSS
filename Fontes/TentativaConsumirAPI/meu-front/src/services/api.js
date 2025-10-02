import axios from "axios";

const baseURL = import.meta.env.VITE_API_URL || "https://localhost:7244";

const api = axios.create({
  baseURL,
  headers: { "Content-Type": "application/json" },
});

// Interceptor para enviar token JWT se existir
api.interceptors.request.use(
  (config) => {
    const token = localStorage.getItem("token");
    if (token && config.headers) config.headers.Authorization = `Bearer ${token}`;
    return config;
  },
  (error) => Promise.reject(error)
);

export default api;