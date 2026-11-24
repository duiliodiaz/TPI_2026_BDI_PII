import { useState, useMemo } from "react";
import { useNavigate } from "react-router";
import { login } from "../../services/dashboardApi" 
import styles from "./Login.module.css";
import logo from "../../assets/cineplanet-logo.png"



const Svg = ({ children, size = 18, ...p }) => (
  <svg width={size} height={size} viewBox="0 0 24 24" fill="none" stroke="currentColor"
    strokeWidth="1.8" strokeLinecap="round" strokeLinejoin="round" aria-hidden="true" {...p}>
    {children}
  </svg>
);
const IconMail = () => (<Svg><rect x="3" y="5" width="18" height="14" rx="2" /><path d="m3 7 9 6 9-6" /></Svg>);
const IconLock = () => (<Svg><rect x="5" y="11" width="14" height="9" rx="2" /><path d="M8 11V8a4 4 0 0 1 8 0v3" /></Svg>);
const IconEye = () => (<Svg><path d="M2 12s3.5-7 10-7 10 7 10 7-3.5 7-10 7S2 12 2 12Z" /><circle cx="12" cy="12" r="3" /></Svg>);
const IconEyeOff = () => (<Svg><path d="M3 3l18 18" /><path d="M10.6 5.1A10 10 0 0 1 12 5c6.5 0 10 7 10 7a17 17 0 0 1-3.2 4.1M6.5 6.6A17 17 0 0 0 2 12s3.5 7 10 7a9.7 9.7 0 0 0 4-.9" /><path d="M9.9 9.9a3 3 0 0 0 4.2 4.2" /></Svg>);
const IconArrowLeft = () => (<Svg size={16}><path d="M19 12H5" /><path d="m11 18-6-6 6-6" /></Svg>);
const IconShield = () => (<Svg size={15} style={{ color: "#a78bfa" }}><path d="M12 3 5 6v6c0 4.5 3 7.5 7 9 4-1.5 7-4.5 7-9V6l-7-3Z" /><path d="m9 12 2 2 4-4" /></Svg>);
const IconUserCircle = () => (<Svg size={18} style={{ color: "#f59e0b" }}><circle cx="12" cy="12" r="9" /><circle cx="12" cy="10" r="3" /><path d="M6.2 18.2a7 7 0 0 1 11.6 0" /></Svg>);
const IconPlanet = () => (
  <svg width="48" height="40" viewBox="0 0 48 40" fill="none" aria-hidden="true">
    <circle cx="24" cy="20" r="13" fill="#a78bfa" />
    <circle cx="19" cy="15" r="2.6" fill="#1a1530" /><circle cx="29" cy="15" r="2.6" fill="#1a1530" />
    <circle cx="19" cy="25" r="2.6" fill="#1a1530" /><circle cx="29" cy="25" r="2.6" fill="#1a1530" />
    <circle cx="24" cy="20" r="2.6" fill="#1a1530" />
    <ellipse cx="24" cy="21" rx="22" ry="7" transform="rotate(-18 24 21)" stroke="#a78bfa" strokeWidth="2.4" />
  </svg>
);

const USER_REGEX = /^[A-Za-zÁÉÍÓÚÜÑáéíóúüñ]+$/;

const validateUser = (value) => {
  if (!value.trim()) return "Ingresa tu nombre de usuario.";
  if (value.length > 64) return `El usuario no puede tener más de 64 caracteres.`;
  if (!USER_REGEX.test(value)) return "Usa solo letras, sin números, espacios ni símbolos.";
  return "";
};

const validatePassword = (value) => {
  if (!value) return "Ingresa tu contraseña.";
  if (/\s/.test(value)) return "La contraseña no puede contener espacios.";
  if (value.length < 7) return `La contraseña debe tener al menos 7 caracteres.`;
  if (value.length > 64) return `La contraseña no puede tener más de 64 caracteres.`;
  return "";
};

const getServerError = (err) => {
  const status = err?.response?.status;
  if (status === 400 || status === 401 || status === 403) return "Usuario o contraseña incorrectos.";
  if (status >= 500) return "El servidor tuvo un problema. Inténtalo de nuevo en unos minutos.";
  if (err?.code === "ECONNABORTED") return "El servidor tardó demasiado en responder. Inténtalo de nuevo.";
  if (!err?.response) return "No se pudo conectar con el servidor. Revisa tu conexión.";
  return "No se pudo iniciar sesión. Inténtalo de nuevo.";
};

