export const CATEGORIAS = [
  { value: "Combos Popcorn", prefijo: "POP", tono: "amber" },
  { value: "Bebidas", prefijo: "BEB", tono: "sky" },
  { value: "Snacks & Dulces", prefijo: "SNK", tono: "orange" },
  { value: "Merchandising", prefijo: "MRCH", tono: "violet" },
];

export const TAMANOS = ["Chico", "Mediano", "Grande", "Mega / Balde", "Único"];

export const getCategoria = (nombre) =>
  CATEGORIAS.find((c) => c.value === nombre) ?? { value: nombre, prefijo: "GEN", tono: "amber" };

export const generarSku = (categoria, id) =>
  `CP-${getCategoria(categoria).prefijo}-${String(id).padStart(3, "0")}`;

// "$6.500" (separador de miles manual, para no depender del locale)
export const formatearPrecio = (n) =>
  "$" + String(Math.round(n)).replace(/\B(?=(\d{3})+(?!\d))/g, ".");

export const PRODUCTOS = [
  { id: 1,  nombre: "Mega Balde Popcorn Dulce", categoria: "Combos Popcorn", tamano: "Mega / Balde", precio: 6500, activo: true, icono: "amber" },
  { id: 2,  nombre: "Combo Dúo Mega Popcorn + 2 Gaseosas", categoria: "Combos Popcorn", tamano: "Mega / Balde", precio: 11200, activo: true, icono: "red" },
  { id: 3,  nombre: "Gaseosa Grande 32oz (Coca Cola)", categoria: "Bebidas", tamano: "Grande", precio: 3800, activo: true, icono: "amber" },
  { id: 4,  nombre: "Nachos con Queso Cheddar Caliente", categoria: "Snacks & Dulces", tamano: "Grande", precio: 5200, activo: true, icono: "amber" },
  { id: 5,  nombre: "Chocolates M&M's Peanut Crispy", categoria: "Snacks & Dulces", tamano: "Único", precio: 2900, activo: false, icono: "amber" },
  { id: 6,  nombre: "Vaso Coleccionable Tron: Ares con Luz", categoria: "Merchandising", tamano: "Único", precio: 8900, activo: true, icono: "amber" },
  { id: 7,  nombre: "Balde Popcorn Salado Clásico", categoria: "Combos Popcorn", tamano: "Grande", precio: 5800, activo: true, icono: "amber" },
  { id: 8,  nombre: "Café Gourmet Espresso Doble", categoria: "Bebidas", tamano: "Mediano", precio: 2500, activo: true, icono: "amber" },
];

export const PAGE_SIZE = 8;

export const FILTROS = [
  { value: "todos", label: "Todos" },
  { value: "habilitados", label: "Habilitados" },
  { value: "deshabilitados", label: "Deshabilitados" },
];