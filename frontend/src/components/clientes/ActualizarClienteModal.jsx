import ClienteFormModal from "./ClienteFormModal";

export default function ActualizarClienteModal({ clientes, cliente, onCerrar, onActualizar }) {
  return (
    <ClienteFormModal
      titulo="Actualizar Cliente"
      descripcion="Edite los datos y guarde los cambios"
      textoGuardar="Guardar Cambios"
      data={cliente}
      clientes={clientes}
      onCerrar={onCerrar}
      onGuardar={onActualizar}
    />
  );
}