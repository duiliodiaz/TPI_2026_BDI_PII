import { useId, useMemo, useState } from "react";
import styles from "./FormComprobante.module.css";

const etiqueta = (c) => `${c.nombre} ${c.apellido} - DNI ${c.dni}`;
const MAX_SUGERENCIAS = 8;

export default function ClienteBuscador({ clientes, value, onChange, disabled }) {
  const id = useId();
  const [texto, setTexto] = useState(value ? etiqueta(value) : "");
  const [abierto, setAbierto] = useState(false);
  const [indice, setIndice] = useState(0);
  const [bloqueado, setBloqueado] = useState(null); // cliente deshabilitado que se intentó elegir

  const sugerencias = useMemo(() => {
    const q = texto.trim().toLowerCase();
    if (!q) return [];
    const qDni = q.replace(/\D/g, "");
    return clientes
      .filter(
        (c) =>
          `${c.nombre} ${c.apellido}`.toLowerCase().includes(q) ||
          (qDni && String(c.dni).replace(/\D/g, "").includes(qDni))
      )
      .slice(0, MAX_SUGERENCIAS);
  }, [clientes, texto]);

  const hayTexto = texto.trim() !== "";
  const sinResultados = abierto && hayTexto && !value && sugerencias.length === 0;
  const mostrarLista = abierto && sugerencias.length > 0 && !value;
  const deshabilitado = bloqueado ?? (value?.activo === false ? value : null);

  const escribir = (e) => {
    setTexto(e.target.value);
    setIndice(0);
    setAbierto(true);
    setBloqueado(null);
    if (value) onChange(null); // si edita el texto, se pierde la selección
  };

   const elegir = (cliente) => {
    if (cliente.activo === false) {
      setBloqueado(cliente);
      onChange(null);
      setTexto(etiqueta(cliente));
      setAbierto(false);
      return;
    }
    setBloqueado(null);
    onChange(cliente);
    setTexto(etiqueta(cliente));
    setAbierto(false);
  };

   const teclado = (e) => {
    if (e.key === "ArrowDown" && sugerencias.length) {
      e.preventDefault();
      setAbierto(true);
      setIndice((i) => (i + 1) % sugerencias.length);
    } else if (e.key === "ArrowUp" && sugerencias.length) {
      e.preventDefault();
      setIndice((i) => (i - 1 + sugerencias.length) % sugerencias.length);
    } else if (e.key === "Enter" && mostrarLista) {
      e.preventDefault();
      elegir(sugerencias[indice]);
    } else if (e.key === "Escape" && abierto) {
      e.stopPropagation();
      setAbierto(false);
    }
  };

  return (
    <div className={styles.combo}>
      <input
        id={id}
        type="search"
        role="combobox"
        aria-expanded={mostrarLista}
        aria-controls={`${id}-lista`}
        aria-autocomplete="list"
        aria-activedescendant={mostrarLista ? `${id}-op-${indice}` : undefined}
        aria-describedby={deshabilitado ? `${id}-aviso` : undefined}
        className={`${styles.control} ${styles.comboInput} form-control`}
        placeholder="Buscar cliente por nombre o DNI..."
        autoComplete="off"
        disabled={disabled}
        value={texto}
        onChange={escribir}
        onFocus={() => setAbierto(true)}
        onBlur={() => setAbierto(false)}
        onKeyDown={teclado}
      />

      {mostrarLista && (
        <ul id={`${id}-lista`} role="listbox" className={styles.listbox}>
          {sugerencias.map((c, i) => (
            <li
              key={c.id}
              id={`${id}-op-${i}`}
              role="option"
              aria-selected={i === indice}
              className={`${styles.option} ${i === indice ? styles.optionActive : ""} ${c.activo === false ? styles.optionOff : ""}`}
              onMouseDown={(e) => e.preventDefault()} // evita perder el foco antes del click
              onClick={() => elegir(c)}
              onMouseEnter={() => setIndice(i)}
            >
              <span className="d-flex align-items-center justify-content-between gap-2">
                <span>{c.nombre} {c.apellido}</span>
                {c.activo === false && <span className={styles.badgeOff}>Deshabilitado</span>}
              </span>
              <span className={styles.optionDni}>DNI {c.dni}</span>
            </li>
          ))}
        </ul>
      )}

      {sinResultados && (
        <p className={styles.noResult} role="status">
          El cliente no existe.
        </p>
      )}
      {deshabilitado && (
        <p id={`${id}-aviso`} className={styles.avisoOff} role="alert">
          {deshabilitado.nombre} {deshabilitado.apellido} está deshabilitado y no puede usarse en un comprobante.
        </p>
      )}
    </div>
  );
}