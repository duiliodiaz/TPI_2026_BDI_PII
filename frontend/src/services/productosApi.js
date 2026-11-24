import api from "./api";

const parserProducto = (p) => ({
  id: p.idProducto,
  descripcion: p.descripcion,
  precio: p.preUnitario,
  idTipo: p.idTipoProducto,
  idTamano: p.idTamañoProducto,
  tipo: p.tipoProductoNombre,
  tamano: p.tamañoProductoNombre,
  activo: p.activo,
});

const haciaApi = (p) => ({
  descripcion: p.descripcion.trim(),
  preUnitario: Number(p.precio),
  idTipoProducto: Number(p.idTipo),
  idTamañoProducto: Number(p.idTamano),
  activo: p.activo ?? true,
});

const parserCatalogo = (idCampo) => (x) => ({ id: x[idCampo], descripcion: x.descripcion });

export const listar = async (signal) => {
  const { data } = await api.get("/Productos", { signal });
  return data.map(parserProducto);
};

export const listarTipos = async (signal) => {
  const { data } = await api.get("/Productos/tipos", { signal });
  return data.map(parserCatalogo("idTipoProducto"));
};

export const listarTamanos = async (signal) => {
  const { data } = await api.get(`/Productos/${encodeURIComponent("tamaños")}`, { signal });
  return data.map(parserCatalogo("idTamañoProducto"));
};

export const crear = async (datos) => {
  await api.post("/Productos", haciaApi({ ...datos, activo: true }));
};

export const actualizar = async (id, datos) => {
  await api.put(`/Productos/${id}`, haciaApi(datos));
};

// Baja lógica: el backend pone activo = false
export const darDeBaja = async (id) => {
  await api.delete(`/Productos/${id}`);
};

export const mensajeError = (e, porDefecto) => {
  const d = e.response?.data;
  if (d?.errors) return Object.values(d.errors).flat().join(" ");
  return d?.message ?? d?.title ?? porDefecto;
};