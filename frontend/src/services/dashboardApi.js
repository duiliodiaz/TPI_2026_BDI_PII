import axios from "axios";
const BASE = import.meta.env.VITE_API_URL;

export const apiJtw = axios.create({
  baseURL: BASE,
  headers: { "Content-Type": "application/json" },
  timeout: 10000,
});

// Si hay token (devuelto por POST /api/auth/login), se envía en todas las peticiones
apiJtw.interceptors.request.use((config) => {
  const token = localStorage.getItem("token");
  if (token) config.headers.Authorization = `Bearer ${token}`;
  return config;
});

export const login = async ({user, password}) => {
    const {data} = await apiJtw.post("auth/login",{nombreUsuario:user,password})
    return data
}


export const resumen = async () => {

  const { data } = await apiJtw.get("/dashboard/resumen");

  return data
  
};

export const ingresosSemana = async () => {
  const { data } = await apiJtw.get(`/dashboard/ingresos-semana?fecha=2026-11-14`);
  return data
};

export const fechasMasEntradas = async () => {
  const { data } = await apiJtw.get(`/dashboard/fechas-mas-entradas?top=3`);
    return data
};

export const ProductosMasVendidos = async () => {
  const { data } = await apiJtw.get(`/dashboard/productos-mas-vendidos?top=5`);
  return data
};
