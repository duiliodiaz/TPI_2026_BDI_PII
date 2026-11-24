import { useState } from "react";
import { NavLink, useNavigate } from "react-router";
import styles from "./SideBar.module.css";
import logo from "../../assets/cineplanet-logo.png"
import IconSoporte from "../../assets/IconSuport.svg"
import IconTransaccion from "../../assets/IconTransaction.svg"
import IconProductos from "../../assets/IconProductos.svg"
import IconDashboard from "../../assets/IconDashboard.svg"
import IconComprobante from "../../assets/IconComprobante.svg"
import IconClientes from "../../assets/IconClientes.svg"
import IconDown from "../../assets/IconDown.svg"
import IconSalir from "../../assets/IconSalir.svg"

const menu = [
  {
    label: "Dashboard General",
    to: "/admin",
    icon: IconDashboard,
    tone: "muted",
    end: true,
  },
  {
    id: "soporte",
    label: "Soporte",
    icon: IconSoporte,
    tone: "violet",
    children: [
      { label: "Clientes", to: "clientes", icon: IconClientes, tone: "amber" },
      { label: "Productos", to: "productos", icon: IconProductos, tone: "violet" },
    ],
  },
  {
    id: "transaccion",
    label: "Transacción",
    icon: IconTransaccion,
    tone: "green",
    children: [
      { label: "Comprobante", to: "/admin/comprobante", icon:IconComprobante, tone: "green" },
    ],
  },
];

const linkClass =
  (extra = "") =>
  ({ isActive }) =>
    [styles.item, extra, isActive ? styles.active : "", "d-flex align-items-center"]
      .filter(Boolean)
      .join(" ");


export default function Sidebar({
  user = { initials: "AD", name: "Admin Principal", email: "admin@cineplanet.co" }
}) {
  const navigate = useNavigate();
  const [open, setOpen] = useState({ soporte: false, transaccion: false });

  const toggle = (id) => setOpen((prev) => ({ ...prev, [id]: !prev[id] }));

  const onLogout = () => {
    navigate("/");
  }
  return (
    <aside className={`${styles.sidebar} d-flex flex-column`} aria-label="Barra lateral de administración">
      <header className={`${styles.header} d-flex align-items-center gap-3`}>
        <img src={logo} alt="" width='140px'/>
        <span className={`${styles.badge} ms-auto`}>ADMIN</span>
      </header>

      <nav className={`${styles.nav} flex-grow-1`} aria-label="Panel de control">
        <h2 className={styles.section}>Panel de control</h2>
        <ul className="list-unstyled mb-0">
          {menu.map((item) =>
            item.children ? (
              <li key={item.id}>
                <button
                  type="button"
                  className={`${styles.item} ${styles.itemGroup} btn border-0 w-100 d-flex align-items-center`}
                  aria-expanded={open[item.id]}
                  aria-controls={`submenu-${item.id}`}
                  onClick={() => toggle(item.id)}
                >
                  <img src={item.icon} alt="" />
                  <span>{item.label}</span>
                  <img
                    src={IconDown}
                    alt=""
                    className={`${styles.chevron} ${open[item.id] ? styles.chevronOpen : ""} ms-auto`}
                    aria-hidden="true"
                  />
                </button>

                <ul
                  id={`submenu-${item.id}`}
                  className={`collapse list-unstyled mb-0 ${open[item.id] ? "show" : ""}`}
                >
                  {item.children.map((child) => (
                    <li key={child.to}>
                      <NavLink to={child.to} className={linkClass(styles.itemChild)}>
                        <img src={child.icon} alt="" />
                        <span>{child.label}</span>
                      </NavLink>
                    </li>
                  ))}
                </ul>
              </li>
            ) : (
              <li key={item.to}>
                <NavLink to={item.to} end={item.end} className={linkClass()}>
                  <img src={item.icon} alt="" />
                  <span>{item.label}</span>
                </NavLink>
              </li>
            )
          )}
          <li >
                <NavLink to="about"  className={linkClass()}>
                 
                  <span>Acerca de</span>
                </NavLink>
          </li>
        </ul>
      </nav>

      <footer className={styles.footer}>
        <div className={`${styles.user} d-flex align-items-center gap-3`}>
          <span className={`${styles.avatar} d-flex align-items-center justify-content-center`} aria-hidden="true">
            {user.initials}
          </span>
          <div className={styles.userInfo}>
            <p className={`${styles.userName} mb-0`}>{user.name}</p>
            <p className={`${styles.userEmail} mb-0`}>{user.email}</p>
          </div>
          <button
            type="button"
            className={`${styles.logout} btn border-0 ms-auto`}
            aria-label="Cerrar sesión"
            onClick={onLogout}
          >
            <img src={IconSalir} alt=""  aria-hidden="true" />
          </button>
        </div>
      </footer>
    </aside>
  );
}