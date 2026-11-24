import { useEffect, useState } from "react";
import stylesCli from "../clientes/Clientes.module.css";
import styles from "./Dashboard.module.css";
import activos from "../../assets/dashboardIcons/activos.svg"
import comida from "../../assets/dashboardIcons/comida.svg"
import entradas from "../../assets/dashboardIcons/entradas.svg"
import ingresos from "../../assets/dashboardIcons/ingresos.svg"
import * as dashboardApi from "../../services/dashboardApi"
import IconProduct from "../../assets/IconProduct.svg"
import { Link } from "react-router";

const MAX_BAR = 150;

const money = (n = 0) =>
  new Intl.NumberFormat("es-AR", {
    style: "currency",
    currency: "ARS",
    maximumFractionDigits: 0,
  }).format(n);
  
const num = (n = 0) => new Intl.NumberFormat("es-AR").format(n);
const cap = (s) => s.charAt(0).toUpperCase() + s.slice(1).replace(".", "");


const fmtDia = (iso) =>
  cap(new Date(iso).toLocaleDateString("es-AR", { weekday: "short", timeZone: "UTC" }));
const fmtCorta = (iso) =>
  new Date(iso).toLocaleDateString("es-AR", { day: "2-digit", month: "2-digit", timeZone: "UTC" });
const fmtLarga = (iso) =>
  new Date(iso)
    .toLocaleDateString("es-AR", { weekday: "short", day: "numeric", month: "short", timeZone: "UTC" })
    .replace(/\./g, "")
    .split(" ")
    .map((w, i) => (i === 0 ? cap(w) : cap(w)))
    .join(" ")
    .replace(",", "");

const text = {
  green: styles.textGreen,
  orange: styles.textOrange,
  purple: styles.textPurple,
  muted: styles.textMutedD,
};

const tone = {
  purple: styles.tonePurple,
  green: styles.toneGreen,
  orange: styles.toneOrange,
};

const productTones = ["orange", "orange", "purple", "green"];
const productIcons = ["bi-cup-hot", "bi-cup-hot", "bi-cup", "bi-layers"];
const rankLabels = ["Mayor pico", "2º lugar", "3º lugar"];

const movies = [
  { title: "Duna: Imperio del Cosmos", meta: "IMAX Laser • D-BOX • 2D", total: "$1.640.000", tickets: "624 entradas", tone: "green" },
  { title: "Gladiador: Arena Eterna", meta: "2D • Comfort VIP", total: "$1.125.000", tickets: "458 entradas", tone: "green" },
  { title: "Tron: Ares", meta: "IMAX Laser • 3D • D-BOX", total: "$940.500", tickets: "392 entradas", tone: "orange" },
  { title: "Horizonte Cósmico", meta: "2D • Comfort", total: "$612.000", tickets: "268 entradas", tone: "muted" },
];

