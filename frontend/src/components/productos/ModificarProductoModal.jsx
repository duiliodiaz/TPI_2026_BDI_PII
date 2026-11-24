import ProductoFormModal from "./ProductoFormModal";

export default function ModificarProductoModal({ producto, tipos, tamanos, onCerrar, onActualizar }) {
  return (
    <ProductoFormModal
      titulo="Modificar Producto"
      descripcion="Edite precio, tipo o tamaño"
      textoGuardar="Guardar Cambios"
      tipos={tipos}
      tamanos={tamanos}
      inicial={{
        descripcion: producto.descripcion,
        idTipo: String(producto.idTipo),
        idTamano: String(producto.idTamano),
        precio: String(producto.precio),
        activo: producto.activo,
      }}
      onCerrar={onCerrar}
      onGuardar={(datos) => onActualizar({ ...datos, id: producto.id })}
    />
  );
}