export default function Login() {
  const navigate = useNavigate();
  const [showPassword, setShowPassword] = useState(false);
  const [form, setForm] = useState({ user: "", password: "", remember: false });
  const [touched, setTouched] = useState({ user: false, password: false });
  const [loading, setLoading] = useState(false);
  const [serverError, setServerError] = useState("");

const errors = useMemo(
    () => ({ user: validateUser(form.user), password: validatePassword(form.password) }),
    [form.user, form.password]
  );

  const handleChange = (e) => {
    const { name, value, type, checked } = e.target;
    setForm((f) => ({ ...f, [name]: type === "checkbox" ? checked : value }));
    if (serverError) setServerError("");
  };

  const handleBlur = (e) => {
    const { name } = e.target;
    setTouched((t) => ({ ...t, [name]: true }));
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    if (loading) return;

    setTouched({ user: true, password: true });
    setServerError("");
    if (errors.user || errors.password) return;

    setLoading(true);
    try {
      // Evita que un token vencido se envíe en la petición de login
      localStorage.removeItem("token");

      const data = await login({ user: form.user, password: form.password });
      if (!data?.token) throw new Error("Respuesta sin token");

      // El interceptor de dashboardApi lee "token" de localStorage
      localStorage.setItem("token", data.token);
      localStorage.setItem(
        "session",
        JSON.stringify({
          idUsuario: data.idUsuario,
          nombreUsuario: data.nombreUsuario,
          email: data.email,
          rol: data.rol,
          expira: data.expira,
        })
      );

      navigate("/admin", { replace: true });
    } catch (err) {
      setServerError(getServerError(err));
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className={styles.root}>
      <main className="container-fluid p-0">
        <div className="row g-0">
          
          <section className={`col-lg-6 ${styles.hero} d-flex flex-column`} aria-labelledby="hero-title">
            

            <div className="my-auto py-5">
              
              <img src={logo} alt="cineplanet logo"  width="300" className="mb-4"/>
              <p className={`${styles.lead} mb-4`}>
                Aplicación web para la administración centralizada de clientes, productos de confitería y comprobantes de venta, integrada con análisis de métricas de rendimiento.
              </p>

              
            </div>

            
          </section>

         
          <section className="col-lg-6 d-flex flex-column align-items-center px-4 px-md-5 py-5" aria-labelledby="login-title">
            <div className={`${styles.formWrap} my-auto`}>
              <nav aria-label="Navegación secundaria" className="mb-5">
                <a href="/cartelera" className={styles.back}>
                  {/* <IconArrowLeft /> Volver a cartelera */}
                </a>
              </nav>

              <header className="mb-4">
                <h2 id="login-title" className={`${styles.heading} fw-bold display-6 mb-2`} style={{ fontWeight: 800 }}>
                  Iniciar sesión
                </h2>
                <p className={`${styles.muted} mb-0`}>
                  Ingresa con tu cuenta Socio Cineplanet para continuar.
                </p>
              </header>

              <form onSubmit={handleSubmit} noValidate>
                <div className="mb-3">
                  <label htmlFor="user" className={`${styles.label} d-block`}>nombre de usuario</label>
                  <div className={styles.inputGroup}>
                    <span className={styles.icon}><IconMail /></span>
                    <input
                      id="user" name="user" type="text"
                      className={`form-control ${styles.input} ${touched.user && errors.user ? styles.inputInvalid : ""}`}
                      placeholder="Tu usuario" autoComplete="username" maxLength={30}
                      value={form.user} onChange={handleChange} onBlur={handleBlur} required
                      aria-invalid={touched.user && !!errors.user}
                      aria-describedby={touched.user && errors.user ? "user-error" : undefined}
                    />
                  </div>
                  {touched.user && errors.user && (
                    <p id="user-error" className={styles.fieldError} role="alert">{errors.user}</p>
                  )}
                </div>

                <div className="mb-3">
                  {/* <div className="d-flex justify-content-between align-items-baseline">
                    <label htmlFor="password" className={styles.label}>Contraseña</label>
                    <a href="/recuperar" className={`${styles.link} small`}>¿Olvidaste tu contraseña?</a>
                  </div> */}
                  <div className={styles.inputGroup}>
                    <span className={styles.icon}><IconLock /></span>
                    <input
                      id="password" name="password" type={showPassword ? "text" : "password"}
                      className={`form-control ${styles.input} ${touched.password && errors.password ? styles.inputInvalid : ""}`}
                      placeholder="••••••••" aria-label="Contraseña"
                      autoComplete="current-password" maxLength={64}
                      value={form.password} onChange={handleChange} onBlur={handleBlur} required
                      aria-invalid={touched.password && !!errors.password}
                      aria-describedby={touched.password && errors.password ? "password-error" : undefined}
                    />
                    <button
                      type="button" className={styles.toggle}
                      onClick={() => setShowPassword((s) => !s)}
                      aria-label={showPassword ? "Ocultar contraseña" : "Mostrar contraseña"}
                      aria-pressed={showPassword}
                    >
                      {showPassword ? <IconEyeOff /> : <IconEye />}
                    </button>
                  </div>
                  {touched.password && errors.password && (
                    <p id="password-error" className={styles.fieldError} role="alert">{errors.password}</p>
                  )}
                </div>

                {serverError && (
                  <div className={`${styles.alertError} mb-3`} role="alert">{serverError}</div>
                )}


                <button type="submit" className={`btn ${styles.submit} w-100`} disabled={loading}>
                  {loading ? "Ingresando..." : "Ingresar a mi cuenta"}
                </button>
              </form>
{/* 
              <div className={`${styles.divider} my-4`} role="separator">
                <span>o continúa con</span>
              </div>

              <div className="text-center">
                <button type="button" className={`btn ${styles.google}`}>
                  <IconUserCircle /> Google
                </button>
              </div> */}

              <p className={`text-center ${styles.muted} mt-4 mb-0`}>
                ¿Aún no eres Socio?{" "}
                <a href="/registro" className={`${styles.link} fw-semibold`}>Regístrate gratis aquí</a>
              </p>
            </div>

            <footer className={`${styles.copy} mt-4 w-100 text-center`}>
              <small>© 2026 Cineplanet • Todos los derechos reservados</small>
            </footer>
          </section>
        </div>
      </main>
    </div>
  );
}