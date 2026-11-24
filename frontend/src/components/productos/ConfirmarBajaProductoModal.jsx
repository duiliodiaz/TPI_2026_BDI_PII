import { useId } from "react";
import Modal from "../clientes/Modal";
import base from "../clientes/ConfirmarBajaModal.module.css"; // content, icon, title, cancel, confirm
import styles from "./ConfirmarBajaProductoModal.module.css";
import IconDelete from "../../assets/IconDelete1.svg"

export default function ConfirmarBajaProductoModal({ producto, onCerrar, onConfirmar }) {
  const uid = useId();
  const tituloId = `${uid}-titulo`;
  const descId = `${uid}-desc`;

  return (
    <Modal peligro role="alertdialog" onCerrar={onCerrar} labelledBy={tituloId} describedBy={descId}>
      <div className={`${base.content} d-flex flex-column align-items-center justify-content-center text-center`}>
        <span className={`${base.icon} d-flex align-items-center justify-content-center`} aria-hidden="true">
          <img src={IconDelete} alt="" />
        </span>

        <h2 id={tituloId} className={base.title}>¿Confirmar Baja del Producto?</h2>

        <p id={descId} className={styles.text}>
          El producto <strong>&quot;{producto.descripcion}&quot;</strong> será retirado del
          catálogo activo y ya no estará disponible para la venta.
        </p>

        <dl className={`${styles.categoria} d-flex align-items-center justify-content-between`}>
          <dt>Tipo:</dt>
          <dd>{producto.tipo}</dd>
        </dl>

        <div className="d-flex flex-wrap justify-content-center gap-3">
          <button type="button" className={`${base.cancel} btn`} onClick={onCerrar}>
            Cancelar Operación
          </button>
          <button type="button" className={`${base.confirm} btn`} onClick={() => onConfirmar(producto.id)}>
            Confirmar Baja
          </button>
        </div>
      </div>
    </Modal>
  );
}