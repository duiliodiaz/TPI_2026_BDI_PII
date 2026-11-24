import styles from "./Producto.module.css";
import IconProduct from "../../assets/IconProduct.svg"
import IconEdit from "../../assets/IconEdit.svg"
import IconDelete from "../../assets/IconDelete.svg"

const formatearPrecio = (n) =>
  "$" + Number(n).toLocaleString("es-AR", { minimumFractionDigits: 0, maximumFractionDigits: 2 });


export default function Producto({ producto, onEditar, onEliminar, onHabilitar }) {
  const { id, descripcion, tipo, tamano, precio, activo } = producto;

  const habilitar = () => {
    onHabilitar?.({ ...producto, activo: true })
  }
  return (
    <tr className={activo ? "" : styles.rowInactive}>
      <th scope="row">
        <div className="d-flex align-items-center gap-3">
          <span className={`${styles.avatar} d-flex align-items-center justify-content-center`} aria-hidden="true">
            <img src={IconProduct} alt="" />
          </span>
          <div>
            <p className={`${styles.name} mb-0`}>{descripcion}</p>
            <p className={`${styles.clientId} mb-0`}>
              {/* ID #{String(id).padStart(4, "0")} ·  */}{tamano}
            </p>
          </div>
        </div>
      </th>

      <td className={styles.dni}>
        <span className={tipo === "Bebidas" ? styles.bebida : styles.combos}>{tipo}</span>
      </td>
      <td className={styles.email}>{formatearPrecio(precio)}</td>
      <td className="text-center">
        <span className={`${styles.status} ${activo ? styles.statusOn : styles.statusOff}`}>
          {activo ? "Activo" : "Inactivo"}
        </span>
      </td>

      <td>
        <div className={`${styles.actions} d-flex justify-content-end gap-2`}>
          {activo ?
          
                      (<> 
                        <button
                          type="button"
                          className={`${styles.iconButton} ${styles.edit} btn border-0`}
                          aria-label={`Editar a ${descripcion}`}
                          onClick={() => onEditar?.(producto)}
                        >
                          <img src={IconEdit} alt="" />
                        </button>
                        <button
                          type="button"
                          className={`${styles.iconButton} ${styles.remove} btn border-0`}
                          aria-label={`Eliminar a ${descripcion}`}
                          onClick={() => onEliminar?.(producto)}
                        >
                          <img src={IconDelete} alt="" />
                        </button> 
                        </>)
                      : <button type="button" className={styles.habilitar} aria-label={`Habilitar ${descripcion}`} onClick={habilitar}>
                         Habilitar
                      </button>
                    }
        </div>
      </td>
    </tr>
  );
}