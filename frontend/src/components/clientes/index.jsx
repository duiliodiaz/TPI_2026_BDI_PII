import { useEffect, useMemo, useState } from "react";
import styles from "./Clientes.module.css";
import Cliente from "../cliente";
import IconNewClient from "../../assets/IconNewClient.svg"
import IconSearch from "../../assets/IconSearch.svg"
import NuevoClienteModal from "./NuevoClienteModal";
import ActualizarClienteModal from "./ActualizarClienteModal";
import ConfirmarBajaModal from "./ConfirmarBajaModal";
import axios from "axios";
import * as clientesApi from "../../services/clientesApi";


const PAGE_SIZE = 8;

const FILTROS = [
  { value: "todos", label: "Todos" },
  { value: "habilitados", label: "Habilitados" },
  { value: "deshabilitados", label: "Deshabilitados" },
];


const soloDigitos = (texto) => texto.replace(/\D/g, "");

export default function Clientes() {
    const [lista, setLista] = useState([]);
    const [cargando, setCargando] = useState(true);
    const [recienHabilitados, setRecienHabilitados] = useState([]);
    const [error, setError] = useState(null);
    const [intento, setIntento] = useState(0);
    const [modal, setModal] = useState(null); // { tipo: "nuevo" | "editar" | "baja", cliente }
    const [busqueda, setBusqueda] = useState("");
    const [filtro, setFiltro] = useState("todos");
    const [pagina, setPagina] = useState(1);
    const [aviso, setAviso] = useState(null);

    const mostrarExito = (texto) => setAviso({ texto, tipo: "exito", key: Date.now() });
    const mostrarError = (texto) => setAviso({ texto, tipo: "error", key: Date.now() });

  const filtrados = useMemo(() => {
    const q = busqueda.trim().toLowerCase();
    const qDni = soloDigitos(q);

     return [...lista]
    .sort((a, b) => b.id - a.id) // más nuevos primero
    .filter((c) => {
      if (filtro === "habilitados" && !c.activo) return false;
      if (filtro === "deshabilitados" && c.activo && !recienHabilitados.includes(c.id)) return false;
      if (!q) return true;

      const texto = `${c.nombre} ${c.apellido} ${c.email ?? ""}`.toLowerCase();
      return texto.includes(q) || (qDni && soloDigitos(c.dni).includes(qDni));
    });
  }, [lista, busqueda, filtro, recienHabilitados]);

  const totalPaginas = Math.max(1, Math.ceil(filtrados.length / PAGE_SIZE));
  const actual = Math.min(pagina, totalPaginas);
  const inicio = (actual - 1) * PAGE_SIZE;
  const visibles = filtrados.slice(inicio, inicio + PAGE_SIZE);

  const desde = filtrados.length === 0 ? 0 : inicio + 1;
  const hasta = inicio + visibles.length;

  const cambiarBusqueda = (e) => {
    setBusqueda(e.target.value);
    setPagina(1);
  };

  const cambiarFiltro = (valor) => {
    setFiltro(valor);
    setPagina(1);
    setRecienHabilitados([]);
  };

  const cerrarModal = () => setModal(null);

const crearCliente = async (datos) => {
  try {
    await clientesApi.crear(datos);
    setLista(await clientesApi.listar());
    setBusqueda("");
    setFiltro("todos");
    setPagina(1);
    mostrarExito("Cliente creado correctamente.");
  } catch {
    mostrarError("No se pudo crear el cliente. Intentá de nuevo.");
  }finally {
    cerrarModal();
  }
};

const actualizarCliente = async (datos) => {
    
    const estabaInactivo = lista.find((c) => c.id === datos.id)?.activo === false;
  try {
    await clientesApi.actualizar(datos.id, datos);
    setLista(await clientesApi.listar());
    if (estabaInactivo && datos.activo) {
      setRecienHabilitados((prev) => [...prev, datos.id]);
    }
    mostrarExito(
      estabaInactivo
        ? "Cliente habilitado correctamente."
        : "Cliente actualizado correctamente."
    );
} catch  {
    mostrarError("No se pudo actualizar el cliente. Intentá de nuevo."); 
  }finally {
    cerrarModal();
  }
};

const darDeBaja = async (id) => {
    
    try {
        await clientesApi.darDeBaja(id);
        setLista(await clientesApi.listar());
        mostrarExito("Cliente dado de baja correctamente.");
    } catch {
        mostrarError("No se pudo dar de baja al cliente.");
    } finally {
        cerrarModal();
    }
};


useEffect(() => {
    const controller = new AbortController();//cancela la peticion cuando el componente se desmonta
    async function cargarClientes() {
        try {
            setCargando(true);
            setError(null);
            setLista(await clientesApi.listar(controller.signal));
           
            
        } catch (e) {
            if (!axios.isCancel(e)) setError("No se pudo cargar los clientes.");
        }finally{
            if (!controller.signal.aborted) setCargando(false);
        }
    }
    cargarClientes();
    
    return () => controller.abort();
},[intento])

useEffect(() => {
  if (!aviso) return;
  const t = setTimeout(() => setAviso(null), aviso.tipo === "error" ? 6000 : 4000);
  return () => clearTimeout(t);
}, [aviso]);
    return (
        <main className={`${styles.main} flex-grow-1`}>
            <header className={`${styles.topbar} d-flex flex-wrap align-items-center justify-content-between gap-3`}>
                <div className="d-flex flex-wrap align-items-center gap-3">
                    <h1 className={styles.title}>ABMC de Clientes</h1>
                    <span className={styles.tag}>Soporte &amp; Clientes</span>
                </div>

                <button type="button" className={`${styles.newButton} btn border-0`} onClick={() => setModal({ tipo: "nuevo" })}>
                    <img src={IconNewClient} alt="" />
                    Nuevo Cliente
                </button>
            </header>

            <div className={styles.content}>
                <section className={styles.card} aria-label="Listado de clientes">
                    <div className={`${styles.toolbar} d-flex flex-wrap align-items-center justify-content-between gap-3`}>
                        <form
                            role="search"
                            className={`${styles.search} d-flex align-items-center`}
                            onSubmit={(e) => e.preventDefault()}
                        >
                            <label htmlFor="buscar-clientes" className="visually-hidden">
                                Buscar por nombre, email o DNI
                            </label>
                            <img src={IconSearch} alt="" />
                            <input
                                id="buscar-clientes"
                                type="search"
                                value={busqueda}
                                onChange={cambiarBusqueda}
                                placeholder="Buscar por nombre, email o DNI..."
                                autoComplete="off"
                            />
                        </form>

                        <fieldset className={`${styles.segment} d-flex`}>
                            <legend className="visually-hidden">Filtrar por estado</legend>
                            {FILTROS.map(({ value, label }) => (
                                <div key={value}>
                                    <input
                                        type="radio"
                                        className={`btn-check ${styles.segmentInput}`}
                                        name="filtro-estado"
                                        id={`filtro-${value}`}
                                        checked={filtro === value}
                                        onChange={() => cambiarFiltro(value)}
                                    />
                                    <label htmlFor={`filtro-${value}`} className={styles.segmentLabel}>
                                        {label}
                                    </label>
                                </div>
                            ))}
                        </fieldset>
                    </div>
                    
                    <div className="table-responsive">
                        <table className={`table ${styles.table} mb-0 align-middle`}>
                            <caption className="visually-hidden">Listado de clientes con su estado y acciones</caption>
                            <thead>
                                <tr>
                                    <th scope="col">ID / Cliente</th>
                                    <th scope="col">DNI / Documento</th>
                                    <th scope="col">Contacto</th>
                                    <th scope="col" className="text-center">Estado</th>
                                    <th scope="col" className="text-end">Acciones </th>
                                </tr>
                            </thead>
                            <tbody>
                                {cargando ? (
                                    <tr>
                                        <td colSpan={5} className={styles.empty} role="status">
                                            Cargando clientes...
                                        </td>
                                    </tr>
                                ) : error ? (
                                    <tr>
                                        <td colSpan={5} className={styles.empty} role="alert">
                                            {error}{" "}
                                            <button type="button" className={`${styles.reintentar} btn `} onClick={() => setIntento((n) => n + 1)}>
                                                Reintentar
                                            </button>
                                        </td>
                                    </tr>
                                ) : visibles.length === 0 ? (
                                    <tr>
                                        <td colSpan={5} className={styles.empty}>
                                            No se encontraron clientes con esos filtros.
                                        </td>
                                    </tr>
                                ) : (
                                    visibles.map((c) => (
                                        <Cliente
                                            key={c.id}
                                            cliente={c}
                                            onEditar={(cliente) => setModal({ tipo: "editar", cliente })}
                                            onEliminar={(cliente) => setModal({ tipo: "baja", cliente })}
                                            onHabilitar={actualizarCliente}
                                        />
                                    ))
                                )}
                            </tbody>
                        </table>
                    </div>

                    <footer className={`${styles.footer} d-flex flex-wrap align-items-center justify-content-between gap-3`}>
                        <p className={`${styles.summary} mb-0`}>
                            Mostrando <strong>{desde}–{hasta}</strong> de <strong>{filtrados.length}</strong> usuarios
                        </p>

                        <nav aria-label="Paginación de clientes">
                            <ul className="list-unstyled d-flex align-items-center gap-2 mb-0">
                                <li>
                                    <button
                                        type="button"
                                        className={`${styles.pageButton} btn`}
                                        disabled={actual === 1}
                                        onClick={() => setPagina(actual - 1)}
                                    >
                                        Anterior
                                    </button>
                                </li>

                                {Array.from({ length: totalPaginas }, (_, i) => i + 1).map((n) => (
                                    <li key={n}>
                                        <button
                                            type="button"
                                            className={`${styles.pageButton} ${styles.pageNumber} ${n === actual ? styles.pageActive : ""} btn`}
                                            aria-current={n === actual ? "page" : undefined}
                                            aria-label={`Página ${n}`}
                                            onClick={() => setPagina(n)}
                                        >
                                            {n}
                                        </button>
                                    </li>
                                ))}

                                <li>
                                    <button
                                        type="button"
                                        className={`${styles.pageButton} btn`}
                                        disabled={actual === totalPaginas}
                                        onClick={() => setPagina(actual + 1)}
                                    >
                                        Siguiente
                                    </button>
                                </li>
                            </ul>
                        </nav>
                    </footer>
                </section>
            </div>

            {modal?.tipo === "nuevo" && (
                <NuevoClienteModal clientes={lista} onCerrar={cerrarModal} onCrear={crearCliente} />
            )}

            {modal?.tipo === "editar" && (
                <ActualizarClienteModal
                    cliente={modal.cliente}
                    onCerrar={cerrarModal}
                    onActualizar={actualizarCliente}
                />
            )}

            {modal?.tipo === "baja" && (
                <ConfirmarBajaModal
                    clientes={lista}
                    cliente={modal.cliente}
                    onCerrar={cerrarModal}
                    onConfirmar={darDeBaja}
                />
            )}

            {aviso && (
                <div
                    key={aviso.key}
                    className={`${styles.toast} ${aviso.tipo === "error" ? styles.toastError : ""}`}
                    role={aviso.tipo === "error" ? "alert" : "status"}
                    aria-live={aviso.tipo === "error" ? "assertive" : "polite"}
                >
                    <i
                        className={`bi ${aviso.tipo === "error" ? "bi-x-circle-fill" : "bi-check-circle-fill"} ${styles.toastIcon}`}
                        aria-hidden="true"
                    />
                    <p className="mb-0 flex-grow-1">{aviso.texto}</p>
                    <button
                        type="button"
                        className={`${styles.toastCerrar} btn border-0`}
                        aria-label="Cerrar notificación"
                        onClick={() => setAviso(null)}
                    >
                        <i className="bi bi-x-lg" aria-hidden="true" />
                    </button>
                </div>
            )}
        </main>
    );
}