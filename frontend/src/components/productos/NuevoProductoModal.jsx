import ProductoFormModal from "./ProductoFormModal";

export default function NuevoProductoModal({ tipos, tamanos, onCerrar, onCrear }) {
  return (
    <ProductoFormModal
      titulo="Nuevo Producto"
      descripcion="Complete los datos obligatorios para el catálogo"
      textoGuardar="Crear Producto"
      tipos={tipos}
      tamanos={tamanos}
      mostrarIconoAuto
      onCerrar={onCerrar}
      onGuardar={onCrear}
    />
  );
}