export default function Dashboard() {

   const [resumen, setResumen] = useState(null);
  const [semana, setSemana] = useState(null);
  const [topDays, setTopDays] = useState([]);
  const [products, setProducts] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  
  useEffect(() => {
    let cancelled = false;

    async function load() {
      try {
        const [r, s, f, p] = await Promise.all([
          dashboardApi.resumen(),
          dashboardApi.ingresosSemana(),
          dashboardApi.fechasMasEntradas(),
          dashboardApi.ProductosMasVendidos(),
        ]);
        if (cancelled) return;
        setResumen(r);
        setSemana(s);
        setTopDays(f);
        setProducts(p);
      } catch (e) {
        if (!cancelled) setError(e.message);
      } finally {
        if (!cancelled) setLoading(false);
      }
    }

    load();
    return () => {
      cancelled = true;
    };
  }, []);

  if (loading) return <p className="text-white p-4">Cargando...</p>;
  if (error) return <p className="text-danger p-4" role="alert">{error}</p>;

  const stats = [
    { label: "Ingresos totales", value: money(resumen.ingresosTotales), icon: ingresos, tone: "purple" },
    { label: "Entradas vendidas", value: num(resumen.cantidadEntradasVendidas), icon: entradas, tone: "green" },
    { label: "Ventas Productos", value: money(resumen.ingresosProductos), icon: comida, tone: "orange", accent: true },
    { label: "Clientes activos", value: num(resumen.clientesActivos), icon: activos, tone: "purple" },
  ];

  const dias = semana?.dias ?? [];
  const maxBar = Math.max(1, ...dias.flatMap((d) => [d.ingresoEntradas, d.ingresoProductos]));
  const barH = (v) => Math.round((v / maxBar) * MAX_BAR);
  const picoFecha = semana?.pico?.fecha;

  const ticketPromedio = resumen.cantidadEntradasVendidas
    ? resumen.ingresosEntradas / resumen.cantidadEntradasVendidas
    : 0;

  return (
    <div className={`${stylesCli.main} ${styles.dash} flex-grow-1`}>
        <header className={`${stylesCli.topbar} d-flex flex-wrap align-items-center justify-content-between gap-3`}>
            <div className="d-flex flex-wrap align-items-center gap-3">
                <h1 className={stylesCli.title}>Dashboard</h1>
            </div>
        </header>

      <main className="p-4">
        {/* KPIs */}
        <section aria-label="Indicadores principales" className="row g-3 mb-4">
          {stats.map((s) => (
            <div className="col-6 col-lg-3 " key={s.label}>
              <article className={`${styles.dcard} ${styles.stat} h-100`}>
                <div className="d-flex justify-content-between align-items-start">
                  <div>
                    <h2 className={`${styles.statLabel} ${s.accent ? styles.textOrange : ""}`}>{s.label}</h2>
                    <p className={`${styles.statValue} ${s.accent ? styles.textOrange : ""}`}>{s.value}</p>
                  </div>
                  <span className={`${styles.iconBox} ${tone[s.tone]}`}>
                    <img src={s.icon} alt="" className={`bi ${s.icon}`} />
                  </span>
                </div>
              </article>
            </div>
          ))}
        </section>

        {/* Gráfico */}
        <section className={`${styles.dcard} p-4 mb-4`} aria-labelledby="chart-title">
          <div className="d-flex justify-content-between align-items-start">
            <div>
              <h2 id="chart-title" className={styles.cardTitle}>Ingresos por Taquilla y Productos</h2>
              <p className={styles.cardSub}>Comparativa de recaudación bruta en pesos (ARS)</p>
            </div>
            <ul className={`${styles.legend} list-unstyled d-flex gap-3 m-0`}>
              <li><span className={`${styles.dot} ${styles.bgPurple}`} />Taquilla</li>
              <li><span className={`${styles.dot} ${styles.bgOrange}`} />Productos</li>
            </ul>
          </div>

          <figure className={`${styles.chart} m-0`} role="img" aria-label="Gráfico de barras semanal">
            {dias.map((x) => {
              const peak = x.fecha === picoFecha;
              return (
                <div className={styles.chartCol} key={x.fecha}>
                  <div className={styles.bars}>
                    {peak && <span className={styles.peakTip}>{money(x.ingresoTotal)}</span>}
                    <span
                      className={`${styles.bar} ${styles.barPurple}`}
                      style={{ height: barH(x.ingresoEntradas) }}
                      title={money(x.ingresoEntradas)}
                    />
                    <span
                      className={`${styles.bar} ${styles.barOrange}`}
                      style={{ height: barH(x.ingresoProductos) }}
                      title={money(x.ingresoProductos)}
                    />
                  </div>
                  <div className={`${styles.day} ${peak ? styles.peak : ""}`}>
                    <strong>{fmtDia(x.fecha)}</strong>
                    <small>{fmtCorta(x.fecha)}</small>
                  </div>
                </div>
              );
            })}
          </figure>

          <div className="d-flex justify-content-between align-items-center mt-4 mb-2 small">
            <span className={styles.textLightD}>
              <i className={`bi bi-calendar-check ${styles.textOrange} me-2`} />
              Fechas con más entradas vendidas
            </span>
            <span className={styles.textMutedD}>
              Entrada promedio: <strong className="text-white">{money(ticketPromedio)}</strong>
            </span>
          </div>
          <ol className="row g-3 list-unstyled m-0">
            {topDays.map((t, i) => (
              <li className="col-12 col-md-4" key={t.fecha}>
                <div className={`${styles.dayCard} ${i === 0 ? styles.active : ""}`}>
                  <div>
                    <div className="fw-semibold small text-white">{fmtLarga(t.fecha)}</div>
                    <small className={i === 0 ? styles.textOrange : styles.textMutedD}>{rankLabels[i]}</small>
                  </div>
                  <strong className={i === 1 ? styles.textPurple : "text-white"}>
                    {num(t.cantidadEntradas)} ent.
                  </strong>
                </div>
              </li>
            ))}
          </ol>
        </section>

        <div className="row g-4">
          <section className="col-lg-6" aria-labelledby="movies-title">
            <div className={`${styles.dcard} p-4 h-100`}>
              <div className="d-flex justify-content-between">
                <div>
                  <h2 id="movies-title" className={styles.cardTitle}>Top Películas en Cartelera</h2>
                  <p className={styles.cardSub}>Rendimiento de taquilla por título</p>
                </div>
                {/* <a href="#" className={styles.linkPurple}>Ver cartelera</a> */}
              </div>
              <ul className="list-unstyled m-0 mt-4">
                {movies.map((m) => (
                  <li className={styles.rowItem} key={m.title}>
                    {/* <span className={styles.poster} aria-hidden="true" /> */}
                    <div className="flex-grow-1">
                      <div className={styles.itemName}>{m.title}</div>
                      <small className={styles.textMutedD}>{m.meta}</small>
                    </div>
                    <div className="text-end">
                      <div className={styles.itemTotal}>{m.total}</div>
                      <small className={text[m.tone]}>{m.tickets}</small>
                    </div>
                  </li>
                ))}
              </ul>
            </div>
          </section>

          <section className="col-lg-6" aria-labelledby="products-title">
            <div className={`${styles.dcard} h-100 d-flex flex-column overflow-hidden`}>
              <div className="p-4 pb-0 flex-grow-1">
                <div className="d-flex justify-content-between">
                  <div>
                    <h2 id="products-title" className={styles.cardTitle}>Productos Más Vendidos</h2>
                    <p className={styles.cardSub}>Ranking de Productos y Combos</p>
                  </div>
                  <Link to="/admin/productos" className={styles.linkOrange}>Ver catálogo</Link>
                </div>
                <ul className="list-unstyled m-0 mt-4">
                  {products.map((p, i) => {
                    const unitario = p.cantidadVendida ? p.totalRecaudado / p.cantidadVendida : 0;
                    return (
                      <li className={styles.rowItem} key={p.idProducto}>
                        <span className={`${styles.iconBox} ${tone[productTones[i % 4]]}`}>
                          <img  src={IconProduct} className={`bi ${productIcons[i % 4]}`} />
                        </span>
                        <div className="flex-grow-1">
                          <div className={styles.itemName}>{p.descripcion}</div>
                          <small className={styles.textMutedD}>
                            {p.tamaño} • {money(unitario)} c/u
                          </small>
                        </div>
                        <div className="text-end">
                          <div className={`fw-bold ${i < 2 ? styles.textOrange : "text-white"}`}>
                            {num(p.cantidadVendida)} u.
                          </div>
                          <small className={styles.textLightD}>{money(p.totalRecaudado)}</small>
                        </div>
                      </li>
                    );
                  })}
                </ul>
              </div>
            </div>
          </section>
        </div>
      </main>
    </div>
  );
}