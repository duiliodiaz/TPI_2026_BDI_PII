import styles from "./About.module.css";
import IconUser from "../../assets/IconUser.svg"
const integrantes = [
  { nombre: "Salvador Cahuana"},
  { nombre: "Duilio Dias"},
  { nombre: "Ignacio Muratore" },
  { nombre: " Ismael Nisman"},
];

export default function About () {
    return (
         <section className={styles.about}>
      <h2 className={styles.titulo}>Quiénes somos ?</h2>
      <ul className={styles.lista}>
        {integrantes.map(({ nombre, rol }) => (
          <li key={nombre} className={styles.integrante}>
            <img src={IconUser} alt="" width="60px"/>
            <span className={styles.nombre}>{nombre}</span>
            <span className={styles.rol}>{rol}</span>
          </li>
        ))}
      </ul>
    </section>
    )
}