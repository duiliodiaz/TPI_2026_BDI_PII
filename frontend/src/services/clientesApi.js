import api from "./api";

export const listar = async (signal) => {

  const { data } = await api.get("/Clientes", { signal });//signal para poder cancelar la peticion

  return data.map(c => parserClientes(c))
  
};

export const crear = async (datos) => {
  await api.post("/Clientes", haciaApi({...datos, activo:true}));
};

export const actualizar = async (id, datos) => {
  await api.put(`/Clientes/${id}`, haciaApi(datos));

};

export const darDeBaja = async (id) => {
  await api.delete(`/Clientes/${id}`);
};


const haciaApi = (c) => ({
  nombre: c.nombre.trim(),
  apellido: c.apellido.trim(),
  dni: c.dni,
  eMail: c.email?.trim() ? c.email.trim() : null,
  activo: c.activo ?? true,
});
const formatearDni = (n) => String(n).replace(/\B(?=(\d{3})+(?!\d))/g, ".");
const parserClientes = (c) => ({
  id: c.idCliente,
  nombre: c.nombre,
  apellido: c.apellido,
  dni: formatearDni(c.dni),
  email: c.eMail ?? "",
  activo: c.activo,
});


export const mensajeError = (e, porDefecto) => {
  const d = e.response?.data;
  if (d?.errors) return Object.values(d.errors).flat().join(" ");
  if (e.response?.status === 409) return d?.message ?? d?.title ?? "Ya existe un cliente con esos datos.";
  return d?.message ?? d?.title ?? porDefecto;
};
/* export const mensajeError = (e, porDefecto) =>
  e.response?.data?.message ?? porDefecto; */