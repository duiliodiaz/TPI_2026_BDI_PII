import { useEffect, useMemo, useState } from "react";
import axios from "axios";
import styles from "./FormComprobante.module.css";
import ClienteBuscador from "./ClienteBuscador";
import * as clientesApi from "../../services/clientesApi"
import * as productosApi from "../../services/productosApi"
import * as comprobantesApi from "../../services/comprobantesApi"


const moneda = new Intl.NumberFormat("es-AR", { style: "currency", currency: "ARS" });

// ISO -> valor para <input type="datetime-local"> (hora local)
const aInputFecha = (iso) => {
  const d = new Date(iso);
  if (Number.isNaN(d.getTime())) return "";
  return new Date(d.getTime() - d.getTimezoneOffset() * 60000).toISOString().slice(0, 16);
};

const Icono = ({ children, size = 16 }) => (
  <svg width={size} height={size} viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round" aria-hidden="true">
    {children}
  </svg>
);
const IconClose = () => <Icono><path d="M18 6 6 18M6 6l12 12" /></Icono>;
const IconReceipt = () => <Icono><path d="M4 2v20l3-2 3 2 3-2 3 2 3-2V2l-3 2-3-2-3 2-3-2-3 2z" /><path d="M8 8h8M8 12h8" /></Icono>;
const IconBag = () => <Icono><path d="M6 2 3 6v14a2 2 0 0 0 2 2h14a2 2 0 0 0 2-2V6l-3-4z" /><path d="M3 6h18" /><path d="M16 10a4 4 0 0 1-8 0" /></Icono>;
const IconTrash = () => <Icono><path d="M3 6h18" /><path d="M8 6V4h8v2" /><path d="M19 6l-1 14H6L5 6" /><path d="M10 11v6M14 11v6" /></Icono>;
const IconPlus = () => <Icono><path d="M12 5v14M5 12h14" /></Icono>;





