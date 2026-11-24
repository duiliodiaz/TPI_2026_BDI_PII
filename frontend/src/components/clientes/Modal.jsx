import { useEffect, useRef } from "react";
import styles from "./Modal.module.css";

export default function Modal({
  onCerrar,
  labelledBy,
  describedBy,
  peligro = false,
  role,
  children,
}) {
  const ref = useRef(null);

  useEffect(() => {
    const dialog = ref.current;
    if (dialog && !dialog.open) dialog.showModal();
  }, []);

  // Con el <dialog> sin padding, un clic directo sobre él es un clic en el fondo oscuro
  const cerrarSiEsFondo = (e) => {
    if (e.target === ref.current) onCerrar();
  };

  return (
    <dialog
      ref={ref}
      role={role}
      className={`${styles.dialog} ${peligro ? styles.peligro : ""}`}
      aria-labelledby={labelledBy}
      aria-describedby={describedBy}
      onClose={onCerrar}
      onClick={cerrarSiEsFondo}
    >
      {children}
    </dialog>
  );
}