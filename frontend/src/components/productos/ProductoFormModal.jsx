import { useEffect, useId, useRef, useState } from "react";
import Modal from "../clientes/Modal";
import Select from "../select";
import IconProduct from '../../assets/IconProduct.svg'
import base from "../clientes/ClienteFormModal.module.css"; // header, body, label, input, footer, botones
import styles from "./ProductoFormModal.module.css";

const NO_ALFANUM = /[^a-zA-Z0-9áéíóúÁÉÍÓÚñÑüÜ ]/g;
const REGEX_NOMBRE = /^[a-zA-Z0-9áéíóúÁÉÍÓÚñÑüÜ ]+$/;
const REGEX_PRECIO = /^\d+(\.\d{1,2})?$/;
const PRECIO_MIN = 0.01;
const PRECIO_MAX = 9999999.99;

const CAMPOS = ["descripcion", "idTamano", "idTipo", "precio"];

const limpiarNombre = (valor) =>
  valor.replace(NO_ALFANUM, "").replace(/^\s+/, "").replace(/\s{2,}/g, " ");

// Solo dígitos, un separador decimal (coma o punto) y hasta 2 decimales
const limpiarPrecio = (valor) => {
  const v = valor.replace(",", ".").replace(/[^\d.]/g, "");
  const [entero, ...resto] = v.split(".");
  if (resto.length === 0) return entero.slice(0, 7);
  return `${entero.slice(0, 7)}.${resto.join("").slice(0, 2)}`;
};

const validar = (datos) => {
  const errores = {};

  const nombre = datos.descripcion.trim();
  if (!nombre) errores.descripcion = "El nombre es obligatorio.";
  else if (nombre.length < 5) errores.descripcion = "Debe tener al menos 3 caracteres.";
  else if (nombre.length > 60) errores.descripcion = "No puede superar los 60 caracteres.";
  else if (!REGEX_NOMBRE.test(nombre)) errores.descripcion = "Solo se permiten letras y números.";

  if (!datos.idTipo) errores.idTipo = "Seleccioná un tipo.";
  if (!datos.idTamano) errores.idTamano = "Seleccioná un tamaño.";

  const precio = String(datos.precio).trim();
  if (!precio) errores.precio = "El precio es obligatorio.";
  else if (!REGEX_PRECIO.test(precio)) errores.precio = "Ingresá un número válido, con hasta 2 decimales.";
  else if (Number(precio) < 100) errores.precio = "El precio mínimo es $100";
  else if (Number(precio) > 1000000) errores.precio = "Precio máximo superado";

  return errores;
};

