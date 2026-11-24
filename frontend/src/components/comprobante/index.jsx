import styles from "./Comprobante.module.css";
import IconUser from "../../assets/IconUser.svg"
import IconEdit from "../../assets/IconEdit.svg"
import IconDelete from "../../assets/IconDelete.svg"
import IconView from "../../assets/dashboardIcons/view.svg"



const MESES = ["Ene", "Feb", "Mar", "Abr", "May", "Jun", "Jul", "Ago", "Sep", "Oct", "Nov", "Dic"];

const formatearFecha = (iso) => {
  const d = new Date(iso);
  const hh = String(d.getHours()).padStart(2, "0");
  const mm = String(d.getMinutes()).padStart(2, "0");
  return `${d.getDate()} ${MESES[d.getMonth()]} ${d.getFullYear()}, ${hh}:${mm}`;
};

export default function Comprobante({ comprobante, onVer, onEditar, onEliminar, onHabilitar }) {
  const { idComprobante, clienteNombreCompleto, fecha, formaPagoNombre, activo } = comprobante;

    const nro = `#${String(idComprobante).padStart(4, "0")}`;
  const habilitar = () => onHabilitar?.({ ...comprobante, activo: true });

    return (
        <tr className={activo ? "" : styles.rowInactive}>
            <th scope="row" className={styles.numero}>
                {nro}
            </th>

            <td>
                <div className="d-flex align-items-center gap-3">
                    <span
                        className={`${styles.avatar} d-flex align-items-center justify-content-center`}
                        aria-hidden="true"
                    >
                        <img src={IconUser} alt="" />
                    </span>
                    <div>
                        <p className={`${styles.name} mb-0`}>{clienteNombreCompleto}</p>
                    </div>
                </div>
            </td>

            <td className={styles.fecha}>
                <time dateTime={fecha}>{formatearFecha(fecha)}</time>
            </td>

            <td>
                <span className={`${styles.pill}`}>
                    {formaPagoNombre}
                </span>
            </td>

            <td>
                <span className={`${styles.pill} ${activo ? styles.statusOn : styles.statusOff}`}>
                    <span className={styles.dot} aria-hidden="true" />
                    {activo ? "Activo" : "Inactivo"}
                </span>
            </td>

            <td>
                <div className={`${styles.actions} d-flex justify-content-end align-items-center gap-2`}>
                    {activo ? (

                        <>
                            <button
                                type="button"
                                className={`${styles.iconButton} ${styles.view} btn border-0`}
                                aria-label={`Ver comprobante ${idComprobante}`}
                                onClick={() => onVer?.(comprobante)}
                            >
                                <img src={IconView} alt="" />
                            </button>
                            <button
                                type="button"
                                className={`${styles.iconButton} ${styles.edit} btn border-0`}
                                aria-label={`Editar comprobante ${idComprobante}`}
                                onClick={() => onEditar?.(comprobante)}
                            >
                                <img src={IconEdit} alt="" />
                            </button>
                            <button
                                type="button"
                                className={`${styles.iconButton} ${styles.remove} btn border-0`}
                                aria-label={`Eliminar comprobante ${idComprobante}`}
                                onClick={() => onEliminar?.(comprobante)}
                            >
                                <img src={IconDelete} alt="" />
                            </button>
                        </>
                    ) : (
                        <button type="button" className={styles.habilitar} onClick={habilitar}>
                            Habilitar
                        </button>
                    )}
                </div>
            </td>
        </tr>
    );
}