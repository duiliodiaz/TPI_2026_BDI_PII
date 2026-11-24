import ClienteFormModal from "./ClienteFormModal";

export default function NuevoClienteModal({ clientes, onCerrar, onCrear }) {
  return (
    <ClienteFormModal
      titulo="Nuevo Cliente"
      descripcion="Complete los datos obligatorios para dar de alta"
      textoGuardar="Crear Cliente"
      clientes={clientes}
      onCerrar={onCerrar}
      onGuardar={onCrear}
    />
  );
}