import { useState } from "react";
import "bootstrap/dist/css/bootstrap.min.css";
import logo from './../../assets/cineplanet-logo.png'
import { Link } from "react-router";

const links = [
  { label: "Inicio", href: "#" },
  { label: "Películas", href: "#" },
  { label: "Series", href: "#" },
  { label: "Estrenos", href: "#" },
  { label: "Acerca de nosotros", href: "#" },
];

const styles = `
  .cp-navbar { background-color: #0d0d14; height: 62px; }
  .cp-brand { font-weight: 700; font-size: 1.6rem; letter-spacing: -0.5px; color: #fff; }
  .cp-brand span { color: #8b6cf6; }
  .cp-link { color: #d4d4dc !important; font-size: 0.82rem; padding: 0 0.9rem !important; }
  .cp-link:hover { color: #fff !important; }
  .cp-link.active { color: #fff !important; font-weight: 700; }
  .cp-search { background: none; border: 0; color: #d4d4dc; }
  .cp-search:hover { color: #fff; }
  .cp-avatar {
    width: 32px; height: 32px; border-radius: 50%;
    background-color: #f59e0b; color: #1a1a1a;
    font-size: 0.68rem; font-weight: 700;
    display: flex; align-items: center; justify-content: center;
    border: 0;
  }
  .cp-toggler { border-color: rgba(255,255,255,.3); }
  .cp-toggler .navbar-toggler-icon { filter: invert(1); }
`;

function Logo() {
  return (
    <svg width="40" height="32" viewBox="0 0 40 32" fill="none" aria-hidden="true">
      <ellipse cx="20" cy="17" rx="19" ry="6" transform="rotate(-18 20 17)" stroke="#8b6cf6" strokeWidth="2.5" />
      <circle cx="20" cy="16" r="9" fill="#8b6cf6" />
      <circle cx="17" cy="13" r="1.6" fill="#0d0d14" />
      <circle cx="23" cy="13" r="1.6" fill="#0d0d14" />
      <circle cx="17" cy="19" r="1.6" fill="#0d0d14" />
      <circle cx="23" cy="19" r="1.6" fill="#0d0d14" />
      <circle cx="20" cy="16" r="1.4" fill="#0d0d14" />
    </svg>
  );
}

function SearchIcon() {
  return (
    <svg width="16" height="16" viewBox="0 0 16 16" fill="currentColor" aria-hidden="true">
      <path d="M11.742 10.344a6.5 6.5 0 1 0-1.397 1.398h-.001l3.85 3.85a1 1 0 0 0 1.415-1.414l-3.85-3.85zM12 6.5a5.5 5.5 0 1 1-11 0 5.5 5.5 0 0 1 11 0z" />
    </svg>
  );
}

export default function Navbar({ activeItem = "Películas", userInitials = "TU" }) {
  const [active, setActive] = useState(activeItem);

  return (
    <>
      <style>{styles}</style>
      <nav className="navbar navbar-expand-lg cp-navbar" aria-label="Principal" data-bs-theme="dark">
        <div className="container-fluid px-4">
          <a className="navbar-brand d-flex align-items-center gap-2 me-4 cp-brand" href="#" aria-label="Cineplanet, inicio">
            <img src={logo} alt="" width='200px'/>
          </a>

          <button
            className="navbar-toggler cp-toggler"
            type="button"
            data-bs-toggle="collapse"
            data-bs-target="#cpNav"
            aria-controls="cpNav"
            aria-expanded="false"
            aria-label="Abrir menú"
          >
            <span className="navbar-toggler-icon" />
          </button>

          <div className="collapse navbar-collapse" id="cpNav">
            <ul className="navbar-nav me-auto">
              {links.map(({ label, href }) => (
                  label === "Acerca de nosotros" ?

                  <Link 
                    to="/about" 
                    className={`nav-link cp-link ${active === label ? "active" : ""}`} 
                    key={label}>
                    {label}
                  </Link>
                  : <li className="nav-item" key={label}>
                    <a
                      className={`nav-link cp-link ${active === label ? "active" : ""}`}
                      href={href}
                      aria-current={active === label ? "page" : undefined}
                      onClick={() => setActive(label)}
                    >
                      {label}
                    </a>
                  </li>
              ))}
            </ul>

            <div className="d-flex align-items-center gap-3">
              <button className="cp-search" aria-label="Buscar">
                <SearchIcon />
              </button>
              <button className="cp-avatar" aria-label="Perfil">
                {userInitials}
              </button>
            </div>
          </div>
        </div>
      </nav>
    </>
  );
}