export default function ProductoFormModal({
  titulo,
  descripcion,
  textoGuardar,
  tipos = [],
  tamanos = [],
  inicial,
  mostrarIconoAuto = false,
  onCerrar,
  onGuardar,
}) {
  const uid = useId();
  const tituloId = `${uid}-titulo`;
  const descId = `${uid}-desc`;
  const nombreRef = useRef(null);

  const [datos, setDatos] = useState({
    descripcion: "",
    idTipo: "",
    idTamano: "",
    precio: "",
    activo: true,
    ...inicial,
  });

  const [seleccionados, setSeleccionado] = useState({});

  useEffect(() => {
    nombreRef.current?.focus();
  }, []);

  const errores = validar(datos);
  const mensaje = (campo) => (seleccionados[campo] ? errores[campo] : undefined);


  const cambiar = (e) => {
    const { name, value } = e.target;
    let limpio = value;
    if (name === "descripcion") limpio = limpiarNombre(value);
    if (name === "precio") limpio = limpiarPrecio(value);
    setDatos((prev) => ({ ...prev, [name]: limpio }));
    // Los selects no tienen "salir de campo" útil: se marcan al elegir
    if (name === "idTipo" || name === "idTamano") {
      setSeleccionado((prev) => ({ ...prev, [name]: true }));
    }
  };

  const salirDeCampo = (e) => {
    const { name } = e.target;
    setSeleccionado((prev) => ({ ...prev, [name]: true }));
  };

  const propsCampo = (campo) => ({
    id: `${uid}-${campo}`,
    name: campo,
    value: datos[campo],
    onChange: cambiar,
    onBlur: salirDeCampo,
    className: `${base.input} ${mensaje(campo) ? base.inputInvalido : ""} form-control`,
    "aria-invalid": mensaje(campo) ? "true" : undefined,
    "aria-describedby": mensaje(campo) ? `${uid}-${campo}-error` : undefined,
  });

  const propsSelect = (campo) => ({
    id: `${uid}-${campo}`,
    name: campo,
    value: datos[campo],
    onChange: cambiar,
    onBlur: salirDeCampo,
    className: `${base.input} ${mensaje(campo) ? base.inputInvalido : ""}`,
    "aria-invalid": mensaje(campo) ? "true" : undefined,
    "aria-describedby": mensaje(campo) ? `${uid}-${campo}-error` : undefined,
  });

  const errorDe = (campo) =>
    mensaje(campo) && (
      <p id={`${uid}-${campo}-error`} className={base.error} role="alert">
        {mensaje(campo)}
      </p>
    );

  const enviar = (e) => {
    e.preventDefault();

    const errs = validar(datos);
    const primero = CAMPOS.find((c) => errs[c]);
    if (primero) {
      setSeleccionado({ descripcion: true, idTamano: true, idTipo: true, precio: true });
      e.currentTarget.elements[primero]?.focus();
      return;
    }

    onGuardar({
      descripcion: datos.descripcion.trim(),
      idTipo: Number(datos.idTipo),
      idTamano: Number(datos.idTamano),
      precio: Number(datos.precio),
      activo: datos.activo,
    });
  };

  return (
    <Modal onCerrar={onCerrar} labelledBy={tituloId} describedBy={descId}>
      <form className={base.form} onSubmit={enviar} noValidate>
        <header className={base.header}>
          <div>
            <h2 id={tituloId} className={base.title}>{titulo}</h2>
            <p id={descId} className={base.subtitle}>{descripcion}</p>
          </div>
          <button type="button" className={`${base.close} btn border-0`} aria-label="Cerrar" onClick={onCerrar}>
            <i className="bi bi-x-lg" aria-hidden="true" />
          </button>
        </header>

        <div className={base.body}>
          <div>
            <label htmlFor={`${uid}-descripcion`} className={base.label}>Nombre del producto *</label>
            <input
              {...propsCampo("descripcion")}
              ref={nombreRef}
              type="text"
              placeholder="Ej. Balde Popcorn XL con Caramelo"
              autoComplete="off"
              maxLength={100}
              required
            />
            {errorDe("descripcion")}
          </div>

          <div className="row g-3">
            <div className="col-6">
              <label htmlFor={`${uid}-idTamano`} className={base.label}>Tamaño *</label>
              <Select {...propsSelect("idTamano")}>
                <option value="">Seleccionar...</option>
                {tamanos.map((t) => (
                  <option key={t.id} value={t.id}>{t.descripcion}</option>
                ))}
              </Select>
              {errorDe("idTamano")}
            </div>

            <div className="col-6">
              <label htmlFor={`${uid}-idTipo`} className={base.label}>Tipo *</label>
              <Select {...propsSelect("idTipo")}>
                <option value="">Seleccionar...</option>
                {tipos.map((t) => (
                  <option key={t.id} value={t.id}>{t.descripcion}</option>
                ))}
              </Select>
              {errorDe("idTipo")}
            </div>
          </div>

          <div className="row">
            <div className="col-6">
              <label htmlFor={`${uid}-precio`} className={base.label}>Precio unitario ($) *</label>
              <input
                {...propsCampo("precio")}
                type="text"
                inputMode="decimal"
                className={`${base.input} ${styles.mono} ${mensaje("precio") ? base.inputInvalido : ""} form-control`}
                placeholder="6800,50"
                autoComplete="off"
                maxLength={10}
                required
              />
              {errorDe("precio")}
            </div>
          </div>

          {mostrarIconoAuto && (
            <div className={styles.autoIcon}>
              <span className={`${styles.iconTile} d-flex align-items-center justify-content-center`} aria-hidden="true">
                <img src={IconProduct} alt="" />
              </span>
              <div>
                <p className={styles.autoTitle}>Ícono automático de producto</p>
                <p className={styles.autoSub}>Popcorn clásico ámbar Cineplanet</p>
              </div>
            </div>
          )}
        </div>

        <footer className={base.footer}>
          <button type="button" className={`${base.cancel} btn`} onClick={onCerrar}>Cancelar</button>
          <button type="submit" className={`${base.submit} btn`}>{textoGuardar}</button>
        </footer>
      </form>
    </Modal>
  );
}