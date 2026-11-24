import { useState } from "react";
import styles from "./Hero.module.css";

function PlayIcon() {
  return (
    <svg width="12" height="12" viewBox="0 0 16 16" fill="currentColor" aria-hidden="true">
      <path d="M11.596 8.697l-6.363 3.692A.802.802 0 0 1 4 11.692V4.308c0-.634.69-1.03 1.233-.697l6.363 3.692a.802.802 0 0 1 0 1.394z" />
    </svg>
  );
}

function TrailerIcon() {
  return (
    <svg width="14" height="14" viewBox="0 0 16 16" fill="none" stroke="currentColor" strokeWidth="1.4" aria-hidden="true">
      <rect x="1.5" y="3" width="13" height="10" rx="1.5" />
      <path d="M1.5 6h13" />
    </svg>
  );
}

function ChevronIcon({ direction }) {
  const d = direction === "left" ? "M10 3L5 8l5 5" : "M6 3l5 5-5 5";
  return (
    <svg width="14" height="14" viewBox="0 0 16 16" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round" aria-hidden="true">
      <path d={d} />
    </svg>
  );
}

export default function Hero({ slides = [] }) {
  const [current, setCurrent] = useState(0);

  if (slides.length === 0) return null;

  const total = slides.length;
  const prev = () => setCurrent((i) => (i - 1 + total) % total);
  const next = () => setCurrent((i) => (i + 1) % total);
  const slide = slides[current];

  return (
    <section
      className={styles.hero}
      aria-roledescription="carrusel"
      aria-label="Estrenos destacados"
    >
      <article
        key={slide.id ?? current}
        aria-roledescription="diapositiva"
        aria-label={`${current + 1} de ${total}`}
        className="w-100"
      >
        <figure className={`${styles.backdrop} m-0`}>
          <img src={slide.image} alt="" />
        </figure>

        <div className={`${styles.content} container-fluid px-5 px-lg-6`}>
          <div className="px-lg-5">
            <p className={styles.tag}>[{slide.tag ?? "Estreno destacado"}]</p>
            <h1 className={styles.title}>{slide.title}</h1>
            <p className={styles.synopsis}>{slide.synopsis}</p>

            <div className={styles.actions}>
              <a href={slide.buyHref ?? "#"} className={`btn ${styles.buy}`}>
                <PlayIcon />
                Comprar entrada
              </a>
              <a href={slide.trailerHref ?? "#"} className={`btn ${styles.trailer}`}>
                <TrailerIcon />
                Ver tráiler
              </a>
            </div>
          </div>
        </div>
      </article>

      {total > 1 && (
        <>
          <button
            type="button"
            className={`${styles.arrow} ${styles.arrowPrev}`}
            onClick={prev}
            aria-label="Diapositiva anterior"
          >
            <ChevronIcon direction="left" />
          </button>
          <button
            type="button"
            className={`${styles.arrow} ${styles.arrowNext}`}
            onClick={next}
            aria-label="Diapositiva siguiente"
          >
            <ChevronIcon direction="right" />
          </button>

          <nav aria-label="Indicadores del carrusel">
            <ul className={styles.indicators}>
              {slides.map((s, i) => (
                <li key={s.id ?? i}>
                  <button
                    type="button"
                    className={`${styles.dot} ${i === current ? styles.dotActive : ""}`}
                    onClick={() => setCurrent(i)}
                    aria-label={`Ir a la diapositiva ${i + 1}`}
                    aria-current={i === current ? "true" : undefined}
                  />
                </li>
              ))}
            </ul>
          </nav>
        </>
      )}
    </section>
  );
}


