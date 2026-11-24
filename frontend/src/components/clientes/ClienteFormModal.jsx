import { useEffect, useId, useRef, useState } from "react";
import Modal from "./Modal";
import styles from "./ClienteFormModal.module.css";

const NO_LETRAS = /[^a-zA-ZáéíóúÁÉÍÓÚñÑ ]/g;
const REGEX_NOMBRE = /^[a-zA-ZáéíóúÁÉÍÓÚñÑ ]+$/;
const REGEX_EMAIL = /^[^\s@]+@[^\s@]+\.[^\s@]{2,}$/;
const REGEX_DNI = /^\d{1,2}\.\d{3}\.\d{3}$/;

const CAMPOS = ["nombre", "apellido", "dni", "email"];

const limpiarTexto = (valor) =>
  valor.replace(NO_LETRAS, "").replace(/^\s+/, "").replace(/\s{2,}/g, " ");

const formatearDni = (valor) =>
  valor
    .replace(/\D/g, "")
    .slice(0, 8)
    .replace(/\B(?=(\d{3})+(?!\d))/g, ".");

const validarTexto = (valor, etiqueta) => {
  const v = valor.trim();
  if (!v) return `Ingresá el ${etiqueta}.`;
  if (v.length < 2) return `El ${etiqueta} debe tener al menos 2 letras.`;
  if (!REGEX_NOMBRE.test(v)) return "Solo se permiten letras.";
  return undefined;
};
const validar = (d, clientes = []) => {
  const errores = {};

  const errNombre = validarTexto(d.nombre, "nombre");
  if (errNombre) errores.nombre = errNombre;

  const errApellido = validarTexto(d.apellido, "apellido");
  if (errApellido) errores.apellido = errApellido;

  const dni = d.dni.trim();
  if (!dni) {
    errores.dni = "Ingresá el DNI.";
  } else if (!REGEX_DNI.test(dni)) {
    errores.dni = "Formato inválido. Ejemplo: 42.890.112";
  } else {
    const n = parseInt(dni.replace(/\./g, ""), 10);
    if (n < 1000000 || n > 99999999) {
      errores.dni = "El DNI debe estar entre 1.000.000 y 99.999.999.";
    }
  }

  const email = d.email.trim();
  if (!email) {
    errores.email = "Ingresá el correo electrónico.";
  } else if (email.length > 100) {
    errores.email = "El correo no puede superar los 100 caracteres.";
  } else if (!REGEX_EMAIL.test(email)) {
    errores.email = "Ingresá un correo válido. Ejemplo: nombre@dominio.com";
  } else if (
    clientes.some(
      (c) => c.id !== d.id && c.email?.trim().toLowerCase() === email.toLowerCase()
    )
  ) {
    errores.email = "Ya existe un cliente con ese correo.";
  }

  return errores;
};

export default function ClienteFormModal({
  titulo,
  descripcion,
  textoGuardar,
  data,
  clientes, 
  onCerrar,
  onGuardar,
}) {
  const uid = useId();
  const tituloId = `${uid}-titulo`;
  const descId = `${uid}-desc`;
  const nombreRef = useRef(null);

  const [datos, setDatos] = useState({
    nombre: "",
    apellido:"",
    dni: "",
    email: "",
    activo:true,
    ...data,
  });
  const [seleccionados, setSeleccionado] = useState({});

  useEffect(() => {
    nombreRef.current?.focus();
  }, []);
  const errores = validar(datos, clientes);
  const mensaje = (campo) => (seleccionados[campo] ? errores[campo] : undefined);

  const cambiar = (e) => {
    const { name, value } = e.target;
    let limpio = value;
    if (name === "nombre" || name === "apellido") limpio = limpiarTexto(value);
    if (name === "dni") limpio = formatearDni(value);
    setDatos((prev) => ({ ...prev, [name]: limpio }));
    
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
    className: `${styles.input} ${mensaje(campo) ? styles.inputInvalido : ""} form-control`,
    "aria-invalid": mensaje(campo) ? "true" : undefined,
    "aria-describedby": mensaje(campo) ? `${uid}-${campo}-error` : undefined,
  });

 const errorDe = (campo) =>
    mensaje(campo) && (
      <p id={`${uid}-${campo}-error`} className={styles.error} role="alert">
        {mensaje(campo)}
      </p>
    );
const enviar = (e) => {
    e.preventDefault();

    const errs = validar(datos, clientes);
    const primero = CAMPOS.find((c) => errs[c]);
    if (primero) {
      setSeleccionado({ nombre: true, apellido: true, dni: true, email: true });
      e.currentTarget.elements[primero]?.focus();
      return;
    }

    onGuardar({
      ...datos,
      nombre: datos.nombre.trim(),
      apellido: datos.apellido.trim(),
      email: datos.email.trim(),
      dni: parseInt(datos.dni.replace(/\./g, ""), 10),
    });
  };

  return (
    <Modal onCerrar={onCerrar} labelledBy={tituloId} describedBy={descId}>
      <form className={styles.form} onSubmit={enviar} noValidate>
        <header className={styles.header}>
          <div>
            <h2 id={tituloId} className={styles.title}>{titulo}</h2>
            <p id={descId} className={styles.subtitle}>{descripcion}</p>
          </div>
          <button
            type="button"
            className={`${styles.close} btn border-0`}
            aria-label="Cerrar"
            onClick={onCerrar}
          >
            <i className="bi bi-x-lg" aria-hidden="true" />
          </button>
        </header>

        <div className={styles.body}>
          <div className="row g-3">
            <div className="col-6">
              <label htmlFor={`${uid}-nombres`} className={styles.label}>
                Nombre *
              </label>
              <input
                 {...propsCampo("nombre")}
                ref={nombreRef}
                type="text"
                placeholder="Ej. Camila"
                autoComplete="given-name"
                maxLength={50}
                required
              />
              {errorDe("nombre")}
            </div>

            <div className="col-6">
              <label htmlFor={`${uid}-apellido`} className={styles.label}>
                Apellido *
              </label>
              <input
                {...propsCampo("apellido")}
                type="text"
                placeholder="Ej. Morales"
                autoComplete="family-name"
                maxLength={50}
                required
              />
              {errorDe("apellido")}
            </div>
          </div>

          <div>
            <label htmlFor={`${uid}-dni`} className={styles.label}>
              DNI *
            </label>
            <input
              {...propsCampo("dni")}
              type="text"
              inputMode="numeric"
              placeholder="42.890.112"
              autoComplete="off"
              maxLength={10}
              required
            />
            {errorDe("dni")}
          </div>

          <div>
            <label htmlFor={`${uid}-email`} className={styles.label}>
              Correo electrónico *
            </label>
            <input
              {...propsCampo("email")}
              type="email"
              placeholder="example@gmail.com"
              autoComplete="email"
              maxLength={100}
              required
            />
            {errorDe("email")}
          </div>
        </div>

        <footer className={styles.footer}>
          <button type="button" className={`${styles.cancel} btn`} onClick={onCerrar}>
            Cancelar
          </button>
          <button type="submit" className={`${styles.submit} btn`}>
            {textoGuardar}
          </button>
        </footer>
      </form>
    </Modal>
  );
}