export default function FormComprobante({
  comprobante,
  soloLectura = false,            // si no viene, es un comprobante nuevo
  onCerrar,
  onCrear,
  onActualizar
}) {
    /* const {idComprobante, clienteNombreCompleto, fechaC = fecha, formaPagoNombre, total } = comprobante */
    const esNuevo = !comprobante;
  const nro = esNuevo ? null : `#${String(comprobante.idComprobante).padStart(4, "0")}`;

  //datos de api
  const [clientes, setClientes] = useState([]);
  const [productos, setProductos] = useState([]);
  const [formasPago, setFormasPago] = useState([]);
  const [cargando, setCargando] = useState(true);
  const [errorCarga, setErrorCarga] = useState(null);
  const [intento, setIntento] = useState(0);
  

  const [cliente, setCliente] = useState(null);
  const [formaPagoId, setFormaPagoId] = useState(String(comprobante?.idFormaPago ?? ""));
  const [fecha, setFecha] = useState(aInputFecha(comprobante?.fecha ?? new Date().toISOString()));
    const [activo, setActivo] = useState(comprobante ? Boolean(comprobante.activo) : true);
  const [detalle, setDetalle] = useState(
  (comprobante?.detalles ?? []).map((d) => ({
    productoId: d.idProducto,
    nombre: d.productoNombre,
    cantidad: d.cantidadProductos,
    precio: d.precioUnitario,
  })))

  const [productoId, setProductoId] = useState("");
  const [cantidad, setCantidad] = useState(1);
  const [error, setError] = useState(null);

  const productoElegido = productos.find((p) => String(p.idProducto) === productoId);


 const agregarProducto = () => {
  if (!productoElegido) return;
  const cant = Math.max(1, Number(cantidad) || 1);
  setDetalle((prev) => {
    const existe = prev.find((i) => i.productoId === productoElegido.idProducto);
    if (existe) {
      return prev.map((i) =>
        i.productoId === productoElegido.idProducto ? { ...i, cantidad: i.cantidad + cant } : i
      );
    }
    return [
      ...prev,
      {
        productoId: productoElegido.idProducto,
        nombre: productoElegido.descripcion,
        cantidad: cant,
        precio: productoElegido.preUnitario,
      },
    ];
  });
  setProductoId("");
  setCantidad(1);
  setError(null);
};

  const cambiarCantidad = (id, delta) =>
    setDetalle((prev) => prev.map((i) => (i.productoId === id ? { ...i, cantidad: Math.max(1, i.cantidad + delta) } : i)));

  const quitar = (id) => setDetalle((prev) => prev.filter((i) => i.productoId !== id));

  const guardar = (e) => {
    e.preventDefault();
    if (soloLectura) return;
    if (!cliente) return setError("Buscá y seleccioná un cliente de la lista.");
    if (cliente.activo === false) return setError("El cliente está deshabilitado. Elegí otro cliente para guardar.")
    if (!formaPagoId) return setError("Seleccioná una forma de pago.");
    if (!fecha) return setError("Indicá la fecha y hora.");
    if (detalle.length === 0) return setError("Agregá al menos un producto al detalle.");

    const datos = {
  idCliente: cliente.id,
  clienteNombreCompleto: `${cliente.nombre} ${cliente.apellido}`,
  idFormaPago: Number(formaPagoId),
  formaPagoNombre: formasPago.find((f) => String(f.idFormaPago) === formaPagoId)?.descripcion,
  fecha: new Date(fecha).toISOString(),
  activo,
  total,
  detalles: detalle.map((i) => ({
    idProducto: i.productoId,
    productoNombre: i.nombre,
    cantidadProductos: i.cantidad,
    precioUnitario: i.precio,
    subtotal: i.cantidad * i.precio,
  })),
};

    if (esNuevo) onCrear(datos);
    else onActualizar({ ...comprobante, ...datos });
  };
useEffect(() => {
    const onKey = (e) => e.key === "Escape" && onCerrar();
    document.addEventListener("keydown", onKey);
    const previo = document.body.style.overflow;
    document.body.style.overflow = "hidden";
    return () => {
      document.removeEventListener("keydown", onKey);
      document.body.style.overflow = previo;
    };
  }, [onCerrar]);

  const { unidades, total } = useMemo(
    () => ({
      unidades: detalle.reduce((s, i) => s + i.cantidad, 0),
      total: detalle.reduce((s, i) => s + i.cantidad * i.precio, 0),
    }),
    [detalle]
  );


useEffect(() => {
    const controller = new AbortController();
    async function cargar() {
      try {
        setCargando(true);
        setErrorCarga(null);
        const [cli, prod, pagos] = await Promise.all([
          clientesApi.listar(controller.signal),
          productosApi.listar(controller.signal),
          comprobantesApi.ObtenerformasPago(controller.signal),
        ]);
        setClientes(cli); // solo clientes activos
        setProductos(prod.filter((p) => p.activo !== false));
        setFormasPago(pagos);
        if (comprobante) {
            setCliente(cli.find((c) => c.id === comprobante.idCliente) ?? null);
        }
      } catch (e) {
        if (!axios.isCancel(e)) setErrorCarga("No se pudieron cargar los datos del formulario.");
      } finally {
        if (!controller.signal.aborted) setCargando(false);
      }
    }
    cargar();
    return () => controller.abort();
  }, [intento]);

  return (
    <div className={styles.overlay} onMouseDown={(e) => e.target === e.currentTarget && onCerrar()}>
      <div className={styles.dialog} role="dialog" aria-modal="true" aria-labelledby="ec-titulo">
        <form onSubmit={guardar} className="d-flex flex-column flex-grow-1 overflow-hidden" style={{ minHeight: 0 }}>
          <header className={`${styles.header} d-flex align-items-start justify-content-between gap-3`}>
            <div>
              <div className="d-flex flex-wrap align-items-center gap-3">
                <h2 id="ec-titulo" className={styles.title}>
                  {esNuevo ? "Nuevo Comprobante" : soloLectura ? `Comprobante ${nro}` : `Editar Comprobante ${nro}`}
              </h2>
                
              </div>
              <p className={styles.subtitle}>
                {soloLectura
                  ? "Detalle del comprobante (solo lectura)"
                  : esNuevo
                    ? "Completa los datos de cabecera y agrega los productos del detalle"
                    : "Completa los datos de cabecera y administra los productos del detalle"}
              </p>
            </div>
            <button type="button" className={`${styles.close} btn border-0`} aria-label="Cerrar" onClick={onCerrar}>
              <IconClose />
            </button>
          </header>

          <div className={styles.body}>
            {cargando && <p className={styles.estado} role="status">Cargando clientes, productos y formas de pago...</p>}

            {errorCarga && (
              <p className={styles.estado} role="alert">
                {errorCarga}{" "}
                <button type="button" className={`${styles.reintentar} btn`} onClick={() => setIntento((n) => n + 1)}>
                  Reintentar
                </button>
              </p>
            )}

            {!cargando && !errorCarga && (
              <fieldset disabled={soloLectura} className={styles.campos}>
                <section className={`${styles.panel} ${styles.panelVisible}`}  aria-labelledby="ec-datos">
                  <div className={`${styles.panelHead} d-flex align-items-center justify-content-between`}>
                    <h3 id="ec-datos" className={`${styles.panelTitle} ${styles.violet}`}>
                      <IconReceipt /> Datos del comprobante
                    </h3>
                  </div>

                  <div className={styles.panelBody}>
                    <div className="row g-3">
                      <div className="col-12 col-md-4">
                        <label className={styles.label}>Cliente</label>
                        <ClienteBuscador clientes={clientes} value={cliente} onChange={setCliente} />
                      </div>

                      <div className="col-12 col-md-4">
                        <label htmlFor="ec-pago" className={styles.label}>Forma de Pago</label>
                        <select id="ec-pago" className={`${styles.control} form-select`} value={formaPagoId} onChange={(e) => setFormaPagoId(e.target.value)}>
                          <option value="">Forma de pago…</option>
                          {formasPago.map((f) => (
                            <option key={f.idFormaPago} value={f.idFormaPago}>{f.descripcion}</option>
                          ))}
                        </select>
                      </div>

                      <div className="col-12 col-md-4">
                        <label htmlFor="ec-fecha" className={styles.label}>Fecha y Hora</label>
                        <input id="ec-fecha" type="datetime-local" className={`${styles.control} form-control`} value={fecha} onChange={(e) => setFecha(e.target.value)} />
                      </div>
                    </div>
                  </div>
                </section>

                <section className={styles.panel} aria-labelledby="ec-detalle">
                  <div className={styles.panelHead}>
                    <h3 id="ec-detalle" className={`${styles.panelTitle} ${styles.orange}`}>
                      <IconBag /> Productos / Detalle de venta
                    </h3>
                  </div>
                  {
                    !soloLectura && (
                        <div className={`${styles.addRow} row g-3 align-items-end`}>
                    <div className="col-12 col-md">
                      <label htmlFor="ec-producto" className={styles.label}>Producto</label>
                      <select id="ec-producto" className={`${styles.control} form-select`} value={productoId} onChange={(e) => setProductoId(e.target.value)}>
                        <option value="">Seleccionar producto…</option>
                        {productos.map((p) => (
                          <option key={p.idProducto} value={p.idProducto}>{p.descripcion}</option>
                        ))}
                      </select>
                    </div>
                    <div className="col-6 col-md-2">
                      <label htmlFor="ec-cantidad" className={styles.label}>Cantidad</label>
                      <input id="ec-cantidad" type="number" min="1" className={`${styles.control} form-control`} value={cantidad} onChange={(e) => setCantidad(e.target.value)} />
                    </div>
                    <div className="col-6 col-md-3">
                      <label htmlFor="ec-precio" className={styles.label}>Precio Unitario</label>
                      <input id="ec-precio" type="text" readOnly className={`${styles.control} ${styles.monoInput} form-control`} value={productoElegido ? moneda.format(productoElegido.preUnitario) : ""} 
                      placeholder="—" />
                    </div>
                    <div className="col-12 col-md-auto">
                      <button type="button" className={`${styles.addBtn} btn border-0`} aria-label="Agregar producto al detalle" disabled={!productoElegido} onClick={agregarProducto}>
                        <IconPlus />
                      </button>
                    </div>
                  </div>
                    )
                  }
                  

                  <div className="table-responsive">
                    <table className={`table ${styles.table} mb-0 align-middle`}>
                      <caption className="visually-hidden">Productos del comprobante</caption>
                      <thead>
                        <tr>
                          <th scope="col">Producto</th>
                          <th scope="col" className="text-center">Cantidad</th>
                          <th scope="col" className="text-end">Precio unitario</th>
                          <th scope="col" className="text-end">Subtotal</th>
                          {!soloLectura && <th scope="col" className="text-center">Quitar</th>}
                        </tr>
                      </thead>
                      <tbody>
                        {detalle.length === 0 ? (
                          <tr>
                            <td colSpan={soloLectura ? 4 : 5} className={styles.empty}>Todavía no hay productos en el detalle.</td>
                          </tr>
                        ) : (
                          detalle.map((i) => (
                            <tr key={i.productoId}>
                              <th scope="row" className={styles.prodName}>{i.nombre}</th>
                              <td className={soloLectura ? "text-center" : ""}>
                                {soloLectura ? (
                                  <span className={styles.stepValue}>{i.cantidad}</span>
                                  ) : (
                                  <div className={`${styles.stepper} d-inline-flex align-items-center mx-auto`}>
                                  <button type="button" className={styles.stepBtn} aria-label={`Quitar una unidad de ${i.nombre}`} onClick={() => cambiarCantidad(i.productoId, -1)} disabled={i.cantidad <= 1}>−</button>
                                  <output className={styles.stepValue}>{i.cantidad}</output>
                                  <button type="button" className={styles.stepBtn} aria-label={`Agregar una unidad de ${i.nombre}`} onClick={() => cambiarCantidad(i.productoId, 1)}>+</button>
                                  </div>)
                                }
                                
                              </td>
                              <td className={`${styles.mono} text-end`}>{moneda.format(i.precio)}</td>
                              <td className={`${styles.subtotal} text-end`}>{moneda.format(i.cantidad * i.precio)}</td>
                              {
                                !soloLectura &&(
                                  <td className="text-center">
                                    <button type="button" className={`${styles.trash} btn border-0`} aria-label={`Quitar ${i.nombre} del detalle`} onClick={() => quitar(i.productoId)}>
                                    <IconTrash />
                                    </button>
                                  </td>
                                )
                              }
                              
                            </tr>
                          ))
                        )}
                      </tbody>
                    </table>
                  </div>
                </section>
              </fieldset>
            )}

            {error && <p className={styles.error} role="alert">{error}</p>}
          </div>

          <footer className={`${styles.footer} d-flex flex-wrap align-items-center justify-content-between gap-3`}>
            <div>
              <p className={styles.resumen}>
                {unidades} {unidades === 1 ? "unidad" : "unidades"} en {detalle.length} {detalle.length === 1 ? "ítem" : "ítems"}
              </p>
              <p className={styles.totalRow}>
                <span className={styles.totalLabel}>Total a pagar:</span>{" "}
                <output className={styles.total}>{moneda.format(total)}</output>
              </p>
            </div>

            <div className="d-flex flex-wrap gap-3">
              <button type="button" className={`${styles.btnCancel} btn`} onClick={onCerrar}>
                {soloLectura ? "Cerrar" : "Cancelar"}
              </button>
              {!soloLectura && (
                <button type="submit" className={`${styles.btnSave} btn border-0`} disabled={cargando || Boolean(errorCarga)}>
                  {esNuevo ? "Crear Comprobante" : "Guardar Comprobante"}
                </button>
              )}
            </div>
          </footer>
        </form>
      </div>
    </div>
  );
}