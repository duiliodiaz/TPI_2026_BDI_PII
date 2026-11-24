import axios from "axios";
const BASE = import.meta.env.VITE_API_URL;

const api = axios.create({
  baseURL: BASE,
  headers: { "Content-Type": "application/json" },
  timeout: 10000,
});

// Si hay token (devuelto por POST /api/auth/login), se envía en todas las peticiones
/* api.interceptors.request.use((config) => {
  const token = localStorage.getItem("token");
  if (token) config.headers.Authorization = `Bearer ${token}`;
  return config;
}); */

// Si  pide token, se agrega acá y vale para todas las peticiones
// api.interceptors.request.use((config) => {
//   const token = localStorage.getItem("token");
//   if (token) config.headers.Authorization = `Bearer ${token}`;
//   return config;
// });

export default api;