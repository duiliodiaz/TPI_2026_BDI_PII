import Producto from "../producto";
import axios from "axios";
import NuevoProductoModal from "./NuevoProductoModal";
import ModificarProductoModal from "./ModificarProductoModal";
import ConfirmarBajaProductoModal from "./ConfirmarBajaProductoModal";
import styles from "./Productos.module.css"
import IconNewProduct from "../../assets/IconNewProduct.svg"
import * as productosApi from "../../services/productosApi"
import { useMemo, useState, useEffect } from "react";
import { PAGE_SIZE , FILTROS  } from "../../data/productosData";



export default function Productos() {
    const [lista, setLista] = useState([]);
    const [recienHabilitados, setRecienHabilitados] = useState([]);
    const [cargando, setCargando] = useState(true);

    //filtros
    const [tipos, setTipos] = useState([]);
    const [tamanos, setTamanos] = useState([]);
    const [busqueda, setBusqueda] = useState("");
    const [filtro, setFiltro] = useState("todos");//habilitados y deshabilitados
    const [filtroTipo, setFiltroTipo] = useState("");

    //errores y msg
    const [error, setError] = useState(null);
    const [intento, setIntento] = useState(0);
    const [aviso, setAviso] = useState(null);//salio todo bn o mal, para crear, editar y eliminar

    //modal de forms
    const [modal, setModal] = useState(null); // { tipo: "nuevo" | "editar" | "baja", producto }

    //paginado
    const [pagina, setPagina] = useState(1);

    const mostrarExito = (texto) => setAviso({ texto, tipo: "exito", key: Date.now() });
    const mostrarError = (texto) => setAviso({ texto, tipo: "error", key: Date.now() });


    const filtrados = useMemo(() => {
        const q = busqueda.trim().toLowerCase();

        return [...lista]
            .sort((a, b) => b.id - a.id) // más nuevos primero
            .filter((p) => {
                if (filtro === "habilitados" && !p.activo) return false;
                if (filtro === "deshabilitados" && p.activo && !recienHabilitados.includes(p.id)) return false;
                if (filtroTipo && p.idTipo !== Number(filtroTipo)) return false;
                if (!q) return true;
                /* return `${p.descripcion} ${p.tipo}`.toLowerCase().includes(q); */ //por nombre y tipo
                return p.descripcion.toLowerCase().includes(q);
            });
    }, [lista, busqueda, filtro, filtroTipo, recienHabilitados]);

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
    const cambiarFiltroTipo = (e) => {
        setFiltroTipo(e.target.value);
        setPagina(1);
    };

    const cerrarModal = () => setModal(null);

    const crearProducto = async (datos) => {
        try {
            await productosApi.crear(datos);
            setLista(await productosApi.listar());
            setBusqueda("");
            setFiltro("todos");
            setFiltroTipo("");
            setPagina(1);
            mostrarExito("Producto creado correctamente.");
        } catch {
            mostrarError("No se pudo crear el producto. Intentá de nuevo.");
        } finally {
            cerrarModal();
        }
    };

    const actualizarProducto = async (datos) => {
        const estabaInactivo = lista.find((p) => p.id === datos.id)?.activo === false;
        try {
            await productosApi.actualizar(datos.id, datos);
            setLista(await productosApi.listar());
            if (estabaInactivo && datos.activo) {
                setRecienHabilitados((prev) => [...prev, datos.id]);
            }
            mostrarExito(
                estabaInactivo
                    ? "Producto habilitado correctamente."
                    : "Producto actualizado correctamente."
            );
        } catch {
            mostrarError("No se pudo actualizar el producto. Intentá de nuevo.");
        } finally {
            cerrarModal();
        };
    };

    const darDeBaja = async (id) => {
        try {
            await productosApi.darDeBaja(id);
            setLista(await productosApi.listar());
            mostrarExito("Producto dado de baja correctamente.");
        } catch  {
            mostrarError("No se pudo dar de baja el producto.");
        } finally {
            cerrarModal();
        }
    };
    useEffect(() => {
        const controller = new AbortController();
        async function cargar() {
            try {
                setCargando(true);
                setError(null);
                const [prods, tps, tams] = await Promise.all([
                    productosApi.listar(controller.signal),
                    productosApi.listarTipos(controller.signal),
                    productosApi.listarTamanos(controller.signal),
                ]);
                setLista(prods);
                setTipos(tps);
                setTamanos(tams);
            } catch (e) {
                if (!axios.isCancel(e)) setError("No se pudieron cargar los productos.");
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
        <>
            <main className={`${styles.main} flex-grow-1`}>
                <header className={`${styles.topbar} d-flex flex-wrap align-items-center justify-content-between gap-3`}>
                    <div className="d-flex flex-wrap align-items-center gap-3">
                        <h1 className={styles.title}>ABMC de Productos</h1>
                        <span className={styles.tag}>Soporte &amp; Productos</span>
                    </div>

                    <button type="button" className={`${styles.newButton} btn border-0`} disabled={cargando || !!error} onClick={() => setModal({ tipo: "nuevo" })}>
                        <img src={IconNewProduct} alt="" />
                        Nuevo Producto
                    </button>
                </header>

                <div className={styles.content}>
                    <section className={styles.card} aria-label="Listado de Productos">
                        <div className={`${styles.toolbar} d-flex flex-wrap align-items-center justify-content-between gap-3`}>
                            <form
                                role="search"
                                className={`${styles.search} d-flex align-items-center`}
                                onSubmit={(e) => e.preventDefault()}
                            >
                                <label htmlFor="buscar-productos" className="visually-hidden">
                                    Buscar por nombre o categoria
                                </label>
                                {/*  <img src={IconSearch} alt="" /> */}
                                <input
                                    id="buscar-productos"
                                    type="search"
                                    value={busqueda}
                                    onChange={cambiarBusqueda}
                                    placeholder="Buscar por nombre..."
                                    autoComplete="off"
                                />
                            </form>
                            <div className="d-flex flex-wrap align-items-center gap-3">
                                <label htmlFor="filtro-tipo" className="visually-hidden">Filtrar por tipo</label>
                                <select
                                    id="filtro-tipo"
                                    className={styles.tipoSelect}
                                    value={filtroTipo}
                                    onChange={cambiarFiltroTipo}
                                >
                                    <option value="">Todos los tipos</option>
                                    {tipos.map((t) => (
                                        <option key={t.id} value={t.id}>{t.descripcion}</option>
                                    ))}
                                </select>

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
                            </div>
                            <div className="table-responsive">
                                <table className={`table ${styles.table} mb-0 align-middle`}>
                                    <caption className="visually-hidden">Listado de Productos con su estado y acciones</caption>
                                    <thead>
                                        <tr>
                                            <th scope="col">ID / PRODUCTO</th>
                                            <th scope="col">TIPO</th>
                                            <th scope="col">PRECIO UNITARIO</th>
                                            <th scope="col" className="text-center">Estado</th>
                                            <th scope="col" className="text-end">Acciones </th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        {cargando ? (
                                            <tr>
                                                <td colSpan={5} className={styles.empty} role="status">Cargando productos...</td>
                                            </tr>
                                        ) : error ? (
                                            <tr>
                                                <td colSpan={5} className={styles.empty} role="alert">
                                                    {error}{" "}
                                                    <button type="button" className={`${styles.reintentar} btn`} onClick={() => setIntento((n) => n + 1)}>
                                                        Reintentar
                                                    </button>
                                                </td>
                                            </tr>
                                        ) : visibles.length === 0 ? (
                                            <tr>
                                                <td colSpan={5} className={styles.empty}>No se encontraron productos.</td>
                                            </tr>
                                        ) : (
                                            visibles.map((p) => (
                                                <Producto
                                                    key={p.id}
                                                    producto={p}
                                                    onEditar={(producto) => setModal({ tipo: "editar", producto })}
                                                    onEliminar={(producto) => setModal({ tipo: "baja", producto })}
                                                    onHabilitar={actualizarProducto}
                                                />
                                            ))
                                        )}

                                    </tbody>

                                </table>
                            </div>

                            <footer className={`${styles.footer} d-flex flex-wrap align-items-center justify-content-between gap-3`}>
                                <p className={`${styles.summary} mb-0`}>
                                    Mostrando <strong>{desde}–{hasta}</strong> de <strong>{filtrados.length}</strong> productos
                                </p>

                                <nav aria-label="Paginación de productos">
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
                    <NuevoProductoModal tipos={tipos} tamanos={tamanos} onCerrar={cerrarModal} onCrear={crearProducto} />
                )}

                {modal?.tipo === "editar" && (
                    <ModificarProductoModal
                        producto={modal.producto}
                        tipos={tipos}
                        tamanos={tamanos}
                        onCerrar={cerrarModal}
                        onActualizar={actualizarProducto}
                    />
                )}

                {modal?.tipo === "baja" && (
                    <ConfirmarBajaProductoModal producto={modal.producto} onCerrar={cerrarModal} onConfirmar={darDeBaja} />
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
        </>
    );
}