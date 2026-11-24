import { useId } from "react";
import IconDelete from "../../assets/IconDelete1.svg"
import Modal from "./Modal";
import styles from "./ConfirmarBajaModal.module.css";

export default function ConfirmarBajaModal({ cliente, onCerrar, onConfirmar }) {
  const uid = useId();
  const tituloId = `${uid}-titulo`;
  const descId = `${uid}-desc`;
  console.log(cliente.id);
  
  return (
    <Modal
      peligro
      role="alertdialog"
      onCerrar={onCerrar}
      labelledBy={tituloId}
      describedBy={descId}
    >
      <div className={`${styles.content} d-flex flex-column align-items-center justify-content-center text-center`}>
        <span className={`${styles.icon} d-flex align-items-center justify-content-center`} aria-hidden="true">
          <img src={IconDelete} alt="" />
        </span>

        <h2 id={tituloId} className={styles.title}>¿Confirmar Baja del Cliente?</h2>

        <p id={descId} className={styles.text}>
          El cliente <strong>&quot;{cliente.nombre}&quot; (DNI {cliente.dni})</strong> será
          dado de baja del sistema y perderá su condición activa.
        </p>

        <div className="d-flex flex-wrap justify-content-center gap-3">
          <button type="button" className={`${styles.cancel} btn`} onClick={onCerrar}>
            Cancelar Operación
          </button>
          <button type="button" className={`${styles.confirm} btn`} onClick={() => onConfirmar(cliente.id)}>
            Confirmar Baja
          </button>
        </div>
      </div>
    </Modal>
  );
}