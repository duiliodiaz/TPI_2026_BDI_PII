import { useState, useMemo, useEffect } from "react";
import IconSearch from "../../assets/IconSearch.svg"
import stylesCli from "../clientes/Clientes.module.css"
import stylesPro from "../productos/Productos.module.css"
import Comprobante from "../comprobante";
import * as comprobantesApi from "../../services/comprobantesApi";
import axios from "axios";
import {PAGE_SIZE, FILTROS } from "../../data/productosData"
import styles from "./Comprobantes.module.css";
import BajaComprobante from "./BajaComprobante";
import FormComprobante from "./FormComprobante";



export default function Comprobantes () {
    const [lista, setLista] = useState([]);
    const [recienHabilitados, setRecienHabilitados] = useState([]);
    const [cargando, setCargando] = useState(false);
    
    const [formasPago, setFormasPago] = useState([]);      // lista de la API (array)
    const [formaPago, setFormaPago] = useState("");
    const [filtro, setFiltro] = useState("todos");//habiltados y deshabilitados
    const [busqueda, setBusqueda] = useState("");


    const [error, setError] = useState(null);
    const [intento, setIntento] = useState(0);
    const [aviso, setAviso] = useState(null);
    
    const [modal, setModal] = useState(null); // { tipo: "nuevo" | "editar" | "baja", comprobante }
    
    //paginado
    const [pagina, setPagina] = useState(1);
    const mostrarExito = (texto) => setAviso({ texto, tipo: "exito", key: Date.now() });
    const mostrarError = (texto) => setAviso({ texto, tipo: "error", key: Date.now() });


    const filtrados = useMemo(() => {
        const q = busqueda.trim().toLowerCase().replace("#", "");

        return [...lista]
            .sort((a, b) => b.idComprobante - a.idComprobante)
            .filter((c) => {
                if (filtro === "habilitados" && !c.activo) return false;
                if (filtro === "deshabilitados" && c.activo && !recienHabilitados.includes(c.idComprobante)) return false;
                if (formaPago && String(c.idFormaPago) !== formaPago) return false;
                if (!q) return true;

                return (
                    String(c.idComprobante).includes(q) ||
                    c.clienteNombreCompleto.toLowerCase().includes(q)
                );
            });
    }, [lista, busqueda, filtro, formaPago, recienHabilitados]);


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

  const cambiarFormaPago = (e) => {
    setFormaPago(e.target.value);
    setPagina(1);
  };

  const cerrarModal = () => setModal(null);

    const crearComprobante = async (datos) => {
        try {
            await comprobantesApi.crear(datos);
            setLista(await comprobantesApi.listar());
            setBusqueda("");
            setFiltro("");
            setFormaPago("todas");
            setPagina(1);
            mostrarExito("Comprobante creado correctamente.");
        } catch {
            mostrarError("No se pudo crear el comprobante. Intentá de nuevo.");
        } finally {
            cerrarModal();
        }
    };
    const actualizarComprobante = async (datos) => {
        const estabaInactivo = lista.find((c) => c.idComprobante === datos.idComprobante)?.activo === false;
        try {
            await comprobantesApi.actualizar(datos.idComprobante, datos);
            setLista(await comprobantesApi.listar());
            if (estabaInactivo && datos.activo) {
                setRecienHabilitados((prev) => [...prev, datos.idComprobante]);
            }
            mostrarExito(estabaInactivo ? "Comprobante habilitado correctamente." : "Comprobante actualizado correctamente.");
        } catch {
            mostrarError("No se pudo actualizar el comprobante. Intentá de nuevo.");
        } finally {
            cerrarModal();
        }
    };

    const darDeBaja = async (id) => {
        console.log(id);
        
        try {
            await comprobantesApi.darDeBaja(id);
            setLista(await comprobantesApi.listar());
            mostrarExito("Comprobante dado de baja correctamente.");
        } catch {
            mostrarError("No se pudo dar de baja el comprobante.");
        } finally {
            cerrarModal();
        }
    };

    useEffect(() => {
        const controller = new AbortController(); // cancela la petición si el componente se desmonta
        async function cargar() {
            try {
                setCargando(true);
                setError(null);
                const [compr, fpagos] = await Promise.all([
                    comprobantesApi.listar(controller.signal),
                    comprobantesApi.ObtenerformasPago(controller.signal),
                ]);
                setLista(compr);
                setFormasPago(fpagos)
            } catch (e) {
                if (!axios.isCancel(e)) setError("No se pudieron cargar los comprobantes.");
            } finally {
                if (!controller.signal.aborted) setCargando(false);
            }
        }
        cargar();

        return () => controller.abort();
    }, [intento]);

    useEffect(() => {
        if (!aviso) return;
        const t = setTimeout(() => setAviso(null), aviso.tipo === "error" ? 6000 : 4000);
        return () => clearTimeout(t);
    }, [aviso]);



    return (

        <main className={`${stylesPro.main} flex-grow-1`}>
            <header className={`${stylesCli.topbar} d-flex flex-wrap align-items-center justify-content-between gap-3`}>
                <div className="d-flex flex-wrap align-items-center gap-3">
                    <h1 className={stylesCli.title}>Gestión de comprobantes</h1>
                    <span className={stylesCli.tag}>Transacciones &amp; Ventas</span>
                </div>
                <button type="button" className={`${stylesCli.newButton} btn border-0`} onClick={() => setModal({ tipo: "nuevo" })}>
                    + Nuevo Comprobante
                </button>
            </header>

            <div className={stylesPro.content}>
                <section className={stylesPro.card} aria-label="Listado de comprobantes">
                    <div className={`${stylesCli.toolbar} d-flex flex-wrap align-items-center justify-content-between gap-3`}>
                        <form role="search" className={`${stylesPro.search} d-flex align-items-center`} onSubmit={(e) => e.preventDefault()}>
                            <label htmlFor="buscar-comprobantes" className="visually-hidden">
                                Buscar por N° comprobante o cliente
                            </label>
                            <img src={IconSearch} alt="" />
                            <input
                                id="buscar-comprobantes"
                                type="search"
                                value={busqueda}
                                onChange={cambiarBusqueda}
                                placeholder="Buscar por N° comprobante o cliente..."
                                autoComplete="off"
                            />
                        </form>

                        <div className="d-flex align-items-center gap-3">
                            <label htmlFor="forma-pago" className="visually-hidden">
                                forma de pago
                            </label>
                            <select id="forma-pago" className={stylesPro.tipoSelect} value={formaPago} onChange={cambiarFormaPago}>
                                    <option  value="">Formas de pago</option>

                                {formasPago.map((f) => (
                                    <option key={f.idFormaPago} value={f.idFormaPago}>{f.descripcion}</option>
                                ))}
                            </select>

                            <fieldset className={`${stylesPro.segment} d-flex`}>
                                <legend className="visually-hidden">Filtrar por estado</legend>
                                {FILTROS.map(({ value, label }) => (
                                    <div key={value}>
                                        <input
                                            type="radio"
                                            className={`btn-check ${stylesPro.segmentInput}`}
                                            name="filtro-estado"
                                            id={`filtro-${value}`}
                                            checked={filtro === value}
                                            onChange={() => cambiarFiltro(value)}
                                        />
                                        <label htmlFor={`filtro-${value}`} className={stylesPro.segmentLabel}>
                                            {label}
                                        </label>
                                    </div>
                                ))}
                            </fieldset>
                        </div>
                    </div>

                    <div className="table-responsive">
                        <table className={`table ${stylesCli.table} mb-0 align-middle`}>
                            <caption className="visually-hidden">Listado de comprobantes con su estado y acciones</caption>
                            <thead>
                                <tr>
                                    <th scope="col">N°</th>
                                    <th scope="col">Cliente</th>
                                    <th scope="col">Fecha y hora</th>
                                    <th scope="col">Forma de pago</th>
                                    <th scope="col" className="text-center">Estado</th>
                                    <th scope="col" className="text-end">Acciones</th>
                                </tr>
                            </thead>
                            <tbody>
                                {cargando ? (
                                    <tr>
                                        <td colSpan={6} className={styles.empty} role="status">
                                            Cargando comprobantes...
                                        </td>
                                    </tr>
                                ) : error ? (
                                    <tr>
                                        <td colSpan={6} className={styles.empty} role="alert">
                                            {error}{" "}
                                            <button type="button" className={`${styles.reintentar} btn`} onClick={() => setIntento((n) => n + 1)}>
                                                Reintentar
                                            </button>
                                        </td>
                                    </tr>
                                ) : visibles.length === 0 ? (
                                    <tr>
                                        <td colSpan={6} className={styles.empty}>
                                            No se encontraron comprobantes con esos filtros.
                                        </td>
                                    </tr>
                                ) : (
                                    visibles.map((c) => (
                                        <Comprobante
                                            key={c.idComprobante}
                                            comprobante={c}
                                            onVer={(comprobante) => setModal({ tipo: "ver", comprobante })}
                                            onEditar={(comprobante) => setModal({ tipo: "editar", comprobante })}
                                            onEliminar={(comprobante) => setModal({ tipo: "baja", comprobante })}
                                            onHabilitar={actualizarComprobante}
                                        />
                                    ))
                                )}

                            </tbody>
                        </table>
                    </div>

                    <footer className={`${stylesCli.footer} d-flex flex-wrap align-items-center justify-content-between gap-3`}>
                        <p className={`${stylesCli.summary} mb-0`}>
                            Mostrando <strong>{desde}–{hasta}</strong> de <strong>{filtrados.length}</strong> Comprobantes
                        </p>
                        <nav aria-label="Paginación de comprobantes">
                            <ul className="list-unstyled d-flex align-items-center gap-2 mb-0">
                                <li >
                                    <button type="button" className={`${stylesCli.pageButton} btn`} disabled={actual === 1} onClick={() => setPagina(actual - 1)}>
                                        Anterior
                                    </button>
                                </li>
                                {Array.from({ length: totalPaginas }, (_, i) => i + 1).map((n) => (
                                    <li key={n} >
                                        <button
                                            type="button"
                                            className={`${stylesCli.pageButton} ${stylesCli.pageNumber} ${n === actual ? stylesCli.pageActive : ""} btn`}
                                            aria-current={n === actual ? "page" : undefined}
                                            aria-label={`Página ${n}`}
                                            onClick={() => setPagina(n)}
                                        >
                                            {n}
                                        </button>
                                    </li>
                                ))}
                                <li >
                                    <button type="button" className={`${stylesCli.pageButton} btn`} disabled={actual === totalPaginas} onClick={() => setPagina(actual + 1)}>
                                        Siguiente
                                    </button>
                                </li>
                            </ul>
                        </nav>
                    </footer>
                </section>
            </div>
            {modal?.tipo === "ver" && (
            <FormComprobante comprobante={modal.comprobante} soloLectura onCerrar={cerrarModal} />
            )}

            {modal?.tipo === "nuevo" && <FormComprobante  onCerrar={cerrarModal} onCrear={crearComprobante} />}

            {modal?.tipo === "editar" && (
                <FormComprobante comprobante={modal.comprobante} onCerrar={cerrarModal} onActualizar={actualizarComprobante} />
            )}
            
            {modal?.tipo === "baja" && (
                <BajaComprobante comprobante={modal.comprobante} onCerrar={cerrarModal} onConfirmar={darDeBaja} />
            )}
    
            {aviso && (
                <div
                    key={aviso.key}
                    className={`${styles.toast} ${aviso.tipo === "error" ? styles.toastError : ""}`}
                    role={aviso.tipo === "error" ? "alert" : "status"}
                    aria-live={aviso.tipo === "error" ? "assertive" : "polite"}
                >
                    <i className={`bi ${aviso.tipo === "error" ? "bi-x-circle-fill" : "bi-check-circle-fill"} ${styles.toastIcon}`} aria-hidden="true" />
                    <p className="mb-0 flex-grow-1">{aviso.texto}</p>
                    <button type="button" className={`${styles.toastCerrar} btn border-0`} aria-label="Cerrar notificación" onClick={() => setAviso(null)}>
                        <i className="bi bi-x-lg" aria-hidden="true" />
                    </button>
                </div>
            )}
        </main>

    )
}