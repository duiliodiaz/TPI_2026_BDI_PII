import styles from "./Cliente.module.css";
import IconUser from "../../assets/IconUser.svg"
import IconEdit from "../../assets/IconEdit.svg"
import IconDelete from "../../assets/IconDelete.svg"



export default function Cliente({ cliente, onEditar, onEliminar, onHabilitar }) {
  const { id, nombre, apellido, dni, email, activo } = cliente;
  
  
  const habilitar = () => {
    const dnii = parseInt(cliente.dni.replace(/\./g, ""), 10)
    onHabilitar({...cliente,activo: true, dni:dnii})
  }

  return (
    <tr className={activo ? "" : styles.rowInactive}>
      <th scope="row">
        <div className="d-flex align-items-center gap-3">
          <span
            className={`${styles.avatar} d-flex align-items-center justify-content-center`}
            aria-hidden="true"
          >
            <img src={IconUser} alt="" />
          </span>
          <div>
            <p className={`${styles.name} mb-0`}>
              {nombre + ' ' + apellido}
            </p>
            {/* <p className={`${styles.clientId} mb-0`}>
              ID #{String(id).padStart(4, "0")} se debe mostrar id?
            </p> */}
          </div>
        </div>
      </th>

      <td className={styles.dni}>{dni}</td>
      <td className={styles.email}>{email}</td>

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
                aria-label={`Editar a ${nombre}`}
                onClick={() => onEditar?.(cliente)}
              >
                <img src={IconEdit} alt="" />
              </button>
              <button
                type="button"
                className={`${styles.iconButton} ${styles.remove} btn border-0`}
                aria-label={`Eliminar a ${nombre}`}
                onClick={() => onEliminar?.(cliente)}
              >
                <img src={IconDelete} alt="" />
              </button> 
              </>)
            : <button className={styles.habilitar} onClick={habilitar}>
               Habilitar
            </button>
          }
        </div>
      </td>
    </tr>
  );
}